using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Runtime.InteropServices;
using xl = Microsoft.Office.Interop.Excel;
namespace LibertyTax.Common
{
    class ExcelUtility
    {
        xl.Application xlApp = null;
        xl.Workbooks workbooks = null;
        xl.Workbook workbook = null;
        Hashtable sheets;
        public string xlFilePath;

        public ExcelUtility(string xlFilePath)
        {
            this.xlFilePath = xlFilePath;
        }

        public void OpenExcel()
        {
            try
            {
                xlApp = new xl.Application();
                workbooks = xlApp.Workbooks;
                workbook = workbooks.Open(xlFilePath);
                sheets = new Hashtable();
                int count = 1;
                foreach (xl.Worksheet sheet in workbook.Sheets)
                {
                    sheets[count] = sheet.Name;
                    count++;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CloseExcel()
        {
            workbook.Close(false, xlFilePath, null);
            Marshal.FinalReleaseComObject(workbook);
            workbook = null;

            workbooks.Close();
            Marshal.FinalReleaseComObject(workbooks);
            workbooks = null;

            xlApp.Quit();
            Marshal.FinalReleaseComObject(xlApp);
            xlApp = null;
        }

        public string GetCellDataPosition(string sheetName, string fnName, string colName, int pos = 0)
        {
            OpenExcel();

            int value = 0;
            int sheetValue = 0;
            int colNumber = 0;


            if (sheets.ContainsValue(sheetName))
            {
                foreach (DictionaryEntry sheet in sheets)
                {
                    if (sheet.Value.Equals(sheetName))
                    {
                        sheetValue = (int)sheet.Key;
                    }
                }
                xl.Worksheet worksheet = null;
                worksheet = workbook.Worksheets[sheetValue] as xl.Worksheet;
                xl.Range range = worksheet.UsedRange;

                for (int i = 1; i <= range.Rows.Count; i++)
                {
                    string colNameValue = Convert.ToString((range.Cells[i, 1] as xl.Range).Value2);
                    if (colNameValue != null)
                    {
                        if (colNameValue.ToLower() == fnName.ToLower())
                        {
                            colNumber = i;
                            break;
                        }
                    }
                }

                value = colNumber;
                Marshal.FinalReleaseComObject(worksheet);
                worksheet = null;
            }
            int col = GetCellDataPositionCol(sheetName, colName, colNumber - 1);
            var name = GetCellData(sheetName, col, value);
            CloseExcel();
            return name;
        }
        public int GetCellDataPositionCol(string sheetName, string colName, int pos = 0)
        {
            ////OpenExcel();

            int value = 0;
            int sheetValue = 0;
            int colNumber = 0;

            if (sheets.ContainsValue(sheetName))
            {
                foreach (DictionaryEntry sheet in sheets)
                {
                    if (sheet.Value.Equals(sheetName))
                    {
                        sheetValue = (int)sheet.Key;
                    }
                }
                xl.Worksheet worksheet = null;
                worksheet = workbook.Worksheets[sheetValue] as xl.Worksheet;
                xl.Range range = worksheet.UsedRange;

                for (int i = 1; i <= range.Columns.Count; i++)
                {
                    string colNameValue = Convert.ToString((range.Cells[pos, i] as xl.Range).Value2);
                    if (colNameValue != null)
                    {
                        if (colNameValue.ToLower() == colName.ToLower())
                        {
                            colNumber = i;
                            break;
                        }
                    }
                }

                value = colNumber;
                Marshal.FinalReleaseComObject(worksheet);
                worksheet = null;
            }
            //CloseExcel();
            return value;
        }
        public string GetCellData(string sheetName, int col, int row)
        {
            //OpenExcel();
            int sheetValue = 0;
            string colNameValue = "";
            foreach (DictionaryEntry sheet in sheets)
            {
                if (sheet.Value.Equals(sheetName))
                {
                    sheetValue = (int)sheet.Key;
                }
            }
            xl.Worksheet worksheet = null;
            worksheet = workbook.Worksheets[sheetValue] as xl.Worksheet;
            xl.Range range = worksheet.UsedRange;

            colNameValue = Convert.ToString((range.Cells[row, col] as xl.Range).Value2);
            //CloseExcel();
            return colNameValue;
        }
    }
}
