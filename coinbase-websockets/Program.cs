using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main()
    {
        HttpClient client = new HttpClient();

        HttpResponseMessage responseForBitcoinPriceData = await client.GetAsync("https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=gbp&days=365&interval=daily");
        HttpResponseMessage responseForPolygonPriceData = await client.GetAsync("https://api.coingecko.com/api/v3/coins/chainlink/market_chart?vs_currency=gbp&days=365&interval=daily");

        responseForBitcoinPriceData.EnsureSuccessStatusCode();
        responseForPolygonPriceData.EnsureSuccessStatusCode();

        string responseForBitcoinPriceDataContent = await responseForBitcoinPriceData.Content.ReadAsStringAsync();
        string responseForPolygonPriceDataContent = await responseForPolygonPriceData.Content.ReadAsStringAsync();

        var bitcoinJsonObject = JObject.Parse(responseForBitcoinPriceDataContent);
        var bitcoinPriceDictionary = bitcoinJsonObject.ToObject<Dictionary<string, object[][]>>();
        var polygonJsonObject = JObject.Parse(responseForPolygonPriceDataContent);
        var polygonPriceDictionary = polygonJsonObject.ToObject<Dictionary<string, object[][]>>();

        var bitcoinPrices = bitcoinPriceDictionary["prices"].Select(x => Convert.ToSingle(x[1])).ToArray();
        var polygonPrices = polygonPriceDictionary["prices"].Select(x => Convert.ToSingle(x[1])).ToArray();

        var pairPricingDictionary = new Dictionary<string, float[]>
        {
            { "bitcoin prices", bitcoinPrices },
            { "polygon prices", polygonPrices }
        };

        float gbpFunds = 1000;
        float polygonFunds = 1000;
        float initialFundsTotalInGBP = gbpFunds + polygonFunds * pairPricingDictionary["polygon prices"].FirstOrDefault();

        for (int i = 1; i < pairPricingDictionary["bitcoin prices"].Length; i++)
        {
            float currentDayBitcoinPrice = pairPricingDictionary["bitcoin prices"][i];
            float yesterdayDayBitcoinPrice = pairPricingDictionary["bitcoin prices"][i - 1];
            float bitcoinPriceIncrease = currentDayBitcoinPrice / yesterdayDayBitcoinPrice;
            if (bitcoinPriceIncrease > 1.02)
            {
                gbpFunds -= 1;
                float currentDayPolygonPrice = pairPricingDictionary["polygon prices"][i];
                float polygonQuanityPurchased = 1 / currentDayPolygonPrice;
                polygonFunds += polygonQuanityPurchased;
                //Console.WriteLine("bitcoin up more than 2%");
            }
            if (bitcoinPriceIncrease < 0.98)
            {
                gbpFunds += 1;
                float currentDayPolygonPrice = pairPricingDictionary["polygon prices"][i];
                float polygonQuanitySold = 1 / currentDayPolygonPrice;
                polygonFunds -= polygonQuanitySold;
                //Console.WriteLine("bitcoin down more than 2%");
            }
        }
        float finalFundsTotalInGBP = gbpFunds + polygonFunds * pairPricingDictionary["polygon prices"].FirstOrDefault();
        float changeInFunds = finalFundsTotalInGBP / initialFundsTotalInGBP;
        float polygonPriceChange = pairPricingDictionary["polygon prices"].LastOrDefault() / pairPricingDictionary["polygon prices"].FirstOrDefault();
    }
}
