using System.ComponentModel.DataAnnotations;

namespace StudentRegisteration.Models;


public class Address
{
    public int Id { get; set; }

    [Required(ErrorMessage = "State is required")]
    public string State { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }

    [Required(ErrorMessage = "Town is required")]
    public string Town { get; set; }

    public ICollection<StudentDetails>? StudentDetails { get; set; } 

}