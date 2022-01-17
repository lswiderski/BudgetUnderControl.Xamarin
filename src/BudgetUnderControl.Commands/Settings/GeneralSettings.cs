using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.CommonInfrastructure.Settings
{
    public class GeneralSettings
    {
        public string ApplicationId { get; set; }
        public string ApiBaseUri { get; set; }
        public string ApiKey { get; set; }
        public string BUC_DB_Name { get; set; }
        public string AdMobAdId { get; set; }
        public string SyncfusionLicenseKey { get; set; }
    }
}
