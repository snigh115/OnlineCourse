using StudentRegisteration.Models;

namespace StudentRegisteration.ViewModels;

public class AddressViewModel
{
    public IEnumerable<Address> Addresses  { get; set; } 
    public IEnumerable<string> States { get; set; }
    public IEnumerable<string> Cities  { get; set; }
    public IEnumerable<string> Towns  { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Town { get; set; }

    public int Page { get; set; }
    public int TotalCount { get; set; }

}