using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.Dac;

namespace ConsoleApp10
{
    class Program
    {
        string dacpacFilePath = @"D:\Inficare_Projects\HCF\HCF_Code\src\hcfc_core\Database\HCF_HospitalDb\bin\Debug\";
        string dacpacFileName = "HCF_HospitalDb.dacpac";

        static void Main(string[] args)
        {
            Console.WriteLine("you are using " + dacpacFileName.Split('.')[0].Split('_')[1]);
            Console.WriteLine("Before publish the database changes, First build the databse project.");
            string mode = "";
            bool confirmed = false;
            while (!confirmed)
            {
                Console.WriteLine("Enter db server environment.");
                mode = Console.ReadLine();
                Console.WriteLine("You entered, " + mode + " as your db environment");
                Console.WriteLine("Are you sure you want to choose this db environment? [yes/no]");
                string option = Console.ReadLine();
                if (option == "yes")
                {
                    confirmed = true;
                }
            }

            UpgradeDatabase(mode);

            Console.WriteLine($"Press any key to exit...");
            Console.ReadKey();
        }

        private static void UpgradeDatabase(string mode)
        {
            string dbPath = dacpacFilePath;
            string fileName = dacpacFileName;
            string[] dbNames = GetDataBaseNames(mode);
            var targetServer = GetTargetDataBaseDetails(mode);

            foreach (string dbName in dbNames)
            {
                Console.WriteLine("==================================================");
                try
                {
                    var connectionString = $"Server={targetServer.TargetServer};Database={dbName};User Id={targetServer.targetServerUserName};Password={targetServer.targetServerPassword};";
                    Console.WriteLine("connecting " + targetServer.TargetServer);
                    var dacpacLocation = Directory.GetFiles(dbPath, fileName, SearchOption.AllDirectories)[0];
                    var dbPackage = DacPackage.Load(dacpacLocation);
                    var services = new DacServices(connectionString);
                    DacDeployOptions dacDeployOptions = new DacDeployOptions();
                    dacDeployOptions.SqlCommandVariableValues.Add("HCF_DB", "HCF");
                    Console.WriteLine("deploy started of " + dbName +" on " + targetServer.TargetServer);
                    services.Deploy(dbPackage, dbName, true, dacDeployOptions);
                    Console.WriteLine("deploy completed of " + dbName + " on " + targetServer.TargetServer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Occoured with {dbName}");
                    Console.WriteLine($"{ex.Message}");
                }

                Console.WriteLine("==================================================");
            }
        }

        private static TargetServerDetails GetTargetDataBaseDetails(string mode)
        {
            TargetServerDetails target = new TargetServerDetails();
            if (mode == "live")
            {
                // live server details
                target.TargetServer = "hcfdb.cp2lnsptp3oi.us-east-1.rds.amazonaws.com";
                target.targetServerUserName = "admin";
                target.targetServerPassword = "P9#1h[tQ(t~LAYy";
            }
            else if (mode == "staging")
            {

                //  test server details
                target.TargetServer = "testrdsdb.cp2lnsptp3oi.us-east-1.rds.amazonaws.com";
                target.targetServerUserName = "admin";
                target.targetServerPassword = "HCFCompliance#123";
            }
            else
            {

                //  local server details
                target.TargetServer = "192.168.75.35";
                target.targetServerUserName = "sa";
                target.targetServerPassword = "sql@123";
            }
            return target;
        }

        private static string[] GetDataBaseNames(string mode)
        {
            var strings = new List<string>();
            if (mode == "live")
            {
                strings = new List<string>() {
                         "HCF_Holy","HCF_HolyTest","HCF_ACH","HCF_Atlantic","HCF_Burke","HCF_NBIMC","HCF_GWVMC","HCF_GMH",
                         "HCF_GMC","HCF_GLH","HCF_GJS","HCF_GCMC","HCF_GBH","HCF_CRxMC"
                    };
            }
            else if (mode == "staging")
            {
                strings = new List<string>()
                {
                    "HCF_TestHospital"
                };
            }
            else
            {
                strings = new List<string>()
                    {
                     "HCF_Holy",
                     "HCF_Atlantic",
                     "HCF_ACH"
                    };
            }
            return strings.ToArray();
        }
    }

}
