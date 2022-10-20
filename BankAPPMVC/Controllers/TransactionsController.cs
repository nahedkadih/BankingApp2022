using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BankApp.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Humanizer;
using Microsoft.Data.SqlClient;
using System.Linq;
using bankApp.Services;
using System.Collections.Generic;

namespace BankApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ILogger<TransactionsController> _logger;
        ITransactionsService  transactionsService;
        IAccountService accountService;
        public TransactionsController(ITransactionsService _transactionsService, IAccountService _accountService , ILogger<TransactionsController> logger)
        {
            _logger = logger;
            transactionsService = _transactionsService;
            accountService = _accountService;
        }

        // GET: Student
        public async Task<ActionResult> Index()
        {
            IEnumerable<TransactionView> result = await transactionsService.getUserTransactions("User_1");
            return View(result);
        }
          
     
        public async Task<ActionResult> Create()
        {
            IEnumerable<Account> _acccounts = await accountService.getUserAccounts("User_1");  
            TransferView _TransferView = new TransferView();
            _TransferView.fromAccount = _acccounts;
            _TransferView.toAccount = _acccounts;  
            return View(_TransferView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TransferView transfer)
        {
            try
            {
                if (transfer.SelectedfromAccount == transfer.SelectedtoAccount)
                {
                    ModelState.AddModelError("SameAccount", "You can not transfer to the same account."); 
                } 
                if (ModelState.IsValid)
                {
                    Account  accountfrom = await accountService.getAccount(transfer.SelectedfromAccount);
                    Account accountto = await accountService.getAccount(transfer.SelectedtoAccount);
                    
                    if (accountfrom == accountto  )
                    {
                        ModelState.AddModelError("SameAccount", "You can not transfer to the same account.");


                    }
                    if (ModelState.IsValid) 
                    {
                        if (accountfrom != null && accountto != null)
                        {
                            accountfrom.balance = accountfrom.balance - transfer.amount;
                            accountto.balance = accountto.balance + transfer.amount;
                            Transaction trans = new Transaction(accountfrom.accountnumber, accountto.accountnumber, transfer.amount);
                            await transactionsService.createTransaction(trans);
                        }
                        return RedirectToAction("Index");
                    }
                   

                }

                IEnumerable<Account> _acccounts = await accountService.getUserAccounts("User_1"); 
                transfer.fromAccount = _acccounts;
                transfer.toAccount = _acccounts;
                return View(transfer);
            }
            catch (Exception e)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                return BadRequest();
            }
            var trans = await transactionsService.getTransaction(transactionId);
            if (trans == null)
            {
                return NotFound();
            }
            return View(trans);
        }
 
    }
}
