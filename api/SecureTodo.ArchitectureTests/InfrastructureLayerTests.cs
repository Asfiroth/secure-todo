using ArchUnitNET.Fluent;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.TUnit;

namespace SecureTodo.ArchitectureTests;

internal class InfrastructureLayerTests : BaseTest
{
    [Test]
    public void Repository_Implementations_Should_Be_Internal_And_Sealed()
    {
        IArchRule internalRule = Classes()
            .That().ResideInNamespace($"{InfrastructureNamespace}.Repositories")
            .Should().BeInternal();
        
        IArchRule sealedRule = Classes()
            .That().ResideInNamespace($"{InfrastructureNamespace}.Repositories")
            .Should().BeSealed();
        
        internalRule.Check(Architecture);
        sealedRule.Check(Architecture);
    }
    
    [Test]
    public void Query_Service_Implementations_Should_Be_Internal_And_Sealed()
    {
        IArchRule internalRule = Classes()
            .That().ResideInNamespace($"{InfrastructureNamespace}.QueryServices")
            .Should().BeInternal();
        
        IArchRule sealedRule = Classes()
            .That().ResideInNamespace($"{InfrastructureNamespace}.QueryServices")
            .Should().BeSealed();
        
        internalRule.Check(Architecture);
        sealedRule.Check(Architecture);
    }
    
    [Test]
    public void Entity_Configurations_Should_Be_Sealed()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespace($"{InfrastructureNamespace}.Data.Configurations")
            .Should().BeSealed();
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Entity_Configurations_Should_Implement_IEntityTypeConfiguration()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespace($"{InfrastructureNamespace}.Data.Configurations")
            .Should().ImplementInterface(typeof(Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<>));
        
        rule.Check(Architecture);
    }
    
    
    [Test]
    public void Only_Infrastructure_Should_Depend_On_DbContext()
    {
        IArchRule apiRule = Types()
            .That().ResideInNamespaceMatching(ApiPattern)
            .Should().NotDependOnAny(
                Types().That().AreAssignableTo(typeof(Microsoft.EntityFrameworkCore.DbContext)));

        IArchRule appRule = Types()
            .That().ResideInNamespaceMatching(ApplicationPattern)
            .Should().NotDependOnAny(
                Types().That().AreAssignableTo(typeof(Microsoft.EntityFrameworkCore.DbContext)));
        
        apiRule.Check(Architecture);
        appRule.Check(Architecture);
    }
}