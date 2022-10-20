using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BankApp.Data;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BankContext db;

        public HomeController(ILogger<HomeController> logger, BankContext context)
        {
            db = context;
            _logger = logger;
        }
       
        
        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    transaction.date_created  = DateTime.Now;
                    db.Add(transaction);
                    await db.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        db.Update(transaction);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return NotFound();
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderViewToString(this, "_ViewAll", db.Transactions.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderViewToString(this, "AddOrEdit", transaction) });
        }
    }
}