namespace backtesting
{
    public class HistoricalPerformance
    {
        public void getStrategyPerformance(string firstCoinName, string secondCoinName, Dictionary<string, float[]> pairPricingDictionary)
        {

            float gbpFunds = 1000;
            float polygonFunds = 1000;
            float initialFundsTotalInGBP = gbpFunds + polygonFunds * pairPricingDictionary[$"{secondCoinName} prices"].FirstOrDefault();

            for (int i = 1; i < pairPricingDictionary[$"{firstCoinName} prices"].Length; i++)
            {
                float currentDayBitcoinPrice = pairPricingDictionary[$"{firstCoinName} prices"][i];
                float yesterdayDayBitcoinPrice = pairPricingDictionary[$"{firstCoinName} prices"][i - 1];
                float bitcoinPriceIncrease = currentDayBitcoinPrice / yesterdayDayBitcoinPrice;
                if (bitcoinPriceIncrease > 1.02)
                {
                    gbpFunds -= 1;
                    float currentDayPolygonPrice = pairPricingDictionary[$"{secondCoinName} prices"][i];
                    float polygonQuanityPurchased = 1 / currentDayPolygonPrice;
                    polygonFunds += polygonQuanityPurchased;;
                }
                if (bitcoinPriceIncrease < 0.98)
                {
                    gbpFunds += 1;
                    float currentDayPolygonPrice = pairPricingDictionary[$"{secondCoinName} prices"][i];
                    float polygonQuanitySold = 1 / currentDayPolygonPrice;
                    polygonFunds -= polygonQuanitySold;
                }
            }
            float finalFundsTotalInGBP = gbpFunds + polygonFunds * pairPricingDictionary[$"{secondCoinName} prices"].FirstOrDefault();
            float changeInFunds = finalFundsTotalInGBP / initialFundsTotalInGBP;
            float polygonPriceChange = pairPricingDictionary[$"{secondCoinName} prices"].LastOrDefault() / pairPricingDictionary[$"{secondCoinName} prices"].FirstOrDefault();
        }
    }
}
