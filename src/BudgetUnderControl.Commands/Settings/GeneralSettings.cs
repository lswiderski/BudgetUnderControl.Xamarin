using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.CommonInfrastructure.Settings
{
    public class GeneralSettings
    {
        public string AppName { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string ApplicationId { get; set; }
        public string ConnectionString
        {
            get
            {
                return $"Data Source={BUC_DB_Address};Initial Catalog={BUC_DB_Name};User ID={BUC_DB_User};Password={BUC_DB_Password};";
            }
        }
        public string ApiBaseUri { get; set; }
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public int JWTExpiresDays { get; set; }

        public string BUC_DB_Address { get; set; }
        public string BUC_DB_Name { get; set; }
        public string BUC_DB_User { get; set; }
        public string BUC_DB_Password { get; set; }

        public string FileRootPath { get; set; }

        public string AdMobAdId { get; set; }

        public string SyncfusionLicenseKey { get; set; }

        public GeneralSettings InjectEnvVariables()
        {
            this.BUC_DB_Address = Environment.GetEnvironmentVariable(nameof(this.BUC_DB_Address)) ?? this.BUC_DB_Address;
            this.BUC_DB_Name = Environment.GetEnvironmentVariable(nameof(this.BUC_DB_Name)) ?? this.BUC_DB_Name;
            this.BUC_DB_User = Environment.GetEnvironmentVariable(nameof(this.BUC_DB_User)) ?? this.BUC_DB_User;
            this.BUC_DB_Password = Environment.GetEnvironmentVariable(nameof(this.BUC_DB_Password)) ?? this.BUC_DB_Password;
            return this;
        }
    }
}
