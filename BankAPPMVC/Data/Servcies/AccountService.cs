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
    public interface IAccountService
    {
        Task<Account> createAccount(Account account);
        Task<Account> getAccount(string accountId);
        Task<IEnumerable<Account>> getUserAccounts(string userId);
        Task<int> updateAccount(Account account ); 
        Task<int> deleteAccount(string accountId);
    }
    public class AccountService : IAccountService
    {
        BankContext db;
        
        public AccountService(BankContext _db )
        {
            db = _db;
           
        }
        public async Task<Account> createAccount(Account account)
        { 
            try
            {
                await db.Accounts.AddAsync(account);
                db.SaveChanges(); 
                return account;
            }
            catch (Exception ex)
            {
                return account;
            }
        }
        
        public async Task<Account> getAccount(string accountId="")
        {
            try
            {
                return await db.Accounts.Where(s => s.accountnumber == accountId).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<Account>> getUserAccounts(string _userId)
        {
            return await db.Accounts.Where(s=>s.userid == _userId).ToListAsync();
        }
       
        public async Task<int> updateAccount(Account account)
        {
            try
            {
                db.Accounts.Update(account);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<int> deleteAccount(string accountId)
        { 
            try
            {
                var account = await db.Accounts.FindAsync(accountId);
                if (account != null)
                {
                    db.Accounts.Remove(account);
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
