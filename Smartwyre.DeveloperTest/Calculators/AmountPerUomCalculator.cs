using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class AmountPerUomCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume)
    {
        var result = new CalculateRebateResult() { Success = false, RebateAmount = 0m };
        if (product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom)
            && rebate.Amount != 0
            && volume != 0)
        {
            result.RebateAmount = rebate.Amount * volume;
            result.Success = true;
        }
        return result;
    }
}