using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.TUnit;

namespace SecureTodo.ArchitectureTests;

internal class ArchitectureTests : BaseTest
{
    [Test]
    public void Domain_Should_Not_Have_Dependency_On_Other_Projects()
    {
        IArchRule rule = Types().That().ResideInNamespaceMatching(DomainPattern)
            .Should().NotDependOnAny(Types().That().ResideInNamespaceMatching(ApplicationPattern))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespaceMatching(InfrastructurePattern))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespaceMatching(ApiPattern));

        rule.Check(Architecture);
    }
    
    [Test]
    public void Application_Should_Not_Have_Dependency_On_Infrastructure_Or_Host()
    {
        IArchRule rule = Types().That().ResideInNamespaceMatching(ApplicationPattern)
            .Should().NotDependOnAny(Types().That().ResideInNamespaceMatching(InfrastructurePattern))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespaceMatching(ApiPattern));

        rule.Check(Architecture);
    }
    
    [Test]
    public void Infrastructure_Should_Not_Have_Dependency_On_Host()
    {
        IArchRule rule = Types().That().ResideInNamespaceMatching(InfrastructurePattern)
            .Should().NotDependOnAny(Types().That().ResideInNamespaceMatching(ApiPattern));

        rule.Check(Architecture);
    }

    [Test]
    public void Application_Interfaces_Should_Start_With_I()
    {
        IArchRule rule = Interfaces()
            .That()
            .ResideInNamespaceMatching(ApplicationPattern)
            .Should()
            .HaveNameStartingWith("I");
	    
        rule.Check(Architecture);
    }
    
}
