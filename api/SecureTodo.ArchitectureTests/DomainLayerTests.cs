using ArchUnitNET.Fluent;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.TUnit;

namespace SecureTodo.ArchitectureTests;

internal class DomainLayerTests : BaseTest
{
    [Test]
    public void Domain_Entities_Should_Be_Sealed()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(DomainPattern)
            .And().AreAssignableTo(typeof(Domain.Base.Auditable))
            .And().AreNotAbstract()
            .Should().BeSealed();
           
        rule.Check(Architecture);
    }
    
    [Test]
    public void Domain_Value_Objects_Should_Be_Sealed_Records()
    {
        IArchRule sealedRule = Classes()
            .That().ResideInNamespaceMatching($"{DomainPattern}.ValueObjects")
            .Should().BeSealed();
        
        IArchRule recordRule = Classes()
            .That().ResideInNamespaceMatching($"{DomainPattern}.ValueObjects")
            .Should().BeRecord();
        
        sealedRule.Check(Architecture);
        recordRule.Check(Architecture);
    }
}