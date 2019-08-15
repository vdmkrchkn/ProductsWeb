namespace ProductsWebApi.Models.Views
{
    public class Product
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}
