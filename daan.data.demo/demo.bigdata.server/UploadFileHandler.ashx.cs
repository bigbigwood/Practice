using Helpers;
using log4net;
using SevenZip;
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

                    string fileNameWithoutExtension = file.FileName.Substring(0, file.FileName.LastIndexOf("."));
                    string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf("."));
                    string filePath = string.Format(@"{0}\{1}_{2}{3}", directory, fileNameWithoutExtension, DateTime.Now.ToString("yyyyMMddHHmmssfff"), fileExtension);
                    file.SaveAs(filePath);

                    using (var tmp = new SevenZipExtractor(filePath))
                    {
                        tmp.ExtractArchive(directory);
                    }

                    string afterZipFilePath = string.Format(@"{0}\{1}", directory, fileNameWithoutExtension);
                    var counter = BuldInsertData(afterZipFilePath);

                    context.Response.Write("Success");
                    context.Response.Write(string.Format("[{0}] rows data inserted.", counter));
                    return;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    context.Response.Write("Error: " + ex.ToString());
                    return;
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("No upload file was recoginzed.");
        }

        private Int32 BuldInsertData(string filePath)
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

            return counter;
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