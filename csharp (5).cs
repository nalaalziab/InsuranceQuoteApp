public class Insuree
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int CarYear { get; set; }
    public string CarMake { get; set; }
    public string CarModel { get; set; }
    public bool DUI { get; set; }
    public int SpeedingTickets { get; set; }
    public bool CoverageType { get; set; } // true = Full Coverage, false = Liability
    public decimal Quote { get; set; }
}