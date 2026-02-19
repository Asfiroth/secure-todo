namespace SecureTodo.ArchitectureTests;
using ArchUnitNET.Fluent;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.TUnit;

internal class ApiLayerTests : BaseTest
{
    [Test]
    public void Endpoint_Classes_Should_Be_Sealed()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(@"SecureTodo\.Api\.Endpoints(\..+)?")
            .And().AreAssignableTo(typeof(Api.Endpoints.IEndpoint))
            .Should().BeSealed();
        
        rule.Check(Architecture);
    }
    
    [Test]
    public void Endpoint_Classes_Should_Implement_IEndpoint()
    {
        IArchRule rule = Classes()
            .That().ResideInNamespaceMatching(@"SecureTodo\.Api\.Endpoints(\..+)?")
            .And().AreNotAbstract()
            .And().AreNotRecord()
            .And().DoNotHaveNameStartingWith("RouteNames")
            .Should().ImplementInterface(typeof(Api.Endpoints.IEndpoint));
        
        rule.Check(Architecture);
    }
}