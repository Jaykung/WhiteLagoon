using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumber villaNumber)
        {
            bool isVillaNumberExists = _unitOfWork.VillaNumber.GetAll().Any(v => v.Villa_Number == villaNumber.Villa_Number);

            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };

            if (isVillaNumberExists)
            {
                TempData["error"] = "A villa number with this number already exists.";
                return View(villaNumberVM);
            }

            if (ModelState.IsValid)
            {

                _unitOfWork.VillaNumber.Add(villaNumber);
                _unitOfWork.Save();

                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "There was an error creating the villa number.";
                return View(villaNumberVM);
            }
        }

        public IActionResult Update(int id)
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(v => v.Villa_Number == id)
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumber villaNumber)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumber);
                _unitOfWork.Save();
                TempData["success"] = $"The villa number {villaNumber.Villa_Number} has been updated.";
                return RedirectToAction("Index");
            }
            else
            {
                VillaNumberVM villaNumberVM = new VillaNumberVM()
                {
                    VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    }),
                    VillaNumber = villaNumber
                };

                TempData["error"] = "There was an error updating the villa number.";
                return View(villaNumberVM);
            }
        }

        public IActionResult Delete(int id)
        {
            var villaNumber = _unitOfWork.VillaNumber.Get(v => v.Villa_Number == id);
            if (villaNumber == null)
            {
                TempData["error"] = "The villa number does not exist.";
                return RedirectToAction("Index");
            }

            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = villaNumber
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumber villaNumber)
        {
            try
            {
                _unitOfWork.VillaNumber.Remove(villaNumber);
                _unitOfWork.Save();
                TempData["success"] = $"The villa number {villaNumber.Villa_Number} has been deleted.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"There was an error deleting the villa number: {ex.Message}";

                VillaNumberVM villaNumberVM = new VillaNumberVM()
                {
                    VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    }),
                    VillaNumber = villaNumber
                };

                return View(villaNumberVM);
            }
        }
    }
}
