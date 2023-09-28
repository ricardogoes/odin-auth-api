using Moq;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;

namespace Odin.Auth.UnitTests.Application.Customers
{
    public abstract class CustomerBaseFixture : BaseFixture
    {
        public Mock<IDocumentService> GetDocumentServiceMock()
            => new();
        public Mock<ICustomerRepository> GetCustomerRepositoryMock()
            => new();                

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();

        public static string GetInvalidUsersername()
            => "";

        public static bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        
    }
}
