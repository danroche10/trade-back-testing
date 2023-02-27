namespace backtesting
{
    public class HistoricalPerformance
    {
        private float _gbpFunds;
        private float _polygonFunds;
        private readonly string _firstCoinName;
        private readonly string _secondCoinName;
        public HistoricalPerformance(float gbpFunds, float polygonFunds, string firstCoinName, string secondCoinName)
        {
            _firstCoinName = firstCoinName; 
            _secondCoinName = secondCoinName;
            _gbpFunds = gbpFunds;
            _polygonFunds = polygonFunds;
        }
        public void GetStrategyPerformance(Dictionary<string, float[]> pairPricingDictionary)
        {
            float initialFundsTotalInGBP = _gbpFunds + _polygonFunds * pairPricingDictionary[$"{_secondCoinName} prices"].FirstOrDefault();

            placeTheoreticalOrders(pairPricingDictionary);

            float finalFundsTotalInGBP = _gbpFunds + _polygonFunds * pairPricingDictionary[$"{_secondCoinName} prices"].FirstOrDefault();
            float changeInFunds = finalFundsTotalInGBP / initialFundsTotalInGBP;
            float polygonPriceChange = pairPricingDictionary[$"{_secondCoinName} prices"].LastOrDefault() / pairPricingDictionary[$"{_secondCoinName} prices"].FirstOrDefault();
        }

       private void placeTheoreticalOrders(Dictionary<string, float[]> pairPricingDictionary)
       {
            for (int i = 1; i < pairPricingDictionary[$"{_firstCoinName} prices"].Length; i++)
            {
                float currentDayBitcoinPrice = pairPricingDictionary[$"{_firstCoinName} prices"][i];
                float yesterdayBitcoinPrice = pairPricingDictionary[$"{_firstCoinName} prices"][i - 1];
                float bitcoinPriceIncrease = currentDayBitcoinPrice / yesterdayBitcoinPrice;
                if (bitcoinPriceIncrease > 1.02)
                {
                    placeTheoreritcalBuyOrder(pairPricingDictionary, i);
                }
                if (bitcoinPriceIncrease < 0.98)
                {
                    placeTheoreritcalSellOrder(pairPricingDictionary, i);
                }
            }
        }

        private void placeTheoreritcalBuyOrder(Dictionary<string, float[]> pairPricingDictionary, int currentDayIndex)
        {
            _gbpFunds -= 1;
            float currentDayPolygonPrice = pairPricingDictionary[$"{_secondCoinName} prices"][currentDayIndex];
            float polygonQuanityPurchased = 1 / currentDayPolygonPrice;
            _polygonFunds += polygonQuanityPurchased;
        }

        private void placeTheoreritcalSellOrder(Dictionary<string, float[]> pairPricingDictionary, int currentDayIndex)
        {
            _gbpFunds += 1;
            float currentDayPolygonPrice = pairPricingDictionary[$"{_secondCoinName} prices"][currentDayIndex];
            float polygonQuanitySold = 1 / currentDayPolygonPrice;
            _polygonFunds -= polygonQuanitySold;
        }
    }
}
