using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public interface IIncentiveCalculator
{
    IncentiveType IncentiveType { get; }

    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume);
}