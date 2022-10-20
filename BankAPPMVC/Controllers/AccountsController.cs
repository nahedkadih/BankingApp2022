using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BankApp.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly BankContext db;
        public AccountsController(BankContext context)
        {
            db = context;
        }

        // GET: Student
        public ViewResult Index()
        {
              

            var accounts = from s in db.Accounts
                           select s;
             
            return View(accounts);
        }


        // GET: Student/Details/5
        public ActionResult Details(string accountnumber)
        {
            if (string.IsNullOrEmpty(accountnumber))
            {
                return BadRequest();
            }
            var account = db.Accounts.Find(accountnumber);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch ( Exception e)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(account);
        }


        // GET: Student/Edit/5
        public IActionResult Edit(string  accountnumber)
        {
            if ( string.IsNullOrEmpty(accountnumber))
            {
                return   BadRequest();
            }
            var account = db.Accounts.Find(accountnumber);
            if (account == null)
            {
                return NotFound();
            }  
            return View(account);
        }

        
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(Account  account  )
        {
            if (account is null)
            {
                return BadRequest();
            }
         

              try
              {
                db.Accounts.Update(account);
                db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            var accountToUpdate = db.Accounts.Find(account.accountnumber);
            return View(accountToUpdate);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string  accountnumber, bool? saveChangesError = false)
        {
            if (string.IsNullOrEmpty(accountnumber))
            {
                return BadRequest();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var account = db.Accounts.Find(accountnumber);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string accountnumber)
        {
            if (string.IsNullOrEmpty(accountnumber))
            {
                return BadRequest();
            }
            try
            {
                var account = db.Accounts.Find(accountnumber);
                if (account !=null)
                {
                    db.Accounts.Remove(account);
                    db.SaveChanges();

                } else
                {
                    return BadRequest();
                }
              
            }
            catch (Exception e)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { accountnumber = accountnumber, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
