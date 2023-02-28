using backtesting;

class Program
{
    static async Task Main()
    {
        float changeInFunds;
        float trailingCoinPriceChange;
        var indicatorCoinName = "bitcoin";
        var trailingCoinName = "matic-network";
        var initialGBPFunds = 1000;
        var initialSecondCoinFunds = 1000;
        var numberOfDaysToMeasurePerformance = 180;

        var firstCoinRequestUrl = $"https://api.coingecko.com/api/v3/coins/{indicatorCoinName}/market_chart?vs_currency=gbp&days={numberOfDaysToMeasurePerformance}&interval=daily";
        var secondCoinRequestUrl = $"https://api.coingecko.com/api/v3/coins/{trailingCoinName}/market_chart?vs_currency=gbp&days={numberOfDaysToMeasurePerformance}&interval=daily";

        PricingData pricingData = new PricingData(indicatorCoinName, trailingCoinName, firstCoinRequestUrl, secondCoinRequestUrl);
        var pairPricingDictionary = await pricingData.GetPriceComparisonDictionary();

        HistoricalPerformance historicalPerformance = new HistoricalPerformance(initialSecondCoinFunds, initialGBPFunds, indicatorCoinName, trailingCoinName);
        historicalPerformance.GetStrategyPerformance(pairPricingDictionary, out changeInFunds, out trailingCoinPriceChange);

        showPerformance(ref changeInFunds, ref trailingCoinPriceChange);
    }
    static private void showPerformance(ref float changeInFunds, ref float trailingCoinPriceChange)
    {
        var strategyPerformance = changeInFunds - 1;
        var trailingCoinPerformanceBenchmark = trailingCoinPriceChange - 1;
        Console.WriteLine($"Strategy Performance: {strategyPerformance.ToString("P")}");
        Console.WriteLine($"Trailing coin price change: {trailingCoinPerformanceBenchmark.ToString("P")}");
    }
}
