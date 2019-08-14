namespace ProductsWebApi.Models
{
    public struct AuthOptions
    {
        // token publisher
        public string Issuer { get; set; }

        public string Key { get; set; }

        // token time to life in minutes
        public int Lifetime { get; set; }
    }
}
