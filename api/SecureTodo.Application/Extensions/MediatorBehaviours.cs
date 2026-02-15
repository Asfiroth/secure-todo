using System.Diagnostics;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;

namespace SecureTodo.Application.Extensions;

public class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
    where TResponse : IResult
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
	    ArgumentNullException.ThrowIfNull(message);

	    if (_logger.IsEnabled(LogLevel.Information))
	    {
		    _logger.LogInformation("Handling message of type {MessageType}", typeof(TRequest).Name);

		    var type = message.GetType();
           
		    foreach (var property in type.GetProperties())
		    {
			    _logger.LogInformation("Property {Property} : {Value}", property.Name, property.GetValue(message));
		    }
	    }
       
	    var stopwatch = Stopwatch.StartNew();
       
	    var response = await next(message, cancellationToken);
       
	    stopwatch.Stop();

	    _logger.LogInformation(
		    "Handled message of type {MessageType} with response {Response} in {ElapsedTime} ms",
		    typeof(TRequest).Name, 
		    response, 
		    stopwatch.ElapsedMilliseconds);

	    return response;
    }
}

public sealed class ExceptionHandlingBehaviour<TRequest, TResponse>
	: MessageExceptionHandler<TRequest, TResponse>
	where TRequest : notnull, IMessage
	where TResponse : IResult
{
	private readonly ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> _logger;

	public ExceptionHandlingBehaviour(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}


	protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(TRequest message, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError(exception, "Error {ExceptionType} when handling message of type {MessageType}", exception.GetType().Name, typeof(TRequest).Name);

		string[] errorMessages = [exception.Message];
		
#nullable disable
		if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
		{
			var resultType = typeof(TResponse).GetGenericArguments()[0];
			var invalidMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod(nameof(Result<int>.CriticalError), [typeof(string[])]);

			if (invalidMethod is not null)
			{
				return Handled((TResponse)invalidMethod.Invoke(null, [errorMessages]));
			}
		}

		if (typeof(TResponse) == typeof(Result))
		{
			 return Handled((TResponse)(object)Result.CriticalError(errorMessages));
		}

		throw exception;
	}
}

public class ValidationBehaviour<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull, IMessage
	where TResponse : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehaviour()
	{
		_validators = [];
	}

	public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any()) return await next(message, cancellationToken);
        
		var context = new ValidationContext<TRequest>(message);
		var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
		var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

		var resultErrors = validationResults.SelectMany(v => v.AsErrors()).ToList();
        
		if (failures.Count == 0) return await next(message, cancellationToken);
#nullable disable         
		if(typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
		{
			var resultType = typeof(TResponse).GetGenericArguments()[0];
			var invalidMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod(nameof(Result<int>.Invalid), [typeof(List<ValidationError>)]);

			if (invalidMethod is not null)
			{
				return (TResponse)invalidMethod.Invoke(null, [resultErrors]);
			}
		} 
        
		if (typeof(TResponse) == typeof(Result))
		{
			return (TResponse)(object)Result.Invalid(resultErrors);
		}

		throw new ValidationException(failures);
	}
}