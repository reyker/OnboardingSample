using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OnboardingConsumer.Models;
using OnboardingConsumer.Utilities;

namespace OnboardingConsumer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PostOnboardingClientDetails(OnboardingClientDetails submitionData)
        {
            var model = new ClientDetails();
            using (new HttpClient())
            {
                var request = WebRequest.CreateHttp("https://reykeronboardingexternaltest.azurewebsites.net/api/Onboarding/");
                request.ContentType = "text/json";
                request.Method = "POST";

                //NEED TO CHANGE USERNAME TO PROVIDED USERNAME
                const string authHeader = "Reyker USERNAME";
                request.Headers.Add("Authorization", authHeader);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var encryptedSubmissionData = await submitionData.AES_Encrypt();
                    var json = new JavaScriptSerializer().Serialize(encryptedSubmissionData);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    var response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            var objText = reader.ReadToEnd();
                            model = await objText.AES_Decrypt<ClientDetails>();
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
                        if (!string.IsNullOrEmpty(objText))
                        {
                            model = await objText.AES_Decrypt<ClientDetails>();
                        }
                    }

                    ViewData["ResponseStatusCode"] = response.StatusCode;
                    ViewData["ResponseStatusMessage"] = response.StatusDescription;
                }
            }
            return View("OnboardingResult", model);
        }

        //public async Task<ClientDetails> AddAdditionalPlan()
        //{
        //    var model = new ClientDetails();

        //    var submitionData = new OnboardingPlanDetails()
        //    {
        //         ExternalCustomerId = "ExternalCustomerId",
        //         ExternalPlanId = "ExternalPlanId",
        //         PlanType = 10
        //    };

        //    using (new HttpClient())
        //    {
        //        var request = WebRequest.CreateHttp("https://reykeronboardingexternaltest.azurewebsites.net/api/AdditionalPlan/");
        //        request.ContentType = "text/json";
        //        request.Method = "POST";

        //        //NEED TO CHANGE USERNAME TO PROVIDED USERNAME
        //        const string authHeader = "Reyker USERNAME";
        //        request.Headers.Add("Authorization", authHeader);

        //        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //        {
        //            var encryptedSubmissionData = await submitionData.AES_Encrypt();
        //            var json = new JavaScriptSerializer().Serialize(encryptedSubmissionData);
        //            streamWriter.Write(json);
        //            streamWriter.Flush();
        //            streamWriter.Close();
        //        }

        //        using (var response = request.GetResponse() as HttpWebResponse)
        //        {
        //            var x = response;
        //            if (response != null && response.StatusCode == HttpStatusCode.OK)
        //            {
        //                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        //                {
        //                    var objText = reader.ReadToEnd();
        //                    model = await objText.AES_Decrypt<ClientDetails>();
        //                }
        //            }
        //        }
        //    }
        //    return model;
        //}

        //public async Task<ClientDetails> GetClientDetails()
        //{
        //    var model = new ClientDetails();

        //    using (new HttpClient())
        //    {
        //        var request = WebRequest.CreateHttp("https://reykeronboardingexternaltest.azurewebsites.net/api/ClientDetails/48707");
        //        request.ContentType = "text/json";
        //        request.Method = "GET";

        //        var authHeader = "Reyker USERNAME";
        //        request.Headers.Add("Authorization", authHeader);

        //        try
        //        {

        //            using (var response = request.GetResponse() as HttpWebResponse)
        //            {
        //                var x = 1;
        //                if (response != null && response.StatusCode == HttpStatusCode.OK)
        //                {
        //                    using (var reader = new StreamReader(response.GetResponseStream()))
        //                    {
        //                        var objText = await reader.ReadToEndAsync();
        //                        model = await objText.AES_Decrypt<ClientDetails>();
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            model.ErrorMessage = ex.Message;
        //        }
        //    }
        //    return model;
        //}

        //public async Task<List<ClientDetails>> GetAllClientsDetails()
        //{
        //    var model = new List<ClientDetails>();

        //    using (new HttpClient())
        //    {
        //        var request = WebRequest.CreateHttp("https://reykeronboardingexternaltest.azurewebsites.net/api/BulkClientDetails/");
        //        request.ContentType = "text/json";
        //        request.Method = "GET";

        //        var authHeader = "Reyker USERNAME";
        //        request.Headers.Add("Authorization", authHeader);

        //        try
        //        {

        //            using (var response = request.GetResponse() as HttpWebResponse)
        //            {
        //                var x = 1;
        //                if (response != null && response.StatusCode == HttpStatusCode.OK)
        //                {
        //                    using (var reader = new StreamReader(response.GetResponseStream()))
        //                    {
        //                        var objText = await reader.ReadToEndAsync();
        //                        model = await objText.AES_Decrypt<List<ClientDetails>>();
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var error = ex.Message;
        //        }
        //    }
        //    return model;
        //}
    }
}