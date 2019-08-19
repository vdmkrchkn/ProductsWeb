namespace ProductsWebApi.Models.Json
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public override string ToString() => $"{Username}: {Password}";
    }
}
