using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EncrpytionTest.Models;
using EncrpytionTest.Utilities;

namespace EncrpytionTest.Controllers
{
    public class HomeController : Controller
    {
        AuthenticatedUser au = new AuthenticatedUser()
        {
            ApiUser = new ApiUser()
            {
                Username = "TEST",
                Salt = "ReykerExample",
                AESKey = "lr1Jwa9IO6l6iF5EccZ8S5fAkFMwkkkfHKyzRLntrJQ=",
            }
        };


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> EncryptMessage(EncryptionVm encryptionVm)
        {
            var data = new JavaScriptSerializer().Serialize(encryptionVm);
            try
            {
                var decryptedMessage = await data.AES_Decrypt<OnboardingClientDetails>(au.ApiUser);

                var encryptedMessage = await decryptedMessage.AES_Encrypt(au.ApiUser);

                return Json(encryptedMessage);

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("Something went wrong");
        }
    }

}