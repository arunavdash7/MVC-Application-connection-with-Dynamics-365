using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.Web;
using TestAppMVCAndCRM.Models;

namespace TestAppMVCAndCRM.DAL
{
    
    public class DAL_AccountEntity
    {
        public static IOrganizationService _service;
        public List<AccountEntityModels> RetriveRecords()
        {
            //        string conn = System.Configuration.ConfigurationManager.
            //ConnectionStrings["MyConnectionString"].ConnectionString; ;

            //string conns= System.Configuration.ConfigurationManager.AppSettings["MyConnectionString"];
            ////var connection = new CrmConnection("MyConnectionString");
            ////var service1 = new OrganizationService(connection);
            ////var context = new CrmOrganizationServiceContext(connection);

            //using (OrganizationService service = new OrganizationService(CrmConnection.Parse(conns)))
            IOrganizationService service = GetConnection();
            QueryExpression query = new QueryExpression
            {
                EntityName = "account",
                ColumnSet = new ColumnSet("accountid", "name", "revenue", "numberofemployees", "primarycontactid")
            };
            List<AccountEntityModels> info = new List<AccountEntityModels>();
            EntityCollection accountRecord = service.RetrieveMultiple(query);
            if (accountRecord != null && accountRecord.Entities.Count > 0)
            {
                AccountEntityModels accountModel;
                for (int i = 0; i < accountRecord.Entities.Count; i++)
                {
                    accountModel = new AccountEntityModels();
                    if (accountRecord[i].Contains("accountid") && accountRecord[i]["accountid"] != null)
                        accountModel.AccountID = (Guid)accountRecord[i]["accountid"];
                    if (accountRecord[i].Contains("name") && accountRecord[i]["name"] != null)
                        accountModel.AccountName = accountRecord[i]["name"].ToString();
                    if (accountRecord[i].Contains("revenue") && accountRecord[i]["revenue"] != null)
                        accountModel.RevenueValue = ((Money)accountRecord[i]["revenue"]).Value;
                    if (accountRecord[i].Contains("numberofemployees") && accountRecord[i]["numberofemployees"] != null)
                        accountModel.NumberOfEmployees = (int)accountRecord[i]["numberofemployees"];
                    if (accountRecord[i].Contains("primarycontactid") && accountRecord[i]["primarycontactid"] != null)
                        accountModel.PrimaryContactName = ((EntityReference)accountRecord[i]["primarycontactid"]).Name;
                    if (accountRecord[i].Contains("primarycontactid") && accountRecord[i]["primarycontactid"] != null)
                        accountModel.PrimaryContact = ((EntityReference)accountRecord[i]["primarycontactid"]);
                    info.Add(accountModel);
                }
            }
            return info;
        }


        public static IOrganizationService GetConnection()
        {
            //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            IOrganizationService s1 = ConnectToMSCRM("arunav@robertbosch12345.onmicrosoft.com", "Qwerty@123", "https://robertbosch12345.api.crm8.dynamics.com/XRMServices/2011/Organization.svc");
            // Guid userid = ((WhoAmIResponse)_service.Execute(new WhoAmIRequest())).UserId;
            // if (userid != Guid.Empty)
            // {
            //  return s1;
            //  }
            return s1;
        }
        public static IOrganizationService ConnectToMSCRM(string UserName, string Password, string SoapOrgServiceUri)
        {
            IOrganizationService OrgService = null;
            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = UserName;
                credentials.UserName.Password = Password;
                Uri serviceUri = new Uri(SoapOrgServiceUri);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                OrgService = (IOrganizationService)proxy;

            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return (OrgService);
        }

        //Get entity Reference from the primarycontact lookup

        public List<Microsoft.Xrm.Sdk.EntityReference> GetEntityReference()
        {
            try
            {
                List<Microsoft.Xrm.Sdk.EntityReference> info = new List<Microsoft.Xrm.Sdk.EntityReference>();
                //using (OrganizationService service = new OrganizationService("MyConnectionString"))
                //{
                IOrganizationService service = GetConnection();
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

        //edit the current record ie getCurrentRecord(Guid accountId)

        public AccountEntityModels getCurrentRecord(Guid accountId)
        {

            AccountEntityModels accountModel = new AccountEntityModels();
            //using (OrganizationService service = new OrganizationService("MyConnectionString"))
            //{
            IOrganizationService service = GetConnection();
            Money m = new Money(0);
            ColumnSet cols = new ColumnSet(new String[] { "name", "revenue", "numberofemployees", "primarycontactid" });
                Entity account = service.Retrieve("account", accountId, cols);
            if (account.Contains("accountid") && account["accountid"] != null)
                accountModel.AccountID = (Guid)account["accountid"];
            if (account.Contains("name") && account["name"] != null)
                accountModel.AccountName = account["name"].ToString();
            if (account.Contains("revenue") && account["revenue"] != null)
                accountModel.RevenueValue = ((Money)account["revenue"]).Value;
            if (account.Contains("numberofemployees") && account["numberofemployees"] != null)
                accountModel.NumberOfEmployees = (int)account["numberofemployees"];
            if (account.Contains("primarycontactid") && account["primarycontactid"] != null)
                accountModel.PrimaryContactName = ((EntityReference)account["primarycontactid"]).Name;
            if (account.Contains("primarycontactid") && account["primarycontactid"] != null)
                accountModel.PrimaryContact = ((EntityReference)account["primarycontactid"]);
            //entity type

            //if (account.Contains("primarycontactid") && account["primarycontactid"] != null)
            //    accountModel.PrimaryContactType = ((EntityReference)account["primarycontactid"]).LogicalName;
            //accountModel.AccountID = accountId;
            //accountModel.AccountName = account.Attributes["name"].ToString();
            //accountModel.NumberOfEmployees = (int)account.Attributes["numberofemployees"];
            //if (account.Attributes.Contains("revenue") && (account.Attributes["revenue"]) != null)
            //{

            //    accountModel.RevenueValue = (((Money)account.Attributes["revenue"]).Value);
            //}
            //else
            //{
            //    accountModel.RevenueValue = 0;
            //}
            //if (account.Attributes.Contains("primarycontactid") && (account.Attributes["primarycontactid"]) != null)
            //{
            //    accountModel.PrimaryContact = account.Attributes["primarycontactid"].;
            //}
            //else
            //{
            //    accountModel.PrimaryContact = null;
            //}
            // }
            return accountModel;
        }

        //Saving account

        public void SaveAccount(AccountEntityModels objAccountModel)
        {
            
            IOrganizationService service = GetConnection();
            Entity AccountEntity = new Entity("account");

            //if (objAccountModel.AccountID != Guid.Empty)
            //{
            var x = Guid.NewGuid();
            AccountEntity["accountid"] = x;
            //}
            AccountEntity["name"] = objAccountModel.AccountName;
            AccountEntity["numberofemployees"] = objAccountModel.NumberOfEmployees;
            AccountEntity["revenue"] = objAccountModel.Revenue;
            AccountEntity["primarycontactid"] = new Microsoft.Xrm.Sdk.EntityReference { Id = objAccountModel.PrimaryContact.Id, LogicalName = "contact" };

            //objAccountModel.AccountID = Guid.Empty;
            //    {
                    objAccountModel.AccountID = service.Create(AccountEntity);
                //}
                //else
                //{
                //    service.Update(AccountEntity);
                //}
            //}
        }
        //Edit Account 
        public void EditAccount(AccountEntityModels objAccountModel)
        {
            IOrganizationService service = GetConnection();
            Entity AccountEntity = new Entity("account");
            AccountEntity["accountid"] = objAccountModel.AccountID;
            AccountEntity["name"] = objAccountModel.AccountName;
            AccountEntity["numberofemployees"] = objAccountModel.NumberOfEmployees;
            AccountEntity["revenue"] = objAccountModel.Revenue;
            AccountEntity["primarycontactid"] = new Microsoft.Xrm.Sdk.EntityReference { Id = objAccountModel.PrimaryContact.Id, LogicalName = objAccountModel.PrimaryContact.LogicalName};
            service.Update(AccountEntity);
        }
        //public AccountEntityModels retrievecontact(AccountEntityModels objAccountModel)
        //{
        //    AccountEntityModels accountModel = new AccountEntityModels();
        //    //IOrganizationService service = GetConnection();
        //    //Entity contactrecord = service.Retrieve("contact", objAccountModel.PrimaryContact.Id, new ColumnSet("contactid", "fullname"));
        //    //accountModel.PrimaryContactName = contactrecord.GetAttributeValue<string>("fullname");
        //    accountModel.PrimaryContact = new EntityReference("account", objAccountModel.PrimaryContact.Id);
        //    return accountModel;
        //}
    }
}
