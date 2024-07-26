using RingoMedia.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RingoMedia.Models.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Department Logo")]
        public string? DepartmentLogo { get; set; }

        public int? ParentId { get; set; }
        public DepartmentVM? Parent { get; set; }

        public IEnumerable<DepartmentVM> SubDepartments { get; set; } = Enumerable.Empty<DepartmentVM>();
    }
}
