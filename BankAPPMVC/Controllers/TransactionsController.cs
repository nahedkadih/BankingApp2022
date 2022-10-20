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

namespace BankApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BankContext db;

        public TransactionsController(ILogger<HomeController> logger, BankContext context)
        {
            db = context;
            _logger = logger;
        }

        // GET: Student
        public ViewResult Index()
        {
            var result = from p1 in db.Transactions
                         join p0 in db.Users on p1.userid equals p0.userId
                         join p2 in db.Accounts on p1.from_account equals p2.accountnumber
                         join p3 in db.Accounts on p1.to_account  equals  p3.accountnumber
                         select new TransactionView
                         {
                             FromAccount = p2.accountname,
                             ToAccount = p3.accountname,
                             TransactionTime = p1.date_created,
                             AmountDebited = p1.amount,
                             FromAccountBalance = p2.balance,
                             ToAccountBalance = p3.balance,
                         };

            List<TransactionView> _model = result.ToList();  
            return View(_model);
        }
          
     
        public ActionResult Create()
        {
            IQueryable<Account> _acccounts = from s in db.Accounts
                                             select s;

            TransferView _TransferView = new TransferView();
            _TransferView.fromAccount = _acccounts;
            _TransferView.toAccount = _acccounts;
            var len = _acccounts.ToList();
            return View(_TransferView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransferView transfer)
        {
            try
            {
                if (transfer.SelectedfromAccount == transfer.SelectedtoAccount)
                {
                    ModelState.AddModelError("SameAccount", "You can not transfer to the same account."); 
                } 
                if (ModelState.IsValid)
                {

                    var accountfrom = db.Accounts.Find(transfer.SelectedfromAccount);
                    var accountto = db.Accounts.Find(transfer.SelectedtoAccount);
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
                            db.Transactions.Add(trans);
                            //call trasfer sp
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                   

                }
                IQueryable<Account> _acccounts = from s in db.Accounts
                                                 select s;
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

        public ActionResult Details(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                return BadRequest();
            }
            var trans = db.Transactions.Find(transactionId);
            if (trans == null)
            {
                return NotFound();
            }
            return View(trans);
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
