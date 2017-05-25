using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestAppMVCAndCRM.DAL;
using TestAppMVCAndCRM.Models;

namespace TestAppMVCAndCRM.Controllers
{
    public class AccountRetrieveController : Controller
    {
        // GET: AccountRetrieve
        public ActionResult Index()
        {
            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            List<AccountEntityModels> accountinfo = objDAL.RetriveRecords();
            ViewBag.accountinfo = accountinfo;
            return View();
        }

    }
}