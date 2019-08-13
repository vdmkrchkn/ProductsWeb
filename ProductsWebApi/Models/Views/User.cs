namespace ProductsWebApi.Models.Views
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public override string ToString() => $"{Username}: {Password}";
    }
}
