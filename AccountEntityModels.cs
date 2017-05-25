using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAppMVCAndCRM.Models
{
    public class AccountEntityModels
    {
       public Guid AccountID { get; set; }
       public string AccountName { get; set; }
       public int NumberOfEmployees { get; set; }
       public Money Revenue{ get; set; }
       public EntityReference PrimaryContact { get; set; }
       public string PrimaryContactName { get; set; }
       public decimal RevenueValue { get; set; }

    }
}