using System.Collections.Generic;
using System.Linq;

using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Calculators;


namespace Smartwyre.DeveloperTest.Services;

public class RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore, IEnumerable<IIncentiveCalculator> calculators) : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore = rebateDataStore;
    private readonly IProductDataStore _productDataStore = productDataStore;
    private readonly IReadOnlyDictionary<IncentiveType, IIncentiveCalculator> _calculators = calculators.ToDictionary(x => x.IncentiveType);

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate is null || product is null || !_calculators.TryGetValue(rebate.Incentive, out var calculator))
        {
            return CalculateRebateResult.Failure();
        }

        var result = calculator.Calculate(rebate, product, request.Volume);

        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, result.RebateAmount);
        }

        return result;
    }
}
