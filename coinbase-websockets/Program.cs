using backtesting;

class Program
{
    static async Task Main()
    {
        PricingData pricingData = new PricingData();

        var firstCoinRequestUrl = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=gbp&days=365&interval=daily";
        var secondCoinRequestUrl = "https://api.coingecko.com/api/v3/coins/matic-network/market_chart?vs_currency=gbp&days=365&interval=daily";

        var pairPricingDictionary = await pricingData.GetPriceComparisonDictionary("bitcoin", "matic-network", firstCoinRequestUrl, secondCoinRequestUrl);

        HistoricalPerformance historicalPerformance = new HistoricalPerformance();
        historicalPerformance.getStrategyPerformance("bitcoin", "matic-network", pairPricingDictionary);
    }
}
