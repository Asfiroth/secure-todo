using ArchUnitNET.Fluent;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.TUnit;

namespace SecureTodo.ArchitectureTests;

internal class ApplicationLayerTests : BaseTest
{
    [Test]
    public void Commands_Should_Have_Name_Ending_With_Command()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.ICommand<>))
            .Should().HaveNameEndingWith("Command");
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Command_Handlers_Should_Have_Name_Ending_With_CommandHandler()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.ICommandHandler<,>))
            .Should().HaveNameEndingWith("CommandHandler");
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Queries_Should_Have_Name_Ending_With_Query()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.IQuery<>))
            .Should().HaveNameEndingWith("Query");
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Query_Handlers_Should_Have_Name_Ending_With_QueryHandler()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.IQueryHandler<,>))
            .Should().HaveNameEndingWith("QueryHandler");
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Handlers_Should_Reside_In_UseCases_Namespace()
    {
        IArchRule commandHandlerRule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.ICommandHandler<,>))
            .Should().ResideInNamespaceMatching(@"SecureTodo\.Application\.UseCases(\..+)?");
        
        IArchRule queryHandlerRule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.IQueryHandler<,>))
            .Should().ResideInNamespaceMatching(@"SecureTodo\.Application\.UseCases(\..+)?");
        
        commandHandlerRule.Check(Architecture);
        queryHandlerRule.Check(Architecture);
    }
    
    [Test]
    public void Commands_Should_Be_Sealed()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.ICommand<>))
            .Should().BeSealed();
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Queries_Should_Be_Sealed()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.IQuery<>))
            .Should().BeSealed();
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Command_Handlers_Should_Be_Sealed()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.ICommandHandler<,>))
            .Should().BeSealed();
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Query_Handlers_Should_Be_Sealed()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(Mediator.IQueryHandler<,>))
            .Should().BeSealed();
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Validators_Should_Have_Name_Ending_With_Validator()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(FluentValidation.IValidator))
            .Should().HaveNameEndingWith("Validator");
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Validators_Should_Reside_In_Validators_Namespace()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .And().AreAssignableTo(typeof(FluentValidation.IValidator))
            .Should().ResideInNamespace($"{ApplicationNamespace}.Validators");
        
        rule.Check(Architecture);
    }
}