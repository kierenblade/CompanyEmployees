using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Employee
{
    [Column("CompanyId")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Employee name is required")]
    [MaxLength(30, ErrorMessage = "Maximum length for employee name is 30")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Employee age is required")]
    public int Age { get; set; }
    
    [Required(ErrorMessage = "Employee position is required")]
    [MaxLength(20, ErrorMessage = "Maximum length for employee name is 20")]
    public string? Position { get; set; }
    
    [ForeignKey(nameof(Company))]
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}