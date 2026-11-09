using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");

            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVM amenityVM = new AmenityVM()
            {
                Amenity = new Amenity(),
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };

            return View(amenityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Amenity amenity)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.Amenity.Add(amenity);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(amenity);
        }

        public IActionResult Update(int id)
        {
            var amenity = _unitOfWork.Amenity.Get(u => u.Id == id);
            if (amenity == null)
            {
                return NotFound();
            }
            AmenityVM amenityVM = new AmenityVM()
            {
                Amenity = amenity,
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };
            return View(amenityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Amenity amenity)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenity);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                AmenityVM amenityVM = new AmenityVM()
                {
                    Amenity = amenity,
                    VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    })
                };
                return View(amenityVM);
            }
        }

        public IActionResult Delete(int id)
        {
            var amenity = _unitOfWork.Amenity.Get(u => u.Id == id);
            if (amenity == null)
            {
                TempData["error"] = "The amenity does not exist.";
                return RedirectToAction("Index");
            }

            AmenityVM amenityVM = new AmenityVM()
            {
                Amenity = amenity,
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };

            return View(amenityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Amenity amenity)
        {
            try
            {
                _unitOfWork.Amenity.Remove(amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting amenity: {ex.Message}";

                AmenityVM amenityVM = new AmenityVM()
                {
                    Amenity = amenity,
                    VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    })
                };

                return View(amenityVM);
            }
        }
    }
}
