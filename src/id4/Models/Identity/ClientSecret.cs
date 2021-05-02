using System;

namespace id4.Models.Entities
{
    public class ClientSecret
    {
        public ClientSecret(){}
        public int Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }="SharedSecret";
        public DateTime Created { get; set; }
        public Client Client {get;set;}
    }
}