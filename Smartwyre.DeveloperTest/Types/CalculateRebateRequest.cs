namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateRequest
{
    public required string RebateIdentifier { get; set; }

    public required string ProductIdentifier { get; set; }

    public decimal Volume { get; set; }
}
