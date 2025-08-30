using FinStack.API.Tests.Factories;

namespace FinStack.API.Tests.Endpoints;

[CollectionDefinition(Definition, DisableParallelization = true)]
public class EndpointCollection : ICollectionFixture<TestWebApplicationFactory>
{
    public const string Definition = "APIEndpointTests";
}