using E_Ticaret.Models;
using Microsoft.AspNetCore.Mvc;

public class ProductsController : Controller
{
    private static readonly List<Product> _products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Kulaklık",
            Description = "Bluetooth, gürültü engelleme",
            Price = 799,
            Stock = 12,
            ImageUrl = "/images/kulaklik.png"
        },
        new Product
        {
            Id = 2,
            Name = "Klavye",
            Description = "Mekanik, RGB aydınlatma",
            Price = 1299,
            Stock = 7,
            ImageUrl = "/images/klavye.png"
        },
        new Product
        {
            Id = 3,
            Name = "Mouse",
            Description = "Gaming, 8.000 DPI",
            Price = 599,
            Stock = 20,
            ImageUrl = "/images/mouse.png"
        },
        new Product
        {
            Id = 4,
            Name = "Monitör",
            Description = "27 inch, 144Hz",
            Price = 6499,
            Stock = 5,
            ImageUrl = "/images/monitor.png"
        }
    };

    public IActionResult Index()
    {
        return View(_products);
    }

    public IActionResult Details(int id)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        if (product == null)
            return NotFound();

        return View(product);
    }
}
