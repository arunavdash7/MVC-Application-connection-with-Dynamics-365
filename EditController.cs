using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using System.Web.Mvc;
using TestAppMVCAndCRM.DAL;
using TestAppMVCAndCRM.Models;

namespace TestAppMVCAndCRM.Controllers
{
    public class EditController : Controller
    {
        [HttpGet]
        public ActionResult Edit(Guid accountId)

        {



            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            AccountEntityModels accountmodel = new AccountEntityModels();
            accountmodel = objDAL.getCurrentRecord(accountId);
            List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();

            if (refUsers.Count > 0)

            {

                ViewBag.EntityReferenceUsers = new SelectList(refUsers, "Id", "Name");

            }
            return View(accountmodel);

        }

        [HttpPost]
        public ActionResult Edit(AccountEntityModels objdmodel)

        {

            DAL_AccountEntity objDAL = new DAL_AccountEntity();

            List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();
            if (refUsers.Count > 0)

            {

                ViewBag.EntityReferenceUsers = new SelectList(refUsers, "Id", "Name");

            }
            AccountEntityModels objmodel = new AccountEntityModels();
            // Guid accountId = objmodel.AccountID;
            //objmodel = objDAL.getCurrentRecord(accountId);
            //objmodel = objDAL.retrievecontact(objdmodel);
            objmodel.AccountID = objdmodel.AccountID;
            objmodel.AccountName = objdmodel.AccountName;
            objmodel.NumberOfEmployees = objdmodel.NumberOfEmployees;
            objmodel.Revenue = objdmodel.Revenue;
            //objmodel.PrimaryContact = new EntityReference("account", objdmodel.PrimaryContact.Id);
            objmodel.PrimaryContact = objdmodel.PrimaryContact;

            objDAL.EditAccount(objmodel);
            return View("DataUpdate");

            //return View(objdmodel);

        }

    }
}
