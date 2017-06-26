using System.Threading.Tasks;
using System.Web.Mvc;
using OnboardingConsumer.Models;
using OnboardingConsumer.Utilities;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.Configuration;
using System.IO;
using System.Text;

namespace OnboardingConsumer.Controllers
{
    public class ClientDetailsController : Controller
    {
        // GET: ClientDetails
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetClientDetails(ClientDetailsId clientDetailsId)
        {
            var model = new ClientDetails();

            using (new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["APIUrl"];

                var request =
                    WebRequest.CreateHttp(url + "api/ClientDetails/" + clientDetailsId.ReykerClientId);
                request.ContentType = "text/json";
                request.Method = "GET";

                //NEED TO CHANGE USERNAME TO PROVIDED USERNAME
                const string authHeader = "Reyker USERNAME";
                request.Headers.Add("Authorization", authHeader);

                try
                {
                    var response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            var objText = reader.ReadToEnd();
                            model = await objText.AES_Decrypt<ClientDetails>();
                            if (model.TelephoneNumbers == null)
                            {
                                model.TelephoneNumbers = new List<TelephoneNumber>();
                            }
                            if (model.Addresses == null)
                            {
                                model.Addresses = new List<ClientAddress>();
                            }
                            if (model.BankAccounts == null)
                            {
                                model.BankAccounts = new List<ClientBankAccount>();
                            }
                            if (model.Plans == null)
                            {
                                model.Plans = new List<ClientPlan>();
                            }
                            if (model.AML == null)
                            {
                                model.AML = new ClientAml();
                            }
                            if (model.Citizenships == null)
                            {
                                model.Citizenships = new List<ClientCitizenship>();
                            }
                        }
                    }
                    ViewData["ResponseStatusCode"] = response.StatusCode;
                    ViewData["ResponseStatusMessage"] = response.StatusDescription;
                }
                catch (WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var objText = reader.ReadToEnd();

                        model.ErrorMessage = objText;
                        model.TelephoneNumbers = new List<TelephoneNumber>();
                        model.Addresses = new List<ClientAddress>();
                        model.BankAccounts = new List<ClientBankAccount>();
                        model.Plans = new List<ClientPlan>();
                        model.AML = new ClientAml();
                        model.Citizenships = new List<ClientCitizenship>();
                    }

                    ViewData["ResponseStatusCode"] = response.StatusCode;
                    ViewData["ResponseStatusMessage"] = response.StatusDescription;
                }

            }

            return View("ClientDetailsResult", model);
        }
    }
}