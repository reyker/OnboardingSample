using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using OnboardingConsumer.Models;
using OnboardingConsumer.Models.ReykerOnboarding.Models;
using OnboardingConsumer.Utilities;

namespace OnboardingConsumer.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //ClientDetails cd = await PostOnboardingClientDetails();
            var cd = await GetAllClientsDetails();
            return View(cd[0]);
        }

        public async Task<ClientDetails> PostOnboardingClientDetails()
        {
            var model = new ClientDetails();
            var submittionData = new OnboardingClientDetails()
            {
                Title = "Mr",
                Forenames = "John",
                Surname = "Doe",
                CountryOfBirth = 1,
                EmailAddress = "john.doe1989@reyker.com",
                EmailType = "WORK",
                BirthDate = DateTime.Now.AddYears(-30),
                PrimaryAddress = new OnboardingPrimaryAddress() { Address1 = "Street 1", City = "London", Postcode = "E15JP", Country = 1},
                PrimaryTelephone = new OnboardingTelephoneNumber() { DialingCode = 1, Number = "0770345657", TelephoneType = 1},
                BankAccount = new OnboardingBankAccount() { AccountName = "John Doe Account", AccountNumber = "123456789", SortCode = "12-34-56"},
                PrimaryCitizenship = new OnboardingCitizenship() { CountryOfResidency = 1, TaxIdentificationNumber = "AZ34654Z"},
                PlanType = 10,
                ExternalCustomerId = "22",
                ExternalPlanId = "10"
            };
            


            using (new HttpClient())
            {
                HttpWebRequest request = WebRequest.CreateHttp("http://localhost:54742/api/Onboarding");
                request.ContentType = "text/json";
                request.Method = "POST";

                //NEED TO CHANGE USERNAME TO PROVIDED USERNAME
                var authHeader = "Reyker USERNAME";
                request.Headers.Add("Authorization", authHeader);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string encryptedSubmissionData = await submittionData.AES_Encrypt();
                    string json = new JavaScriptSerializer().Serialize(encryptedSubmissionData);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string objText = reader.ReadToEnd();
                            model = await objText.AES_Decrypt<ClientDetails>();
                        }
                    }
                }
            }
            return model;
        }

        public async Task<ClientDetails> GetClientDetails()
        {
            var model = new ClientDetails();

            using (new HttpClient())
            {
                var request = WebRequest.CreateHttp("http://localhost:54742/api/Onboarding/47417");
                request.ContentType = "text/json";
                request.Method = "GET";

                var authHeader = "Reyker Crowdstacker";
                request.Headers.Add("Authorization", authHeader);

                try
                {

                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        var x = 1;
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                var objText = await reader.ReadToEndAsync();
                                model = await objText.AES_Decrypt<ClientDetails>();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                }
            }
            return model;
        }

        public async Task<List<ClientDetails>> GetAllClientsDetails()
        {
            var model = new List<ClientDetails>();

            using (new HttpClient())
            {
                var request = WebRequest.CreateHttp("http://localhost:54742/api/Onboarding");
                request.ContentType = "text/json";
                request.Method = "GET";

                var authHeader = "Reyker Crowdstacker";
                request.Headers.Add("Authorization", authHeader);

                try
                {

                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        var x = 1;
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                var objText = await reader.ReadToEndAsync();
                                model = await objText.AES_Decrypt<List<ClientDetails>>();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                }
            }
            return model;
        }
    }
}