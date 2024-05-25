using Fiorella.ViewModels.Baskets;
using Fiorello.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fiorella.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _context;
        public CartController(IHttpContextAccessor contextAccessor, AppDbContext context)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<IActionResult> Index()
        {


            List<BasketVM> basketProducts = null;

            if (_contextAccessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                basketProducts = new List<BasketVM>();
            }




            var products = await _context.Products.Include(m => m.Category)
                                                      .Include(m => m.ProductImage)
                                                      .ToListAsync();

            List<BasketProductsVM> basket = new();

            foreach (var item in basketProducts)
            {
                var contextProduct = products.FirstOrDefault(m => m.Id == item.Id);
                basket.Add(new BasketProductsVM
                {
                    Id = contextProduct.Id,
                    Name = contextProduct.Name,
                    Description = contextProduct.Description,
                    Price = contextProduct.Price,
                    Image = contextProduct.ProductImage.FirstOrDefault(m => m.IsMain).Name,
                    Category = contextProduct.Category.Name,
                    Count = item.Count
                });
            }


            CartVM response = new()
            {
                BasketProducts = basket,
                Total = basketProducts.Sum(m => m.Count * m.Price)
            };

            return View(response);
        }

        [HttpPost]
        public IActionResult DeleteProductFromBasket(int? id)
        {


            List<BasketVM> basketProducts = new();

            if (_contextAccessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }


            basketProducts = basketProducts.Where(m => m.Id != id).ToList();

            _contextAccessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

            int count = basketProducts.Sum(m => m.Count);
            decimal total = basketProducts.Sum(m => m.Count * m.Price);

            return Ok(new { count, total });


        }
    }
}
