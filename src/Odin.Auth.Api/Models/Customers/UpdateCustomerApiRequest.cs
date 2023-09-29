namespace Odin.Baseline.Api.Models.Customers
{
    public class UpdateCustomerApiRequest
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }

        public UpdateCustomerApiRequest(Guid id, string name, string document)
        {
            Id = id;
            Name = name;
            Document = document;
        }
    }
}
