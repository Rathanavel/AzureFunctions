using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using CryptoFunction.Helper;

namespace CryptoFunction
{
    public static class Crypto
    {
        [FunctionName("Crypto")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
        HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string action = req.GetQueryNameValuePairs()
                 .FirstOrDefault(q => string.Compare(q.Key, "action", true) == 0)
                 .Value;

            string cryptoKey = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "cryptokey", true) == 0)
                .Value;

            string value = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "content", true) == 0)
                .Value;

            CryptoResult result = new CryptoResult();

            switch (action.ToUpper())
            {
                case "ENCRYPT":
                    {
                        result = CryptoHelper.Encrypt(value, cryptoKey);
                        break;
                    }
                case "DECRYPT":
                    {
                        result = CryptoHelper.Decrypt(value, cryptoKey);
                        break;
                    }
                default:
                    {
                        result.HasError = true;
                        result.Error = "Action did not matched";
                        break;
                    }
            }

            return result.HasError == true
                ? req.CreateResponse(HttpStatusCode.BadRequest, result.Error)
                : req.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
