namespace ProductsWebAdmin.Models
{
    public class Product : ProductBase
    {
        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}
