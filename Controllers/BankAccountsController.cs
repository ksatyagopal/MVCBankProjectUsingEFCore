using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBankProjectUsingEFCore.Models;

namespace MVCBankProjectUsingEFCore.Controllers
{
    public class BankAccountsController : Controller
    {
        private readonly SharpBankContext _context;

        public BankAccountsController(SharpBankContext context)
        {
            _context = context;
        }

        // GET: BankAccounts
        public async Task<IActionResult> Index()
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username != null)
            {
                return View(await _context.BankAccounts.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: BankAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.transactions = _context.MoneyTransactions;
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(m => m.AccountNumber == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public IActionResult Create()
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountNumber,AccountHolderName,TypeOfAccount,DateOfCreation,Balance")] BankAccount bankAccount)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Add(bankAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
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

            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountNumber,AccountHolderName,TypeOfAccount,DateOfCreation,Balance")] BankAccount bankAccount)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (id != bankAccount.AccountNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountExists(bankAccount.AccountNumber))
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
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
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

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(m => m.AccountNumber == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.username = HttpContext.Session.GetString("fName");
            if (ViewBag.username == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var bankAccount = await _context.BankAccounts.FindAsync(id);
            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.AccountNumber == id);
        }
    }
}
