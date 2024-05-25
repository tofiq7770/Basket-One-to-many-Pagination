using Fiorello.Models;

namespace Fiorello.ViewModels
{
    public class ProductVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
    }
}
