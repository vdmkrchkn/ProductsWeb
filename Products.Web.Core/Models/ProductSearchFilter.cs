namespace Products.Web.Core.Models
{
    public class ProductSearchFilter
    {
        public ProductSearchFilter(string name, decimal? priceMin, decimal? priceMax)
        {
            Name = name;
            PriceMin = priceMin;
            PriceMax = priceMax;
        }

        public string Name { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
    }
}
