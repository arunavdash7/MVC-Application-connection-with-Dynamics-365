using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestAppMVCAndCRM.DAL;
using TestAppMVCAndCRM.Models;

namespace TestAppMVCAndCRM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Retrieving Primary Contact details
        public List<Microsoft.Xrm.Sdk.EntityReference> GetEntityReference()
        {
            try
            {
                List<Microsoft.Xrm.Sdk.EntityReference> info = new List<Microsoft.Xrm.Sdk.EntityReference>();
                //using (OrganizationService service = new OrganizationService("MyConnectionString"))
                //{
                IOrganizationService service = DAL_AccountEntity.GetConnection();
                QueryExpression query = new QueryExpression
                {
                    EntityName = "contact",
                    ColumnSet = new ColumnSet("contactid", "fullname")
                };
                EntityCollection PrimaryContact = service.RetrieveMultiple(query);
                if (PrimaryContact != null && PrimaryContact.Entities.Count > 0)
                {
                    Microsoft.Xrm.Sdk.EntityReference itm;
                    for (int i = 0; i < PrimaryContact.Entities.Count; i++)
                    {
                        itm = new EntityReference();
                        if (PrimaryContact[i].Id != null)
                            itm.Id = PrimaryContact[i].Id;
                        if (PrimaryContact[i].Contains("fullname") && PrimaryContact[i]["fullname"] != null)
                            itm.Name = PrimaryContact[i]["fullname"].ToString();
                        itm.LogicalName = "contact";
                        info.Add(itm);
                    }
                }
                //}
                return info;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //
        public JsonResult GetAccount(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            List<AccountEntityModels> accountinfo = objDAL.RetriveRecords();

            //List<AccountEntityModels> StudentList = new List<AccountEntityModels>();
            //StudentList.Add(new AccountEntityModels { AccountID = Guid.NewGuid(), AccountName = "Ganesh", NumberOfEmployees = 5 });
            //StudentList.Add(new AccountEntityModels { AccountID = Guid.NewGuid(), AccountName = "Siva", NumberOfEmployees = 6 });

            int totalRecords = accountinfo.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = accountinfo
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //create
        [HttpPost]
        public string AddNew(AccountEntityModels accountdmodel)

        {

            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            //List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();
            AccountEntityModels objmodel = new AccountEntityModels();
            objmodel.AccountName = accountdmodel.AccountName;
            objmodel.NumberOfEmployees = accountdmodel.NumberOfEmployees;
            objmodel.Revenue =new Microsoft.Xrm.Sdk.Money(accountdmodel.RevenueValue) ;
            objmodel.PrimaryContact = new Microsoft.Xrm.Sdk.EntityReference( accountdmodel.PrimaryContactName);

            //if (refUsers.Count > 0)

            //{
            //    //IEnumerable<SelectListItem> primarycontactlookup = (IEnumerable<SelectListItem>)refUsers;

            //    ViewBag.EntityReferenceUsers = new SelectList(refUsers.ToList(), "Id", "Name");

            //}
            objDAL.SaveAccount(objmodel);
            return "Data is created";


        }
        //Edit

        public string Edit(AccountEntityModels objdmodel)

        {

            DAL_AccountEntity objDAL = new DAL_AccountEntity();

            List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();
            //if (refUsers.Count > 0)

            //{

            //    ViewBag.EntityReferenceUsers = new SelectList(refUsers, "Id", "Name");

            //}
            AccountEntityModels objmodel = new AccountEntityModels();
            // Guid accountId = objmodel.AccountID;
            //objmodel = objDAL.getCurrentRecord(accountId);
            //objmodel = objDAL.retrievecontact(objdmodel);
            objmodel.AccountID = objdmodel.AccountID;
            objmodel.AccountName = objdmodel.AccountName;
            objmodel.NumberOfEmployees = objdmodel.NumberOfEmployees;
            objmodel.Revenue = new Microsoft.Xrm.Sdk.Money(objdmodel.RevenueValue);
            //objmodel.PrimaryContact = new EntityReference("account", objdmodel.PrimaryContact.Id);
            objmodel.PrimaryContact = new Microsoft.Xrm.Sdk.EntityReference( objdmodel.PrimaryContactName);

            objDAL.EditAccount(objmodel);
            return "Data is updated";

            //return View(objdmodel);

        }

        //Delete
        public string DeleteAccount(Guid AccountID)
        {

            IOrganizationService service = DAL_AccountEntity.GetConnection();
            service.Delete("account", AccountID);
            return "Deleted Successfully";

        }

    }
}