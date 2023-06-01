using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TheoryPractice.Context;
using TheoryPractice.Models;

namespace TheoryPractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext _northwindContext; //newleme işlemini program.cs içinde yaptık.(Dependecy injection)
        public HomeController(ILogger<HomeController> logger,NorthwindContext northwindContext)//program.cs içinde newlediğimizi burada input olarak verdik.
        {
            _logger = logger;
            //SOLID (D-Dependency Injection)
            _northwindContext = northwindContext;
        }

        public IActionResult Index()
        {
            return View(_northwindContext.Products.FirstOrDefault());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void Test1()
        {
            //_northwindContext.Products.Where(prod => prod.ProductId == 1).FirstOrDefault();
            _northwindContext.Products.Where(KontrolEt).FirstOrDefault();
            
        }
        public bool KontrolEt(Product product)
        {
            return product.ProductId== 1;
        }
    }
}
