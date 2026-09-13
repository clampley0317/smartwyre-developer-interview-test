namespace Smartwyre.DeveloperTest.Types;

public class Product
{
    public int Id { get; set; }
    public required string Identifier { get; set; }
    public decimal Price { get; set; }
    public required string Uom { get; set; }
    public SupportedIncentiveType SupportedIncentives { get; set; }
}
