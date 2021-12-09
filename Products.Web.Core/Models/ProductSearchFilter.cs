namespace Products.Web.Core.Models
{
    public class ProductSearchFilter
    {
        public ProductSearchFilter(string name, double? priceMin, double? priceMax)
        {
            Name = name;
            PriceMin = priceMin;
            PriceMax = priceMax;
        }

        public string Name { get; }
        public double? PriceMin { get; }
        public double? PriceMax { get; }
    }
}
