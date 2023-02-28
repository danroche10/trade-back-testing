using Newtonsoft.Json.Linq;

namespace backtesting
{
    public class PricingData
    {
        private readonly string _indicatorCoinName;
        private readonly string _trailingCoinName;
        private readonly string _indicatorCoinRequestUrl;
        private readonly string _trailingCoinRequestUrl;
        public PricingData(string indicatorCoinName, string trailingCoinName, string indicatorCoinRequesUrl, string trailingCoinRequesUrl)
        {
            _indicatorCoinName = indicatorCoinName;
            _trailingCoinName = trailingCoinName;
            _indicatorCoinRequestUrl = indicatorCoinRequesUrl;
            _trailingCoinRequestUrl = trailingCoinRequesUrl;
        }
        public async Task<Dictionary<string, float[]>> GetPriceComparisonDictionary()
        {
            var indicatorCoinPrices = await getCoinPrices(_indicatorCoinRequestUrl);
            var trailingCoinPrices = await getCoinPrices(_trailingCoinRequestUrl);

            var pairPricingDictionary = new Dictionary<string, float[]>
            {
                { $"{_indicatorCoinName} prices", indicatorCoinPrices },
                { $"{_trailingCoinName} prices", trailingCoinPrices }
            };

            return pairPricingDictionary;
        }

        private async Task<float[]> getCoinPrices(string requestUrl)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage coinPriceDataResponse = await client.GetAsync(requestUrl);

            coinPriceDataResponse.EnsureSuccessStatusCode();

            string responseForIndicatorCoinPriceDataContent = await coinPriceDataResponse.Content.ReadAsStringAsync();

            var coinJsonObject = JObject.Parse(responseForIndicatorCoinPriceDataContent);
            var coinPriceDictionary = coinJsonObject.ToObject<Dictionary<string, object[][]>>();

            return coinPriceDictionary["prices"].Select(x => Convert.ToSingle(x[1])).ToArray();
        }
    }
}
