using Cignium.Entitites;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Cignium.Service
{
    public class BingSearch
    {
        private readonly BingEntity oBingEntity = new BingEntity();

        public BingSearch()
        {
            oBingEntity.BingUrl = ConfigurationManager.AppSettings["BingSearchUrl"];
            oBingEntity.BingCustomConfigId = ConfigurationManager.AppSettings["BingCustomConfigId"];
            oBingEntity.BingKey = ConfigurationManager.AppSettings["BingKey"];
        }

        public Result GetBingSearchList(string searchWord)
        {
            Result bingResult = new Result();
            bingResult.ProgrammingLanguage = searchWord;

            try
            {
                string requestUriString = $"{oBingEntity.BingUrl}{searchWord}&customconfig={oBingEntity.BingCustomConfigId}";

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", oBingEntity.BingKey);

                var httpResponseMessage = client.GetAsync(requestUriString).Result;
                var responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                dynamic response = JsonConvert.DeserializeObject(responseContent);

                bingResult.BingAmount = response.webPages.totalEstimatedMatches.Value;
            }
            catch (Exception ex)
            {
                bingResult.ErrorMessage = ex.Message;
            }

            return bingResult;
        }
    }
}
