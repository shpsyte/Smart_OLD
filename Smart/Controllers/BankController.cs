using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Services.Interfaces;
using Smart.Services;
using Core.Domain.Finance;
using Smart.Data;
namespace Smart.Controllers
{
    [Authorize]    
    public class BankController : BaseController
    {
        #region vars
        private readonly  IServices<Bank> _bankServices;
        #endregion
        #region ctor
            public BankController(
                                    IServices<Bank> bankServices, 
                                    IUser currentUser, 
                                    IEmailSender emailSender, 
                                    IHttpContextAccessor accessor
                                    ) : base(currentUser, emailSender, accessor)
            {
            this._bankServices = bankServices;
            }
        #endregion
        #region methods
        // GET: Bank
        [Route("bank-management/bank-list")]
        public async Task<IActionResult> List(string search)
        {
            ViewData["search"] = search;
            var data = await _bankServices.QueryAsync();
            if (!string.IsNullOrEmpty(search)) 
            {
               data = data.Where(p =>  
                           p.Name.Contains(search)
                );
            }
               return View(data);
        }
       // GET: Bank/Add
        [Route("bank-management/bank-add")]
        public IActionResult Add()
        {
          var data = new Bank();
          return View(data);
        }
        // POST: Bank/Add
        [HttpPost, ValidateAntiForgeryToken]
        [Route("bank-management/bank-add")]
        public async Task<IActionResult> Add([Bind("BankId,Name,Active,BusinessEntityId")] Bank bank, bool continueAdd)
        {
            if (ModelState.IsValid)
            {
                 await _bankServices.AddAsync(bank);
                 return continueAdd ? RedirectToAction(nameof(Add)) : RedirectToAction(nameof(List));
            }
            return View(bank);
        }
        // GET: Bank/Edit/5
        [Route("bank-management/bank-edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            var bank = await _bankServices.SingleOrDefaultAsync(m => m.BankId == id);
            if (bank == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            return View(bank);
        }
        // POST: Bank/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        [Route("bank-management/bank-edit/{id?}")]
        public async Task<IActionResult> Edit(int id, [Bind("BankId,Name,Active,BusinessEntityId")] Bank bank, bool continueAdd, bool addTrash)
        {
            if (id != bank.BankId)
            {
                return NotFound();
            }
                 if (ModelState.IsValid)
            {
                try
                {
                    await _bankServices.UpdateAsync(bank);
                }
                catch (DbUpdateConcurrencyException)
                {
                        throw;
                }
                return continueAdd ? RedirectToAction(nameof(Edit), new { id = bank.BankId }) : RedirectToAction(nameof(List));
            }
            return View(bank);
        }
        [Route("bank-management/bank-delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
               var bank = await  _bankServices.SingleOrDefaultAsync(m => m.BankId == id);
               if (bank == null)
               {
                    return NotFound();
                }
                    return View(bank);
         }  
         [HttpPost, ValidateAntiForgeryToken]
         [Route("bank-management/bank-delete/{id?}")]
         public async Task<IActionResult> Delete (Bank bank)
         {
             await _bankServices.DeleteAsync(bank);
             return RedirectToAction(nameof(List));
         }
        #endregion methods
    }
}