using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll().ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (ModelState.IsValid)
            {
                villa.CreatedDate = DateTime.Now;
                _unitOfWork.Villa.Add(villa);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "There was an error creating the villa.";
                return View(villa);
            }
        }

        public IActionResult Update(int id)
        {
            Villa? villa = _unitOfWork.Villa.Get(v => v.Id == id);

            if (villa == null)
            {
                return RedirectToAction("E_NotFound", "Error");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Villa.Update(villa);
                _unitOfWork.Save();
                TempData["success"] = $"The villa {villa.Name} has been updated.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "There was an error updating the villa.";
                return View(villa);
            }
        }

        public IActionResult Delete(int id)
        {
            Villa? villa = _unitOfWork.Villa.Get(v => v.Id == id);

            if (villa is null)
            {
                return RedirectToAction("E_NotFound", "Error");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? villaFromDb = _unitOfWork.Villa.Get(v => v.Id == villa.Id);

            if (villaFromDb is not null)
            {
                _unitOfWork.Villa.Remove(villaFromDb);
                _unitOfWork.Save();
                TempData["success"] = $"The villa {villaFromDb.Name} has been deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = $"The villa could not be deleted.";
                return View(villa);
            }
        }
    }
}
