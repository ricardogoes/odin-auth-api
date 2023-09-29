using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValueObjects;

namespace Odin.Auth.Application.Customers
{
    public class CustomerOutput
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }
        public Address? Address { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public CustomerOutput(Guid id, string name, string document, Address? address, bool isActive,
            DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            Id = id;
            Name = name;
            Document = document;
            Address = address;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
        }


        public static CustomerOutput FromCustomer(Customer customer)
        {
            return new CustomerOutput
            (
                customer.Id,
                customer.Name,
                customer.Document,
                customer.Address,
                customer.IsActive,
                customer.CreatedAt ?? default,
                customer.CreatedBy ?? "",
                customer.LastUpdatedAt ?? default,
                customer.LastUpdatedBy ?? ""

            );
        }

        public static IEnumerable<CustomerOutput> FromCustomer(IEnumerable<Customer> customers)
            => customers.Select(FromCustomer);
    }
}
