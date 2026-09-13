using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class AmountPerUomCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume)
    {
        if (product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom)
            && rebate.Amount != 0
            && volume != 0)
        {
            return CalculateRebateResult.Successful(rebate.Amount * volume);
        }
        return CalculateRebateResult.Failure();
    }
}