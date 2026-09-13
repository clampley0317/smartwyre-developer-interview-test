using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedCashAmountCalculator : IIncentiveCalculator
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume)
    {
        if (product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) && rebate.Amount != 0)
        {
            return CalculateRebateResult.Successful(rebate.Amount);
        }
        return CalculateRebateResult.Failure();
    }
}