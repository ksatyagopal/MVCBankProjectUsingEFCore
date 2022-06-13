using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBankProjectUsingEFCore.Models;

namespace MVCBankProjectUsingEFCore.Controllers
{
    public class MoneyTransactionsController : Controller
    {
        private readonly SharpBankContext _context;

        public MoneyTransactionsController(SharpBankContext context)
        {
            _context = context;
        }

        // GET: MoneyTransactions
        
        public async Task<IActionResult> Index()
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username != null)
            {
                var sharpBankContext = _context.MoneyTransactions.Include(m => m.TaccountNumberNavigation);
                return View(await sharpBankContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            
        }

        // GET: MoneyTransactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var moneyTransaction = await _context.MoneyTransactions
                .Include(m => m.TaccountNumberNavigation)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (moneyTransaction == null)
            {
                return NotFound();
            }

            return View(moneyTransaction);
        }

        // GET: MoneyTransactions/Create
        public IActionResult Create()
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ViewData["TaccountNumber"] = new SelectList(_context.BankAccounts, "AccountNumber", "AccountNumber");
            return View();
        }

        // POST: MoneyTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,TaccountNumber,TransactionDate,TransactionAmount,CurrentBalance")] MoneyTransaction moneyTransaction, string state)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                moneyTransaction.TransactionDate = DateTime.Now;
                if (state == "w")
                {
                    moneyTransaction.TransactionAmount = -moneyTransaction.TransactionAmount;
                }
                foreach(var acc in _context.BankAccounts)
                {
                    if(acc.AccountNumber == moneyTransaction.TaccountNumber)
                    {
                        moneyTransaction.CurrentBalance = double.Parse(acc.Balance.ToString()) + moneyTransaction.TransactionAmount;
                        acc.Balance = moneyTransaction.CurrentBalance;
                        break;
                    }
                }
                
                _context.Add(moneyTransaction);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaccountNumber"] = new SelectList(_context.BankAccounts, "AccountNumber", "AccountNumber", moneyTransaction.TaccountNumber);
            return View(moneyTransaction);
        }

        // GET: MoneyTransactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var moneyTransaction = await _context.MoneyTransactions.FindAsync(id);
            if (moneyTransaction == null)
            {
                return NotFound();
            }
            ViewData["TaccountNumber"] = new SelectList(_context.BankAccounts, "AccountNumber", "AccountHolderName", moneyTransaction.TaccountNumber);
            return View(moneyTransaction);
        }

        // POST: MoneyTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,TaccountNumber,TransactionDate,TransactionAmount,CurrentBalance")] MoneyTransaction moneyTransaction)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (id != moneyTransaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moneyTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoneyTransactionExists(moneyTransaction.TransactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaccountNumber"] = new SelectList(_context.BankAccounts, "AccountNumber", "AccountHolderName", moneyTransaction.TaccountNumber);
            return View(moneyTransaction);
        }

        // GET: MoneyTransactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var moneyTransaction = await _context.MoneyTransactions
                .Include(m => m.TaccountNumberNavigation)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (moneyTransaction == null)
            {
                return NotFound();
            }

            return View(moneyTransaction);
        }

        // POST: MoneyTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var moneyTransaction = await _context.MoneyTransactions.FindAsync(id);
            _context.MoneyTransactions.Remove(moneyTransaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoneyTransactionExists(int id)
        {
            return _context.MoneyTransactions.Any(e => e.TransactionId == id);
        }
    }
}
