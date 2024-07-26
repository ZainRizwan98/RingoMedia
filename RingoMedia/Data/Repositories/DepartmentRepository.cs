using AutoMapper;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using RingoMedia.Data.Repositories.Interfaces;
using RingoMedia.Exceptions;
using RingoMedia.Models.Entities;
using RingoMedia.Models.ViewModels;

namespace RingoMedia.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly RingoMediaDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _uploadsFolderPath;

        public DepartmentRepository(RingoMediaDbContext context, IHostEnvironment environment, IMapper mapper)
        {
            _context = context;
            _uploadsFolderPath = Path.Combine(environment.ContentRootPath, "wwwroot", "images");
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentVM>> FindAsync()
        {
            var departments = await _context.Departments
                .Where(x => !x.ParentId.HasValue)
                .ToListAsync();

            foreach (var department in departments)
            {
                await LoadSubDepartments(department);
            }

            return _mapper.Map<List<DepartmentVM>>(departments);
        }

        private async Task LoadSubDepartments(DepartmentEntity department)
        {
            var subDepartments = await _context.Departments
                .Where(d => d.ParentId == department.Id)
                .ToListAsync();

            department.SubDepartments = subDepartments;

            foreach (var subDepartment in subDepartments)
            {
                await LoadSubDepartments(subDepartment);
            }
        }

        public async Task<DepartmentVM> GetByIdAsync(int id)
        {
            DepartmentEntity? department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            return _mapper.Map<DepartmentVM>(department);
        }

        public async Task AddAsync(DepartmentVM request, IFormFile? image)
        {
            string? logoPath = await UploadLogoAsync(image);

            await _context.AddAsync(new DepartmentEntity
            {
                DepartmentName = request.DepartmentName,
                DepartmentLogo = logoPath,
                ParentId = request.ParentId,
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DepartmentVM request, IFormFile? image)
        {
            DepartmentEntity? department = await _context.Departments.FindAsync(request.Id);

            if (department == null)
            {
                throw new NotFoundException($"Product with id {request.Id} not found.");
            }

            DeleteLogo(request.DepartmentLogo);
            string? logoPath = await UploadLogoAsync(image);

            department.DepartmentName = request.DepartmentName;
            department.DepartmentLogo = logoPath;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            DepartmentEntity? department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            DeleteLogo(department.DepartmentLogo);

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        private async Task<string?> UploadLogoAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0)
            {
                return null; // Handle this case as per your application's requirements
            }

            // Generate a unique file name to avoid conflicts
            var fileExtension = Path.GetExtension(image.FileName);
            var uniqueFileName = $"{DateTime.Now.Ticks}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadsFolderPath, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        private void DeleteLogo(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var filePath = Path.Combine(_uploadsFolderPath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
