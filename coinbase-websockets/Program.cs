using backtesting;

class Program
{
    static async Task Main()
    {
        var firstCoinName = "bitcoin";
        var secondCoinName = "solana";
        var initialGBPFunds = 1000;
        var initialSecondCoinFunds = 1000;

        var firstCoinRequestUrl = $"https://api.coingecko.com/api/v3/coins/{firstCoinName}/market_chart?vs_currency=gbp&days=365&interval=daily";
        var secondCoinRequestUrl = $"https://api.coingecko.com/api/v3/coins/{secondCoinName}/market_chart?vs_currency=gbp&days=365&interval=daily";

        PricingData pricingData = new PricingData(firstCoinName, secondCoinName, firstCoinRequestUrl, secondCoinRequestUrl);
        var pairPricingDictionary = await pricingData.GetPriceComparisonDictionary();

        HistoricalPerformance historicalPerformance = new HistoricalPerformance(initialSecondCoinFunds, initialGBPFunds, firstCoinName, secondCoinName);
        historicalPerformance.GetStrategyPerformance(pairPricingDictionary);
    }
}
