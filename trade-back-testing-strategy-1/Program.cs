using backtesting;

class Program
{
    static async Task Main()
    {
        float changeInFunds;
        float trailingCoinPriceChange;
        string indicatorCoinName = "bitcoin";
        string trailingCoinName = "matic-network";
        int initialGBPFunds = 1000;
        int initialSecondCoinFunds = 1000;
        int numberOfDaysToMeasurePerformance = 365;

        string firstCoinRequestUrl = $"https://api.coingecko.com/api/v3/coins/{indicatorCoinName}/market_chart?vs_currency=gbp&days={numberOfDaysToMeasurePerformance}&interval=daily";
        string secondCoinRequestUrl = $"https://api.coingecko.com/api/v3/coins/{trailingCoinName}/market_chart?vs_currency=gbp&days={numberOfDaysToMeasurePerformance}&interval=daily";

        PricingData pricingData = new PricingData(indicatorCoinName, trailingCoinName, firstCoinRequestUrl, secondCoinRequestUrl);
        Dictionary<string, float[]> pairPricingDictionary = await pricingData.GetPriceComparisonDictionary();

        HistoricalPerformance historicalPerformance = new HistoricalPerformance(initialSecondCoinFunds, initialGBPFunds, indicatorCoinName, trailingCoinName);
        historicalPerformance.GetStrategyPerformance(pairPricingDictionary, out changeInFunds, out trailingCoinPriceChange);

        showPerformance(ref changeInFunds, ref trailingCoinPriceChange);
    }
    static private void showPerformance(ref float changeInFunds, ref float trailingCoinPriceChange)
    {
        float strategyPerformance = changeInFunds - 1;
        float trailingCoinPerformanceBenchmark = trailingCoinPriceChange - 1;

        Console.WriteLine($"Strategy Performance: {strategyPerformance.ToString("P")}");
        Console.WriteLine($"Trailing coin price change: {trailingCoinPerformanceBenchmark.ToString("P")}");
    }
}
