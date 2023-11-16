using Newtonsoft.Json.Linq;

namespace backtesting
{
    public class PricingData
    {
        private readonly string _indicatorCoinName;
        private readonly string _trailingCoinName;
        private readonly string _indicatorCoinRequestUrl;
        private readonly string _trailingCoinRequestUrl;
        private readonly HttpClient _httpClient;

        public PricingData(string indicatorCoinName, string trailingCoinName, string indicatorCoinRequesUrl, string trailingCoinRequesUrl)
        {
            _indicatorCoinName = indicatorCoinName;
            _trailingCoinName = trailingCoinName;
            _indicatorCoinRequestUrl = indicatorCoinRequesUrl;
            _trailingCoinRequestUrl = trailingCoinRequesUrl;
            _httpClient = new HttpClient();
        }
        public async Task<Dictionary<string, float[]>> GetPriceComparisonDictionary()
        {
            float[] indicatorCoinPrices = await getCoinPrices(_indicatorCoinRequestUrl);
            float[] trailingCoinPrices = await getCoinPrices(_trailingCoinRequestUrl);

            Dictionary<string, float[]> pairPricingDictionary = new Dictionary<string, float[]>
            {
                { $"{_indicatorCoinName} prices", indicatorCoinPrices },
                { $"{_trailingCoinName} prices", trailingCoinPrices }
            };

            return pairPricingDictionary;
        }

        private async Task<float[]> getCoinPrices(string requestUrl)
        {
            string responseForIndicatorCoinPriceDataContent = await getPriceDataContent(requestUrl);

            JObject coinJsonObject = JObject.Parse(responseForIndicatorCoinPriceDataContent);

            Dictionary<string, object[][]>? coinPriceDictionary = coinJsonObject.ToObject<Dictionary<string, object[][]>>();

            return coinPriceDictionary["prices"].Select(x => Convert.ToSingle(x[1])).ToArray();
        }

        private async Task<string> getPriceDataContent(string requestUrl)
        {
            HttpResponseMessage coinPriceDataResponse = await _httpClient.GetAsync(requestUrl);

            coinPriceDataResponse.EnsureSuccessStatusCode();

            string responseForIndicatorCoinPriceDataContent = await coinPriceDataResponse.Content.ReadAsStringAsync();

            return responseForIndicatorCoinPriceDataContent;
        }
    }
}
