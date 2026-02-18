using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.TUnit;

namespace SecureTodo.ArchitectureTests;

internal class ArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            typeof(Api.Program).Assembly,
            typeof(Domain.IAssemblyMarker).Assembly,
            typeof(Application.IAssemblyMarker).Assembly,
            typeof(Infrastructure.IAssemblyMarker).Assembly
        ).Build();
    
    private const string ApiNamespace = "SecureTodo.Api";
    private const string DomainNamespace = "SecureTodo.Domain";
    private const string ApplicationNamespace = "SecureTodo.Application";
    private const string InfrastructureNamespace = "SecureTodo.Infrastructure";
    
    [Test]
    public void Domain_Should_Not_Have_Dependency_On_Other_Projects()
    {
        IArchRule rule = Types().That().ResideInNamespace(DomainNamespace)
            .Should().NotDependOnAny(Types().That().ResideInNamespace(ApplicationNamespace))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace(InfrastructureNamespace))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace(ApiNamespace));

        rule.Check(Architecture);
    }
    
    [Test]
    public void Application_Should_Not_Have_Dependency_On_Infrastructure_Or_Host()
    {
        IArchRule rule = Types().That().ResideInNamespace(ApplicationNamespace)
            .Should().NotDependOnAny(Types().That().ResideInNamespace(InfrastructureNamespace))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace(ApiNamespace));

        rule.Check(Architecture);
    }
    
    [Test]
    public void Infrastructure_Should_Not_Have_Dependency_On_Host()
    {
        IArchRule rule = Types().That().ResideInNamespace(InfrastructureNamespace)
            .Should().NotDependOnAny(Types().That().ResideInNamespace(ApiNamespace));

        rule.Check(Architecture);
    }
    
    [Test]
    public void Application_Repository_Interfaces_Should_Start_With_I()
    {
        IArchRule rule = Interfaces()
            .That()
            .ResideInNamespace($"{ApplicationNamespace}")
            .Should()
            .HaveNameStartingWith("I");
	    
        rule.Check(Architecture);
    }
    

}