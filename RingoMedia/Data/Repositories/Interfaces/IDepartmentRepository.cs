using RingoMedia.Models.ViewModels;

namespace RingoMedia.Data.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentVM>> FindAsync();
        Task<DepartmentVM> GetByIdAsync(int id);
        Task AddAsync(DepartmentVM request, IFormFile? image);
        Task UpdateAsync(DepartmentVM request, IFormFile? image);
        Task DeleteAsync(int id);
    }
}
