namespace id4.Models.Entities
{
    public class ClientScope
    {
        public ClientScope(){}

        public ClientScope(string scope){Scope=scope;}

        public int Id { get; set; }
        public string Scope { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}