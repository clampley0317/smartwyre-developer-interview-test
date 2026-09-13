using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixeRateRebateCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume)
    {
        var result = new CalculateRebateResult() { Success = false, RebateAmount = 0m };
        if (product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) && rebate.Amount != 0)
        {
            result.RebateAmount = rebate.Amount;
            result.Success = true;
        }
        return result;
    }
}