using Newtonsoft.Json.Linq;

namespace backtesting
{
    public class PricingData
    {        
        public async Task<Dictionary<string, float[]>> GetPriceComparisonDictionary(string firstCoinName, string secondCoinName, string firstCoinRequesUrl, string secondCoinRequesUrl)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage responseForFirstCoinPriceData = await client.GetAsync(firstCoinRequesUrl);
            HttpResponseMessage responseForSecondCoinPriceData = await client.GetAsync(secondCoinRequesUrl);

            responseForFirstCoinPriceData.EnsureSuccessStatusCode();
            responseForSecondCoinPriceData.EnsureSuccessStatusCode();

            string responseForFirstCoinPriceDataContent = await responseForFirstCoinPriceData.Content.ReadAsStringAsync();
            string responseForSecondCoinPriceDataContent = await responseForSecondCoinPriceData.Content.ReadAsStringAsync();

            var firstCoinJsonObject = JObject.Parse(responseForFirstCoinPriceDataContent);
            var firstCoinPriceDictionary = firstCoinJsonObject.ToObject<Dictionary<string, object[][]>>();
            var secondCoinJsonObject = JObject.Parse(responseForSecondCoinPriceDataContent);
            var secondCoinPriceDictionary = secondCoinJsonObject.ToObject<Dictionary<string, object[][]>>();

            var firstCoinPrices = firstCoinPriceDictionary["prices"].Select(x => Convert.ToSingle(x[1])).ToArray();
            var secondCoinPrices = secondCoinPriceDictionary["prices"].Select(x => Convert.ToSingle(x[1])).ToArray();

            var pairPricingDictionary = new Dictionary<string, float[]>
            {
                { $"{firstCoinName} prices", firstCoinPrices },
                { $"{secondCoinName} prices", secondCoinPrices }
            };

            return pairPricingDictionary;
        }
    }
}
