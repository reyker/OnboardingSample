using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OnboardingConsumer.Models;
using OnboardingConsumer.Utilities;

namespace OnboardingConsumer.Controllers
{
    public class BulkClientDetailsController : Controller
    {
        // GET: BulkClientDetails
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetBulkClientDetails()
        {
            var model = new List<ClientDetails>();

            using (new HttpClient())
            {
                var url = ConfigurationManager.AppSettings["APIUrl"];

                var request =
                    WebRequest.CreateHttp(url + "api/BulkClientDetails/");
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
                            model = await objText.AES_Decrypt<List<ClientDetails>>();

                            foreach (var clientDetails in model)
                            {

                                if (clientDetails.TelephoneNumbers == null)
                                {
                                    clientDetails.TelephoneNumbers = new List<TelephoneNumber>();
                                }
                                if (clientDetails.Addresses == null)
                                {
                                    clientDetails.Addresses = new List<ClientAddress>();
                                }
                                if (clientDetails.BankAccounts == null)
                                {
                                    clientDetails.BankAccounts = new List<ClientBankAccount>();
                                }
                                if (clientDetails.Plans == null)
                                {
                                    clientDetails.Plans = new List<ClientPlan>();
                                }
                                if (clientDetails.AML == null)
                                {
                                    clientDetails.AML = new ClientAml();
                                }
                                if (clientDetails.Citizenships == null)
                                {
                                    clientDetails.Citizenships = new List<ClientCitizenship>();
                                }
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

                        var cd = new ClientDetails();

                        cd.ErrorMessage = objText;
                        cd.TelephoneNumbers = new List<TelephoneNumber>();
                        cd.Addresses = new List<ClientAddress>();
                        cd.BankAccounts = new List<ClientBankAccount>();
                        cd.Plans = new List<ClientPlan>();
                        cd.AML = new ClientAml();
                        cd.Citizenships = new List<ClientCitizenship>();

                        model.Add(cd);
                    }

                    ViewData["ResponseStatusCode"] = response.StatusCode;
                    ViewData["ResponseStatusMessage"] = response.StatusDescription;
                }

            }

            return View("BulkClientDetailsResult", model);
        }
    }
}