using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BankApp.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
using bankApp.Services;
using Microsoft.CodeAnalysis.CSharp;

namespace BankApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger; 
        IAccountService  accountService;

        public AccountsController(IAccountService _accountService, ILogger<AccountsController> logger)
        {
            _logger = logger;
            accountService = _accountService;
        }

     

        // GET: Student
        public async Task<ActionResult> Index()
        { 
            var accounts =  await  accountService.getUserAccounts("User_1") ; 
            return View(accounts);
        }


        // GET: Student/Details/5
        public async Task<ActionResult> Details(string accountnumber)
        {
            if (string.IsNullOrEmpty(accountnumber))
            {
                return BadRequest();
            }
            var account = await accountService.getAccount(accountnumber) ;
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
        public async Task<ActionResult> Create(Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _account = await  accountService.createAccount(account) ; 
                    return RedirectToAction("Index");
                }
            }
            catch ( Exception e)
            { 
                ModelState.AddModelError("", "Unable to save changes.");
            }
            return View(account);
        }


        // GET: Student/Edit/5
        public async Task<ActionResult> Edit(string  accountnumber)
        {
            if ( string.IsNullOrEmpty(accountnumber))
            {
                return   BadRequest();
            }
            var _account = await accountService.getAccount(accountnumber);
             
            if (_account == null)
            {
                return NotFound();
            }  
            return View(_account);
        }

        
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(Account  account  )
        {
            if (account is null)
            {
                return BadRequest();
            }
         

            try
            {
                var _account = await accountService.updateAccount(account) ;
                return RedirectToAction("Index");
             }
            catch (Exception e)
            {   
                ModelState.AddModelError("", "Unable to save changes.");
            }
            var accountToUpdate = accountService.getAccount(account.accountnumber);
            return View(accountToUpdate);
        }

        // GET: Student/Delete/5
        public async Task<ActionResult> Delete(string  accountnumber, bool? saveChangesError = false)
        {
            if (string.IsNullOrEmpty(accountnumber))
            {
                return BadRequest();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.";
            }
            var _account = await accountService.getAccount(accountnumber) ; 
            if (_account == null)
            {
                return NotFound();
            }
            return View(_account);
        }

         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string accountnumber)
        {
            if (string.IsNullOrEmpty(accountnumber))
            {
                return BadRequest();
            } 
            try
            {
                var _IsDeeleted = await accountService.deleteAccount(accountnumber) ; 
                if ( _IsDeeleted < 1  )
                { 
                    return BadRequest();
                }
              
            }
            catch (Exception e)
            {
                
                return RedirectToAction("Delete", new { accountnumber = accountnumber, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        
    }
}
