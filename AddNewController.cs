using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestAppMVCAndCRM.DAL;
using TestAppMVCAndCRM.Models;

namespace TestAppMVCAndCRM.Controllers
{
    public class AddNewController : Controller
    {
        // GET: AccountAddNew
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddNew()

        {

            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            AccountEntityModels accountdmodel = new AccountEntityModels();
            List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();
            AccountEntityModels objmodel = new AccountEntityModels();

            //objmodel.AccountName = Request.Form;
            //objDAL.SaveAccount(objmodel);

            if (refUsers.Count > 0)

            {

                ViewBag.EntityReferenceUsers = new SelectList(refUsers, "Id", "Name");

            }


            return View();

           

        }

        [HttpPost]
        public ActionResult AddNew(AccountEntityModels accountdmodel)

        {

            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();
            AccountEntityModels objmodel = new AccountEntityModels();
            objmodel.AccountName = accountdmodel.AccountName;
            objmodel.NumberOfEmployees = accountdmodel.NumberOfEmployees;
            objmodel.Revenue = accountdmodel.Revenue;
            objmodel.PrimaryContact = accountdmodel.PrimaryContact;
           
            if (refUsers.Count > 0)

            {
                //IEnumerable<SelectListItem> primarycontactlookup = (IEnumerable<SelectListItem>)refUsers;

                ViewBag.EntityReferenceUsers = new SelectList(refUsers.ToList(), "Id", "Name");
                
            }
            objDAL.SaveAccount(objmodel);
            return View("SuccessMessage");



        }







    }
}
