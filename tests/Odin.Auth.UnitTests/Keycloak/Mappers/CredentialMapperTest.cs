
using FluentAssertions;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Mappers;
using Odin.Auth.Infra.Keycloak.Models;

namespace Odin.Auth.UnitTests.Keycloak.Mappers
{
    [Collection(nameof(MapperTestFixtureCollection))]
    public class CredentialMapperTest
    {
        private readonly MapperTestFixture _fixture;

        public CredentialMapperTest(MapperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ToCredentialRepresentation() should map a UserCredential to CredentialRepresentation type")]
        [Trait("Keycloak", "Mappers / CredentialMapper")]
        public void ToCredentialRepresentation()
        {
            var credential = new UserCredential("password", true);

            var credentialRepresentation = credential.ToCredentialRepresentation();

            credentialRepresentation.Should().NotBeNull();
            credentialRepresentation.Type.Should().Be("password");
            credentialRepresentation.Value.Should().Be(credential.Value);
            credentialRepresentation.Temporary.Should().Be(credential.Temporary);
        }


        [Fact(DisplayName = "ToCredentialRepresentation() should map a list of Credentials to a list of CredentialRepresentation type")]
        [Trait("Keycloak", "Mappers / CredentialMapper")]
        public void ToCredentialRepresentationList()
        {
            var credential1 = new UserCredential("password01", true);
            var credential2 = new UserCredential("password02", true);

            var credentials = new List<UserCredential> { credential1, credential2 };

            var credentialsRepresentation = credentials!.ToCredentialRepresentation().ToList();

            credentialsRepresentation[0].Should().NotBeNull();
            credentialsRepresentation[0].Type.Should().Be("password");
            credentialsRepresentation[0].Value.Should().Be(credential1.Value);
            credentialsRepresentation[0].Temporary.Should().Be(credential1.Temporary);

            credentialsRepresentation[1].Should().NotBeNull();
            credentialsRepresentation[1].Type.Should().Be("password");
            credentialsRepresentation[1].Value.Should().Be(credential2.Value);
            credentialsRepresentation[1].Temporary.Should().Be(credential2.Temporary);
        }


        [Fact(DisplayName = "ToUserCredential() should map a CredentialRepresentation to Credential")]
        [Trait("Keycloak", "Mappers / CredentialMapper")]
        public void ToCredential()
        {
            var credentialRepresentation = new CredentialRepresentation
            {
                Id = Guid.NewGuid(),
                Type = "password",
                Value = "password",
                Temporary = true
            };

            var credential = credentialRepresentation.ToUserCredential();

            credential.Should().NotBeNull();
            credential.Type.Should().Be(credentialRepresentation.Type);
            credential.Value.Should().Be(credentialRepresentation.Value);
            credential.Temporary.Should().Be(credentialRepresentation.Temporary!.Value);
        }       

        [Fact(DisplayName = "ToUserCredential() should map a list of CredentialRepresentation to list of Credential")]
        [Trait("Keycloak", "Mappers / CredentialMapper")]
        public void ToCredentialWithGroupsList()
        {
            var credentialRepresentation1 = new CredentialRepresentation
            {
                Id = Guid.NewGuid(),
                Type = "password",
                Value = "password",
                Temporary = true
            };

            var credentialRepresentation2 = new CredentialRepresentation
            {
                Id = Guid.NewGuid(),
                Type = "password",
                Value = "password1",
                Temporary = true
            };

            var credentialRepresentations = new List<CredentialRepresentation> { credentialRepresentation1, credentialRepresentation2 };

            var credentials = credentialRepresentations.ToUserCredential().ToList();

            credentials.Should().NotBeNull();
            credentials.Should().HaveCount(2);

            credentials[0].Should().NotBeNull();
            credentials[0].Type.Should().Be(credentialRepresentation1.Type);
            credentials[0].Value.Should().Be(credentialRepresentation1.Value);
            credentials[0].Temporary.Should().Be(credentialRepresentation1.Temporary!.Value);
            
            credentials[1].Should().NotBeNull();
            credentials[1].Type.Should().Be(credentialRepresentation2.Type);
            credentials[1].Value.Should().Be(credentialRepresentation2.Value);
            credentials[1].Temporary.Should().Be(credentialRepresentation2.Temporary!.Value);
        }
    }
}
