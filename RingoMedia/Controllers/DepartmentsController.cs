using Microsoft.AspNetCore.Mvc;
using RingoMedia.Data.Repositories.Interfaces;
using RingoMedia.Models.ViewModels;

namespace RingoMedia.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentsController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.FindAsync());
        }

        public async Task<IActionResult> DetailsAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            return View(department);
        }

        public IActionResult Create(int? parentId)
        {
            return View(new DepartmentVM
            {
                ParentId = parentId,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(DepartmentVM department, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(department, image);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, DepartmentVM department, IFormFile? image)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(department, image);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
