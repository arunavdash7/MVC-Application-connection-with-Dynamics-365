using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestAppMVCAndCRM.DAL;

namespace TestAppMVCAndCRM.Controllers
{
    public class DeleteController : Controller
    {
        // GET: Delete
        public ActionResult Index()
        {
            return View();
        }
     
        public ActionResult Delete(Guid AccountID)
        {
            //using (OrganizationService service = new OrganizationService("MyConnectionString"))
            //{
            IOrganizationService service = DAL_AccountEntity.GetConnection();
            service.Delete("account", AccountID);


            return RedirectToAction("Index", "AccountRetrieve");
            //}
        }

    }
}