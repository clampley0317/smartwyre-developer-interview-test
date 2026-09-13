namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{
    public bool Success { get; set; }
    public decimal RebateAmount { get; set; }

    public static CalculateRebateResult Failure() =>
        new() { Success = false, RebateAmount = 0m };

    public static CalculateRebateResult Successful(decimal rebateAmount) =>
        new() { Success = true, RebateAmount = rebateAmount };
}
