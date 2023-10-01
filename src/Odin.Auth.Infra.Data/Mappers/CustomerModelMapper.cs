using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValueObjects;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.Infra.Data.EF.Mappers
{
    public static class CustomerModelMapper
    {
        public static CustomerModel ToCustomerModel(this Customer customer)
        {
            return new CustomerModel
            (
                customer.Id,
                customer.Name,
                customer.Document,
                customer.Address?.StreetName,
                customer.Address?.StreetNumber,
                customer.Address?.Complement,
                customer.Address?.Neighborhood,
                customer.Address?.ZipCode,
                customer.Address?.City,
                customer.Address?.State,
                customer.IsActive,
                customer.CreatedAt ?? default,
                customer.CreatedBy ?? "",
                customer.LastUpdatedAt ?? default,
                customer.LastUpdatedBy ?? ""
            );
        }

        public static IEnumerable<CustomerModel> ToCustomerModel(this IEnumerable<Customer> customers)
            => customers.Select(ToCustomerModel);

        public static Customer ToCustomer(this CustomerModel model)
        {
            var customer = new Customer(model.Id, model.Name, model.Document, isActive: model.IsActive);
            
            if(!string.IsNullOrWhiteSpace(model.StreetName))
            { 
                var address = new Address(model.StreetName, model.StreetNumber ?? 0, model.Complement ?? "", model.Neighborhood!, model.ZipCode!, model.City!, model.State!);
                customer.ChangeAddress(address);
            }

            customer.SetAuditLog(model.CreatedAt, model.CreatedBy, model.LastUpdatedAt, model.LastUpdatedBy);

            return customer;
        }

        public static IEnumerable<Customer> ToCustomer(this IEnumerable<CustomerModel> models)
            => models.Select(ToCustomer);
    }
}
