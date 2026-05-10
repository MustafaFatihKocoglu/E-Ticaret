using Microsoft.AspNetCore.Mvc;
using E_Ticaret.Models;

namespace E_Ticaret.Controllers
{
    public class CartController : Controller
    {
        private const string CartKey = "cart";

        private static readonly List<Product> _products = new()
        {
            new Product { Id=1, Name="Kulaklık", Description="Bluetooth, gürültü engelleme", Price=799, Stock=12, ImageUrl="/images/kulaklik.png" },
            new Product { Id=2, Name="Klavye", Description="Mekanik, RGB aydınlatma", Price=1299, Stock=7, ImageUrl="/images/klavye.png" },
            new Product { Id=3, Name="Mouse", Description="Gaming, 8.000 DPI", Price=599, Stock=20, ImageUrl="/images/mouse.png" },
            new Product { Id=4, Name="Monitör", Description="27 inch, 144Hz", Price=6499, Stock=5, ImageUrl="/images/monitor.png" },
        };

        private List<CartItem> GetCart()
            => HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();

        private void SaveCart(List<CartItem> cart)
            => HttpContext.Session.SetObject(CartKey, cart);

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                item.Quantity += 1;
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddJson(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { ok = false, message = "Ürün bulunamadı." });

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                item.Quantity += 1;
            }

            SaveCart(cart);

            var count = cart.Sum(x => x.Quantity);
            return Json(new { ok = true, message = "Sepete eklendi.", count });
        }

        [HttpGet]
        public IActionResult CountJson()
        {
            var cart = GetCart();
            var count = cart.Sum(x => x.Quantity);
            return Json(new { count });
        }

        [HttpPost]
        public IActionResult RemoveJson(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
                return Json(new { ok = false, message = "Ürün sepette yok." });

            cart.Remove(item);
            SaveCart(cart);

            var count = cart.Sum(x => x.Quantity);
            return Json(new { ok = true, message = "Ürün sepetten silindi.", count });
        }

        [HttpPost]
        public IActionResult IncreaseJson(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
                return Json(new { ok = false });

            item.Quantity += 1;
            SaveCart(cart);

            return Json(new
            {
                ok = true,
                qty = item.Quantity,
                total = cart.Sum(x => x.Price * x.Quantity)
            });
        }

        [HttpPost]
        public IActionResult DecreaseJson(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
                return Json(new { ok = false });

            item.Quantity -= 1;

            if (item.Quantity <= 0)
                cart.Remove(item);

            SaveCart(cart);

            return Json(new
            {
                ok = true,
                qty = item.Quantity,
                total = cart.Sum(x => x.Price * x.Quantity)
            });
        }


    }
}
