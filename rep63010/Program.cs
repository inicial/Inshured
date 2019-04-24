using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using ltp_v2.Framework;

namespace rep6050
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(params string[] args)
        {
            string user = (args.Length > 0) ? args[0] : "";
            string pass = (args.Length > 1) ? args[1] : "";
            LogonScreen screen = new LogonScreen(user, pass, Application.ProductName);
            
            if (screen.Show() == DialogResult.OK)
            {
                string UsingDGCode = ltp_v2.Framework.MasterValue.DGCodeFromASKData;
                if ((args.Length > 2) && (args[2].IndexOf("!DGCODE=") >= 0))
                {
                    UsingDGCode = args[2].Replace("!DGCODE=", "");
                }
                // dgcode=MSC40629A1
string LantaSqlConnection = "Data Source=192.168.10.4;Initial Catalog=test;User ID={0}; pwd={1}; Timeout=30;";
string ConnectionUserName = global::ltp_v2.Framework.SqlConnection.ConnectionUserName;
string ConnectionPassword = global::ltp_v2.Framework.SqlConnection.ConnectionPassword;
string ConnectionString = string.Format(LantaSqlConnection, ConnectionUserName, ConnectionPassword);
System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);
                //System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ltp_v2.Framework.SqlConnection.Connection);
                conn.Open();
                frmMain mainForm = new frmMain(conn, UsingDGCode);
                
                if ((args.Length > 2) && (args[2].IndexOf("!bordero") >= 0)) // && false)
                {
                   
#if DEBUG
                    mainForm.bordero(new DateTime(2014, 05, 03).Date, new DateTime(2014, 05, 06).Date);
#else
                    mainForm.bordero(DateTime.Now.Date.AddDays(-1),DateTime.Now.Date.AddDays(0));
#endif
                }
                else
                {
                    mainForm.GetDate();
#if DEBUG
                    mainForm.Text += " DateBaseName \"" + conn.Database + "\"";
#endif
                    mainForm.Text +=  " ProductVersion " + Application.ProductVersion;
                    Application.Run(mainForm);
                }
                
                
            }
        }
    }
}
