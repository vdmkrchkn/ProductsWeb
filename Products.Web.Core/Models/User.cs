namespace Products.Web.Core.Models
{
    public class User
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public override string ToString() => $"{Name}: {Password}";
    }
}
