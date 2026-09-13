using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServicesTEst
{
    [Fact]
    public void Calculate_WhenFixedCashAmountIsValid_StoresTheFixedAmount()
    {
        var rebateStore = new FakeRebateDataStore
        {
            Rebate = new Rebate
            {
                Identifier = "rebate-1",
                Incentive = IncentiveType.FixedCashAmount,
                Amount = 25m
            }
        };
        var productStore = new FakeProductDataStore
        {
            Product = new Product
            {
                Identifier = "product-1",
                Uom = "Each",
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            }
        };
        var service = CreateService(rebateStore, productStore);

        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1",
            Volume = 2m
        });

        Assert.True(result.Success);
        Assert.Equal(25m, result.RebateAmount);
        Assert.Equal(25m, rebateStore.StoredAmount);
    }

    [Fact]
    public void Calculate_WhenFixedRateRebateIsValid_StoresPricePercentageAndVolume()
    {
        var rebateStore = new FakeRebateDataStore
        {
            Rebate = new Rebate
            {
                Identifier = "rebate-1",
                Incentive = IncentiveType.FixedRateRebate,
                Percentage = 0.1m
            }
        };
        var productStore = new FakeProductDataStore
        {
            Product = new Product
            {
                Identifier = "product-1",
                Uom = "Each",
                Price = 100m,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            }
        };
        var service = CreateService(rebateStore, productStore);

        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1",
            Volume = 3m
        });

        Assert.True(result.Success);
        Assert.Equal(30m, result.RebateAmount);
        Assert.Equal(30m, rebateStore.StoredAmount);
    }

    [Fact]
    public void Calculate_WhenAmountPerUomIsValid_StoresAmountMultipliedByVolume()
    {
        var rebateStore = new FakeRebateDataStore
        {
            Rebate = new Rebate
            {
                Identifier = "rebate-1",
                Incentive = IncentiveType.AmountPerUom,
                Amount = 4m
            }
        };
        var productStore = new FakeProductDataStore
        {
            Product = new Product
            {
                Identifier = "product-1",
                Uom = "Each",
                SupportedIncentives = SupportedIncentiveType.AmountPerUom
            }
        };
        var service = CreateService(rebateStore, productStore);

        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1",
            Volume = 5m
        });

        Assert.True(result.Success);
        Assert.Equal(20m, result.RebateAmount);
        Assert.Equal(20m, rebateStore.StoredAmount);
    }

    [Fact]
    public void Calculate_WhenRebateDoesNotExist_ReturnsFailureAndDoesNotStore()
    {
        var rebateStore = new FakeRebateDataStore();
        var productStore = new FakeProductDataStore
        {
            Product = new Product { Identifier = "product-1", Uom = "Each" }
        };
        var service = CreateService(rebateStore, productStore);

        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1"
        });

        Assert.False(result.Success);
        Assert.Null(rebateStore.StoredAmount);
    }

    [Fact]
    public void Calculate_WhenProductDoesNotSupportTheIncentive_ReturnsFailureAndDoesNotStore()
    {
        var rebateStore = new FakeRebateDataStore
        {
            Rebate = new Rebate
            {
                Identifier = "rebate-1",
                Incentive = IncentiveType.AmountPerUom,
                Amount = 5m
            }
        };
        var productStore = new FakeProductDataStore
        {
            Product = new Product
            {
                Identifier = "product-1",
                Uom = "Each",
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            }
        };
        var service = CreateService(rebateStore, productStore);

        var result = service.Calculate(new CalculateRebateRequest
        {
            RebateIdentifier = "rebate-1",
            ProductIdentifier = "product-1",
            Volume = 2m
        });

        Assert.False(result.Success);
        Assert.Null(rebateStore.StoredAmount);
    }

    private static RebateService CreateService(
        FakeRebateDataStore rebateStore,
        FakeProductDataStore productStore) =>
        new(
            rebateStore,
            productStore,
            new IIncentiveCalculator[]
            {
                new FixedCashAmountCalculator(),
                new FixeRateRebateCalculator(),
                new AmountPerUomCalculator()
            });

    private sealed class FakeRebateDataStore : IRebateDataStore
    {
        public Rebate? Rebate { get; init; }
        public decimal? StoredAmount { get; private set; }

        public Rebate? GetRebate(string rebateIdentifier) => Rebate;

        public void StoreCalculationResult(Rebate rebate, decimal rebateAmount) =>
            StoredAmount = rebateAmount;
    }

    private sealed class FakeProductDataStore : IProductDataStore
    {
        public Product? Product { get; init; }

        public Product? GetProduct(string productIdentifier) => Product;
    }
}
