using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedCashAmountCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume)
    {
        var result = new CalculateRebateResult() { Success = false, RebateAmount = 0m };
        if (product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate)
            && rebate.Percentage != 0
            && product.Price != 0
            && volume != 0)
        {
            result.RebateAmount = product.Price * rebate.Percentage * volume;
            result.Success = true;
        }
        return result;
    }
}