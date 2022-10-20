using BankApp.Data;
using BankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace bankApp.Services
{
    public interface ITransactionsService
    {
        Task<Transaction> createTransaction(Transaction account);
        Task<Transaction> getTransaction(string transactionsId);
        Task<IEnumerable<TransactionView>> getUserTransactions(string userId);
      
    }
    public class TransactionsService : ITransactionsService
    {
        BankContext db;
        
        public TransactionsService(BankContext _db )
        {
            db = _db;
           
        }
        public async Task<Transaction> createTransaction(Transaction account)
        { 
            try
            {
                await db.Transactions.AddAsync(account);
                db.SaveChanges(); 
                return account;
            }
            catch (Exception ex)
            {
                return account;
            }
        }
        
        public async Task<Transaction> getTransaction(string transactionId = "")
        {
            try
            {
                return await db.Transactions.Where(s => s.transactionId == transactionId).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<TransactionView>> getUserTransactions(string _userId)
        {
           

            var result = from p1 in db.Transactions
                         join p0 in db.Users on p1.userid equals p0.userId
                         join p2 in db.Accounts on p1.from_account equals p2.accountnumber
                         join p3 in db.Accounts on p1.to_account equals p3.accountnumber
                         select new TransactionView
                         {
                             FromAccount = p2.accountname,
                             ToAccount = p3.accountname,
                             TransactionTime = p1.date_created,
                             AmountDebited = p1.amount,
                             FromAccountBalance = p2.balance,
                             ToAccountBalance = p3.balance,
                         };

            return await result.ToListAsync();
        }
       
        public async Task<int> updateTransaction(Transaction transaction)
        {
            try
            {
                db.Transactions.Update(transaction);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<int> deleteTransaction(string transactionId)
        { 
            try
            {
                var transaction = await db.Transactions.FindAsync(transactionId);
                if (transaction != null)
                {
                    db.Transactions.Remove(transaction);
                    return await db.SaveChangesAsync();

                }
                else
                {
                     throw new ApplicationException("Account not found");
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
        } 

    }
}
