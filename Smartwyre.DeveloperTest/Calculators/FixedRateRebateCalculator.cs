using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedRateRebateCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume)
    {
        if (product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate)
            && rebate.Percentage != 0
            && product.Price != 0
            && volume != 0)
        {
            return CalculateRebateResult.Successful(product.Price * rebate.Percentage * volume);
        }
        return CalculateRebateResult.Failure();
    }
}