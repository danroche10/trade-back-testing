using Newtonsoft.Json.Linq;

namespace backtesting
{
    public class PricingData
    {
        private readonly string _firstCoinName;
        private readonly string _secondCoinName;
        private readonly string _firstCoinRequestUrl;
        private readonly string _secondCoinRequestUrl;
        public PricingData(string firstCoinName, string secondCoinName, string firstCoinRequesUrl, string secondCoinRequesUrl)
        {
            _firstCoinName = firstCoinName;
            _secondCoinName = secondCoinName;
            _firstCoinRequestUrl = firstCoinRequesUrl;
            _secondCoinRequestUrl = secondCoinRequesUrl;
        }
        public async Task<Dictionary<string, float[]>> GetPriceComparisonDictionary()
        {
            var firstCoinPrices = await getCoinPrices(_firstCoinRequestUrl);
            var secondCoinPrices = await getCoinPrices(_secondCoinRequestUrl);

            var pairPricingDictionary = new Dictionary<string, float[]>
            {
                { $"{_firstCoinName} prices", firstCoinPrices },
                { $"{_secondCoinName} prices", secondCoinPrices }
            };

            return pairPricingDictionary;
        }

        private async Task<float[]> getCoinPrices(string requestUrl)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage coinPriceDataResponse = await client.GetAsync(requestUrl);

            coinPriceDataResponse.EnsureSuccessStatusCode();

            string responseForFirstCoinPriceDataContent = await coinPriceDataResponse.Content.ReadAsStringAsync();

            var coinJsonObject = JObject.Parse(responseForFirstCoinPriceDataContent);
            var coinPriceDictionary = coinJsonObject.ToObject<Dictionary<string, object[][]>>();

            return coinPriceDictionary["prices"].Select(x => Convert.ToSingle(x[1])).ToArray();
        }
    }
}
