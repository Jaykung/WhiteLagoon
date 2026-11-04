using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                // try to save the uploaded image file
                try
                {
                    if (villa.Image != null && villa.Image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                        villa.ImageUrl = Path.Combine("/images/Villa", fileName);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('/'));
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            villa.Image.CopyTo(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"There was an error uploading the image: {ex.Message}";
                    return View(villa);
                }

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

            if (villa.ImageUrl == null)
            {
                villa.ImageUrl = "/images/placeholder.png";
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid)
            {
                // check if a new image file has been uploaded and delete the old image file if it exists
                if (villa.Image != null && !string.IsNullOrEmpty(villa.ImageUrl))
                {
                    try
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = $"There was an error deleting the image: {ex.Message}";
                        return View(villa);
                    }
                }

                // try to save the uploaded image file
                try
                {
                    if (villa.Image != null && villa.Image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                        villa.ImageUrl = Path.Combine("/images/Villa", fileName);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('/'));
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            villa.Image.CopyTo(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"There was an error uploading the image: {ex.Message}";
                    return View(villa);
                }

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

                // try to delete the image file associated with the villa
                try
                {
                    if (!string.IsNullOrEmpty(villaFromDb.ImageUrl))
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, villaFromDb.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"There was an error deleting the image: {ex.Message}";
                    return View(villa);
                }

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
