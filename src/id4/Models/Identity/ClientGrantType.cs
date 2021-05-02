
namespace id4.Models.Entities
{
    public class ClientGrantType
    {
        public ClientGrantType(){}

        public int Id { get; set; }
        public string GrantType { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}