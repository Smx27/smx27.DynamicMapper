namespace Sample.ConsoleApp.Models;

public class DestinationModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CreationDate { get; set; } = string.Empty;
    public double TotalAmount { get; set; }
}
