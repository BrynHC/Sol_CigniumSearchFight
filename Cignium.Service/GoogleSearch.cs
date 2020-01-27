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

namespace Cignium.Service
{
    public class GoogleSearch
    {
        private readonly GoogleEntity oGoogleEntity = new GoogleEntity();

        public GoogleSearch()
        {
            oGoogleEntity.GoogleUrl = ConfigurationManager.AppSettings["GoogleSearchUrl"];
            oGoogleEntity.GoogleCx = ConfigurationManager.AppSettings["GoogleCx"];
            oGoogleEntity.GoogleKey = ConfigurationManager.AppSettings["GoogleKey"];
        }
        
        #region Methods

        public Result GetGoogleSearchList(string searchWord)
        {
            Result googleResult = new Result();
            googleResult.ProgrammingLanguage = searchWord;

            try
            {
                string resquestUriString = $"{oGoogleEntity.GoogleUrl}{oGoogleEntity.GoogleKey}&cx={oGoogleEntity.GoogleCx}&q={searchWord}";

                var request = WebRequest.Create(resquestUriString);
                HttpWebResponse oHttpWebResponse = (HttpWebResponse)request.GetResponse();
                Stream oStream = oHttpWebResponse.GetResponseStream();
                StreamReader oStreamReader = new StreamReader(oStream);
                string readerString = oStreamReader.ReadToEnd();
                dynamic jsonData = JsonConvert.DeserializeObject(readerString);
                
                foreach (var item in jsonData.queries)
                {
                    foreach (var value in item.Value)
                        googleResult.GoogleAmount = Get<long>(value.totalResults.Value);
                    break;
                }
            }
            catch (Exception ex)
            {
                googleResult.ErrorMessage = ex.Message;
            }

            return googleResult;
        }

        private T Get<T>(dynamic value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion
    }
}
