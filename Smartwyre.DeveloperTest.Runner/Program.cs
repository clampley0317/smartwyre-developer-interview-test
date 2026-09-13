using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static int Main()
    {
        var service = new RebateService(
            new DemoRebateDataStore(),
            new DemoProductDataStore(),
            [
                new FixedCashAmountCalculator(),
                new FixedRateRebateCalculator(),
                new AmountPerUomCalculator()
            ]);

        Console.WriteLine("Available rebates: cash-25, rate-10, uom-4");
        Console.WriteLine("Available product: product-100");

        Console.Write("Rebate identifier: ");
        var rebateIdentifier = Console.ReadLine() ?? string.Empty;

        Console.Write("Product identifier: ");
        var productIdentifier = Console.ReadLine() ?? string.Empty;

        Console.Write("Volume: ");
        if (!decimal.TryParse(Console.ReadLine(), out var volume))
        {
            Console.WriteLine("Volume must be a number.");
            return 1;
        }

        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        });

        if (!result.Success)
        {
            Console.WriteLine("The rebate could not be calculated.");
            return 1;
        }

        Console.WriteLine($"Rebate calculated successfully: {result.RebateAmount:C}");
        return 0;
    }
}

sealed class DemoRebateDataStore : IRebateDataStore
{
    private readonly Dictionary<string, Rebate> _rebates = new()
    {
        ["cash-25"] = new Rebate
        {
            Identifier = "cash-25",
            Incentive = IncentiveType.FixedCashAmount,
            Amount = 25m
        },
        ["rate-10"] = new Rebate
        {
            Identifier = "rate-10",
            Incentive = IncentiveType.FixedRateRebate,
            Percentage = 0.10m
        },
        ["uom-4"] = new Rebate
        {
            Identifier = "uom-4",
            Incentive = IncentiveType.AmountPerUom,
            Amount = 4m
        }
    };

    public Rebate? GetRebate(string rebateIdentifier) =>
        _rebates.GetValueOrDefault(rebateIdentifier);

    public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
    {
        Console.WriteLine(
            $"Stored calculation for '{rebate.Identifier}': {rebateAmount:C}");
    }
}

sealed class DemoProductDataStore : IProductDataStore
{
    private readonly Product _product = new()
    {
        Identifier = "product-100",
        Uom = "Each",
        Price = 100m,
        SupportedIncentives =
            SupportedIncentiveType.FixedCashAmount |
            SupportedIncentiveType.FixedRateRebate |
            SupportedIncentiveType.AmountPerUom
    };

    public Product? GetProduct(string productIdentifier) =>
        productIdentifier == _product.Identifier ? _product : null;
}