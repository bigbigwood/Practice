using demo.bigdata.zipper;
using Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace demo.bigdata.server
{
    /// <summary>
    /// Summary description for UploadFileHandler
    /// </summary>
    public class UploadFileHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger("HPWelcomeSMSListener");
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFile file = context.Request.Files[0];

                    string directory = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "demo.bigdata.server");
                    if (Directory.Exists(directory) == false)
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string filePath = string.Format(@"{0}\{1}", directory, file.FileName);
                    file.SaveAs(filePath);

                    string zipPath = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, @"bin\7z\7z.exe");
                    var sevenZipper = new SevenZipper();
                    sevenZipper.UnZip(filePath, filePath.Substring(0, filePath.LastIndexOf(".")), zipPath);

                    BuldInsertData(filePath.Substring(0, filePath.LastIndexOf(".")));

                    context.Response.Write("Success/r/n");
                    return;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    context.Response.Write("Error/r/n");
                    return;
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        private void BuldInsertData(string filePath)
        {
            string conn = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            MySqlHelper mySqlHelper = new MySqlHelper(conn);
            //var data = mySqlHelper.ExecuteDataTable("select * from bloguser");

            List<DataColumn> columns = new List<DataColumn>();
            columns.Add(new DataColumn() { ColumnName = "batchNumber" });
            columns.Add(new DataColumn() { ColumnName = "scanDate" });
            columns.Add(new DataColumn() { ColumnName = "department" });
            columns.Add(new DataColumn() { ColumnName = "section" });
            columns.Add(new DataColumn() { ColumnName = "package" });
            columns.Add(new DataColumn() { ColumnName = "barcode" });
            columns.Add(new DataColumn() { ColumnName = "name" });
            columns.Add(new DataColumn() { ColumnName = "gender" });
            columns.Add(new DataColumn() { ColumnName = "birthday" });
            columns.Add(new DataColumn() { ColumnName = "age" });
            columns.Add(new DataColumn() { ColumnName = "mobileNumber" });
            columns.Add(new DataColumn() { ColumnName = "remark" });
            columns.Add(new DataColumn() { ColumnName = "marrystatus" });
            columns.Add(new DataColumn() { ColumnName = "idNumber" });
            columns.Add(new DataColumn() { ColumnName = "address" });
            columns.Add(new DataColumn() { ColumnName = "homeNumber" });
            columns.Add(new DataColumn() { ColumnName = "email" });


            DataTable table = new DataTable();
            table.TableName = "daan_orders2";
            table.Columns.AddRange(columns.ToArray());

            Log.Info("Start insert");
            var counter = mySqlHelper.BulkInsertByFile(filePath, table);
            Log.InfoFormat("Finish insert {0} rows", counter);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}