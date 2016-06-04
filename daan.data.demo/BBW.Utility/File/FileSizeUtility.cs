using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBW.Utility.File
{
    public class FileSizeUtility
    {
        public static Int32 decimals = 2;
        public static decimal CalculateFileSizeInKBytes(Int64 fileLength)
        {
            return Math.Round((decimal)fileLength / 1024, decimals);
        }

        public static decimal CalculateFileSizeInMBytes(Int64 fileLength)
        {
            return Math.Round((decimal) CalculateFileSizeInKBytes(fileLength) / 1024, decimals);
        }

        public static decimal CalculateFileSizeInGBytes(Int64 fileLength)
        {
            return Math.Round((decimal)CalculateFileSizeInMBytes(fileLength) / 1024, decimals);
        }
    }
}
