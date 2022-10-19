using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using BankPPP.Data;
using Microsoft.AspNetCore.Identity;

namespace BankPPP.Models
{
    
   
    public class User
    {
        [Key]
        [StringLength(50)]
        [Display(Name = "User ID")]
        public string userId { get; set; } = String.Empty;



        [StringLength(50)]
        [Display(Name = "Name")]
        public string name { get; set; } = String.Empty;

       

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime date_created { get; set; }

        public User()
        {
            this.userId = Guid.NewGuid().ToString("N").ToUpper();
            this.date_created = DateTime.Now;
        }

    }

    public class Account  
    {
        [Key]
        [StringLength(50)]
        [Display(Name = "Account Number")]
        public string accountnumber { get; set; } = String.Empty;


        
        [StringLength(50)]
        [Display(Name = "User ID")]
        public string userid { get; set; } = String.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Account Name")]
        public string accountname { get; set; } = String.Empty;


        [Display(Name = "Balance")]
        [Range(0, 1000000000)]
        public decimal balance { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime date_created { get; set; }

        public Account()
        {
            this.userid = "User_1";
            this.accountnumber = Guid.NewGuid().ToString("N").ToUpper(); 
            this.date_created = DateTime.Now;
        }

    }
    public class Transaction
    {
        [Key]
        [Required]
        [StringLength(50)]
        [Display(Name = "Transaction Id")]
        public string transactionId { get; set; } = String.Empty;

        [StringLength(50)]
        [Display(Name = "User ID")]
        public string userid { get; set; } = String.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "From Account")]
        public string from_account { get; set; } = String.Empty;


        [Required]
        [StringLength(20)]
        [Display(Name = "To Account")]
        public string to_account { get; set; } = String.Empty;



        [Range(1, 10000000)]
        [Display(Name = "Amount")]
        public decimal amount { get; set; }  

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime date_created { get; set; }

        public Transaction(string from_account, string to_account, decimal amount)
        {
            this.userid = "User_1";
            this.transactionId = Guid.NewGuid().ToString("N").ToUpper();
            this.from_account = from_account;
            this.to_account = to_account;
            this.amount = amount;
            this.date_created = DateTime.Now;
        }

        public Transaction(Account accountfrom, Account accountto, decimal amount)
        {
            this.userid = "User_1";
            this.transactionId = Guid.NewGuid().ToString("N").ToUpper();
            this.from_account = accountfrom.accountnumber;
            this.to_account = accountto.accountnumber;
            this.amount = amount;
            this.date_created = DateTime.Now;
        }
    }

    public class TransferView
    {
        [Required]
        public string SelectedfromAccount { get; set; } = String.Empty;
        [Required] 
        public string SelectedtoAccount { get; set; } = String.Empty;
       
        public IQueryable<Account>? fromAccount { get; set; }
   
        public IQueryable<Account>? toAccount { get; set; }
        [Required]
        [Range(0.01, 100000000, ErrorMessage = "Amount must be greter than zero !")] 
        // [Range(typeof(decimal), "0", "999999999")]
        public decimal amount { get; set; }
     
        public DateTime dateCreated { get; set; } 
    }

    public class  TransactionView
    {
        [Required]
        public string FromAccount { get; set; } = String.Empty;
        [Required]
        public string ToAccount { get; set; } = String.Empty;


        public DateTime TransactionTime { get; set; }


        public decimal AmountDebited { get; set; }

        public decimal FromAccountBalance { get; set; }

        public decimal ToAccountBalance { get; set; }
    }

    public class Helper
    {
        public static void InitializeData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<BankContext>();


            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                   new User
                   {
                       userId = "User_1",// for test only  Guid.NewGuid().ToString("N").ToUpper(),
                       name = "Nader Ssam",
                       date_created = DateTime.Now
                   }
                };
                context.AddRange(users);
                context.SaveChangesAsync();
            }
           
           
        }
  
    }
    

}