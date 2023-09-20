namespace Odin.Auth.UnitTests.Keycloak.Mappers
{
    [CollectionDefinition(nameof(MapperTestFixtureCollection))]
    public class MapperTestFixtureCollection : ICollectionFixture<MapperTestFixture>
    { }

    public class MapperTestFixture : BaseFixture
    {
        public MapperTestFixture()
            : base() { }
    }
}
