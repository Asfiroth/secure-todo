using ArchUnitNET.Domain;
using ArchUnitNET.Loader;

namespace SecureTodo.ArchitectureTests;

internal class BaseTest
{
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            typeof(Api.Program).Assembly,
            typeof(Domain.IAssemblyMarker).Assembly,
            typeof(Application.IAssemblyMarker).Assembly,
            typeof(Infrastructure.IAssemblyMarker).Assembly
        ).Build();
    
    protected const string ApiNamespace = "SecureTodo.Api";
    protected const string DomainNamespace = "SecureTodo.Domain";
    protected const string ApplicationNamespace = "SecureTodo.Application";
    protected const string InfrastructureNamespace = "SecureTodo.Infrastructure";
    
    protected const string ApiPattern = @"SecureTodo\.Api(\..+)?";
    protected const string DomainPattern = @"SecureTodo\.Domain(\..+)?";
    protected const string ApplicationPattern = @"SecureTodo\.Application(\..+)?";
    protected const string InfrastructurePattern = @"SecureTodo\.Infrastructure(\..+)?";
}