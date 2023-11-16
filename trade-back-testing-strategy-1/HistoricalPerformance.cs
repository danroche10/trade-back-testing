namespace backtesting
{
    public class HistoricalPerformance
    {
        private float _gbpFunds;
        private float _trailingCoinFunds;
        private readonly string _indicatorCoinName;
        private readonly string _trailingCoinName;
        public HistoricalPerformance(float gbpFunds, float trailingCoinFunds, string indicatorCoinName, string trailingCoinName)
        {
            _indicatorCoinName = indicatorCoinName; 
            _trailingCoinName = trailingCoinName;
            _gbpFunds = gbpFunds;
            _trailingCoinFunds = trailingCoinFunds;
        }
        public void GetStrategyPerformance(Dictionary<string, float[]> pairPricingDictionary, out float changeInFunds, out float trailingCoinPriceChange)
        {
            float initialFundsTotalInGBP = _gbpFunds + (_trailingCoinFunds * pairPricingDictionary[$"{_trailingCoinName} prices"].FirstOrDefault());

            placeTheoreticalOrders(pairPricingDictionary);

            float finalFundsTotalInGBP = _gbpFunds + (_trailingCoinFunds * pairPricingDictionary[$"{_trailingCoinName} prices"].FirstOrDefault());
            changeInFunds = finalFundsTotalInGBP / initialFundsTotalInGBP;
            trailingCoinPriceChange = pairPricingDictionary[$"{_trailingCoinName} prices"].LastOrDefault() / pairPricingDictionary[$"{_trailingCoinName} prices"].FirstOrDefault();
        }

       private void placeTheoreticalOrders(Dictionary<string, float[]> pairPricingDictionary)
       {
            double buyTriggerValue = 1.02;
            double sellTriggerValue = 0.98;

            for (int dayIndex = 1; dayIndex < pairPricingDictionary[$"{_indicatorCoinName} prices"].Length; dayIndex++)
            {
                float currentDayIndicatorCoinPrice = pairPricingDictionary[$"{_indicatorCoinName} prices"][dayIndex];
                float yesterdayIndicatorCoinPrice = pairPricingDictionary[$"{_indicatorCoinName} prices"][dayIndex - 1];
                float IndicatorCoinPriceIncrease = currentDayIndicatorCoinPrice / yesterdayIndicatorCoinPrice;

                if (IndicatorCoinPriceIncrease > buyTriggerValue)
                {
                    placeTheoreritcalBuyOrder(pairPricingDictionary, dayIndex);
                }

                if (IndicatorCoinPriceIncrease < sellTriggerValue)
                {
                    placeTheoreritcalSellOrder(pairPricingDictionary, dayIndex);
                }
            }
        }

        private void placeTheoreritcalBuyOrder(Dictionary<string, float[]> pairPricingDictionary, int currentDayIndex)
        {
            _gbpFunds -= 1;

            float currentDaytrailingCoinPrice = pairPricingDictionary[$"{_trailingCoinName} prices"][currentDayIndex];
            float trailingCoinQuanityPurchased = 1 / currentDaytrailingCoinPrice;

            _trailingCoinFunds += trailingCoinQuanityPurchased;
        }

        private void placeTheoreritcalSellOrder(Dictionary<string, float[]> pairPricingDictionary, int currentDayIndex)
        {
            _gbpFunds += 1;

            float currentDaytrailingCoinPrice = pairPricingDictionary[$"{_trailingCoinName} prices"][currentDayIndex];
            float trailingCoinQuanitySold = 1 / currentDaytrailingCoinPrice;

            _trailingCoinFunds -= trailingCoinQuanitySold;
        }
    }
}
