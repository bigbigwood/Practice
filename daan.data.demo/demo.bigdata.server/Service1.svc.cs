using Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace demo.bigdata.server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string GetData(string filePath)
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

            return string.Format("You entered: {0}", filePath);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
