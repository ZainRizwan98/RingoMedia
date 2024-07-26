using System.ComponentModel.DataAnnotations;

namespace RingoMedia.Models.Entities
{
    public class DepartmentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? DepartmentLogo { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        public DepartmentEntity? Parent { get; set; }

        //public int DepartmentId { get; set; }
        //public DepartmentEntity? Department { get; set; }

        public ICollection<DepartmentEntity> SubDepartments { get; set; } = new List<DepartmentEntity>();
    }
}
