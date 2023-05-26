using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Company
{
    [Column("CompanyId")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Company name is required")]
    [MaxLength(60, ErrorMessage = "Maximum length for company name is 60")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Company address is required")]
    [MaxLength(60, ErrorMessage = "Maximum length for company address is 60")]
    public string? Address { get; set; }
    
    public string? Country { get; set; }
    
    public ICollection<Employee> Employees { get; set; }
}