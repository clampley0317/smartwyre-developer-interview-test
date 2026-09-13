namespace Smartwyre.DeveloperTest.Types;

public class RebateCalculation
{
    public int Id { get; set; }
    public required string Identifier { get; set; }
    public required string RebateIdentifier { get; set; }
    public IncentiveType IncentiveType { get; set; }
    public decimal Amount { get; set; }
}
