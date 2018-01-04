using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;

namespace NLTD.EmployeePortal.LMS.Ux.AppHelpers
{
    //http://www.c-sharpcorner.com/article/export-to-excel-in-asp-net-mvc/
    public class ExcelExportHelper
    {
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, 14];
                    //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count()); //commented by suresh
                    //if (maxLength < 150)
                    //{
                        workSheet.Column(columnIndex).AutoFit();
                    //}


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }
                //custom code for this excel
                using (ExcelRange col = workSheet.Cells[2, 5, 2 + dataTable.Rows.Count, 5])
                {
                    col.Style.Numberformat.Format = "dd/MM/yyyy";
                    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                //

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }

        public static byte[] ExportPermissionsExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, 15];
                    //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count()); //commented by suresh
                    //if (maxLength < 150)
                    //{
                     workSheet.Column(columnIndex).AutoFit();
                    //}
                    //workSheet.Column(9).Style.WrapText = true;
                    //workSheet.Column(10).Style.WrapText = true;
                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }
                //custom code for this excel
                using (ExcelRange col = workSheet.Cells[2, 5, 2 + dataTable.Rows.Count, 5])
                {
                    col.Style.Numberformat.Format = "dd/MM/yyyy";
                    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                //

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportPermissionsExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportPermissionsExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }
        public static byte[] ExportExcelYearSummary(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }
               

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcelYearSummary<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcelYearSummary(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }


        public static byte[] ExportExcelMonthSummary(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 3 : 5;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }




                // add the content into the Excel file  
                workSheet.Cells["A2"].Value = "Emp Id";
                workSheet.Cells["B2"].Value = "Name";

                workSheet.Cells["C2"].Value = "CL/SL";
                workSheet.Cells["D2"].Value = "EL";
                workSheet.Cells["E2"].Value = "LWP";
                workSheet.Cells["F2"].Value = "CO";

                workSheet.Cells["G2"].Value = "CL/SL";
                workSheet.Cells["H2"].Value = "EL";
                workSheet.Cells["I2"].Value = "LWP";
                workSheet.Cells["J2"].Value = "CO";

                workSheet.Cells["K2"].Value = "CL/SL";
                workSheet.Cells["L2"].Value = "EL";
                workSheet.Cells["M2"].Value = "LWP";
                workSheet.Cells["N2"].Value = "CO";

                workSheet.Cells["O2"].Value = "CL/SL";
                workSheet.Cells["P2"].Value = "EL";
                workSheet.Cells["Q2"].Value = "LWP";
                workSheet.Cells["R2"].Value = "CO";

                workSheet.Cells["S2"].Value = "CL/SL";
                workSheet.Cells["T2"].Value = "EL";
                workSheet.Cells["U2"].Value = "LWP";
                workSheet.Cells["V2"].Value = "CO";

                workSheet.Cells["W2"].Value = "CL/SL";
                workSheet.Cells["X2"].Value = "EL";
                workSheet.Cells["Y2"].Value = "LWP";
                workSheet.Cells["Z2"].Value = "CO";

                workSheet.Cells["AA2"].Value = "CL/SL";
                workSheet.Cells["AB2"].Value = "EL";
                workSheet.Cells["AC2"].Value = "LWP";
                workSheet.Cells["AD2"].Value = "CO";

                workSheet.Cells["AE2"].Value = "CL/SL";
                workSheet.Cells["AF2"].Value = "EL";
                workSheet.Cells["AG2"].Value = "LWP";
                workSheet.Cells["AH2"].Value = "CO";

                workSheet.Cells["AI2"].Value = "CL/SL";
                workSheet.Cells["AJ2"].Value = "EL";
                workSheet.Cells["AK2"].Value = "LWP";
                workSheet.Cells["AL2"].Value = "CO";

                workSheet.Cells["AM2"].Value = "CL/SL";
                workSheet.Cells["AN2"].Value = "EL";
                workSheet.Cells["AO2"].Value = "LWP";
                workSheet.Cells["AP2"].Value = "CO";

                workSheet.Cells["AQ2"].Value = "CL/SL";
                workSheet.Cells["AR2"].Value = "EL";
                workSheet.Cells["AS2"].Value = "LWP";
                workSheet.Cells["AT2"].Value = "CO";

                workSheet.Cells["AU2"].Value = "CL/SL";
                workSheet.Cells["AV2"].Value = "EL";
                workSheet.Cells["AW2"].Value = "LWP";
                workSheet.Cells["AX2"].Value = "CO";


                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, false);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }
                
                
                workSheet.Cells[1, 3, 1, 6].Merge = true;
                workSheet.Cells[1,1,1,6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 3, 1, 6].Value = "January";
                workSheet.Cells[1, 3, 1, 6].Style.Font.Bold = true;
//                workSheet.Cells[1, 3, 1, 6].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                workSheet.Cells[1, 3, 1, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 3, 1, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 7, 1, 10].Merge = true;
                workSheet.Cells[1, 7, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 7, 1, 10].Value = "February";
                workSheet.Cells[1, 7, 1, 10].Style.Font.Bold = true;
//                workSheet.Cells[1, 7, 1, 10].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                workSheet.Cells[1, 7, 1, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 7, 1, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 11, 1, 14].Merge = true;
                workSheet.Cells[1, 11, 1, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 11, 1, 14].Value = "March";
                workSheet.Cells[1, 11, 1, 14].Style.Font.Bold = true;
                workSheet.Cells[1, 11, 1, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 11, 1, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 15, 1, 18].Merge = true;
                workSheet.Cells[1, 15, 1, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 15, 1, 18].Value = "April";
                workSheet.Cells[1, 15, 1, 18].Style.Font.Bold = true;
                workSheet.Cells[1, 15, 1, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 15, 1, 18].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 19, 1, 22].Merge = true;
                workSheet.Cells[1, 19, 1, 22].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 19, 1, 22].Value = "May";
                workSheet.Cells[1, 19, 1, 22].Style.Font.Bold = true;
                workSheet.Cells[1, 19, 1, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 19, 1, 22].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 23, 1, 26].Merge = true;
                workSheet.Cells[1, 23, 1, 26].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 23, 1, 26].Value = "June";
                workSheet.Cells[1, 23, 1, 26].Style.Font.Bold = true;
                workSheet.Cells[1, 23, 1, 26].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 23, 1, 26].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 27, 1, 30].Merge = true;
                workSheet.Cells[1, 27, 1, 30].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 27, 1, 30].Value = "July";
                workSheet.Cells[1, 27, 1, 30].Style.Font.Bold = true;
                workSheet.Cells[1, 27, 1, 30].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 27, 1, 30].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 31, 1, 34].Merge = true;
                workSheet.Cells[1, 31, 1, 34].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 31, 1, 34].Value = "August";
                workSheet.Cells[1, 31, 1, 34].Style.Font.Bold = true;
                workSheet.Cells[1, 31, 1, 34].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 31, 1, 34].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 35, 1, 38].Merge = true;
                workSheet.Cells[1, 35, 1, 38].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 35, 1, 38].Value = "September";
                workSheet.Cells[1, 35, 1, 38].Style.Font.Bold = true;
                workSheet.Cells[1, 35, 1, 38].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 35, 1, 38].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 39, 1, 42].Merge = true;
                workSheet.Cells[1, 39, 1, 42].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 39, 1, 42].Value = "October";
                workSheet.Cells[1, 39, 1, 42].Style.Font.Bold = true;
                workSheet.Cells[1, 39, 1, 42].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 39, 1, 42].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 43, 1, 46].Merge = true;
                workSheet.Cells[1, 43, 1, 46].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 43, 1, 46].Value = "November";
                workSheet.Cells[1, 43, 1, 46].Style.Font.Bold = true;
                workSheet.Cells[1, 43, 1, 46].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 43, 1, 46].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));

                workSheet.Cells[1, 47, 1, 50].Merge = true;
                workSheet.Cells[1, 47, 1, 50].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, 47, 1, 50].Value = "December";
                workSheet.Cells[1, 47, 1, 50].Style.Font.Bold = true;
                workSheet.Cells[1, 47, 1, 50].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                workSheet.Cells[1, 47, 1, 50].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffff00"));


                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[2,1, 2, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[1, 1, 1 + dataTable.Rows.Count+1, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                using (ExcelRange r = workSheet.Cells[3, 2, 1 + dataTable.Rows.Count + 1, 2])
                {
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                    // removed ignored columns  
                    for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }


                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcelMonthSummary<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcelMonthSummary(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }

        public static byte[] ExportExcelAttendence(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    //if (maxLength < 150)
                    //{
                        workSheet.Column(columnIndex).AutoFit();
                    //}


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }


                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcelAttendence(List<EmployeeAttendanceModel> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            List < EmployeeAttendanceModel > employeeAttendanceModelListObj = new List<EmployeeAttendanceModel>();
            foreach (var d in data)
            {
                EmployeeAttendanceModel employeeAttendanceModelObj = new EmployeeAttendanceModel();
                employeeAttendanceModelObj.InOut = d.InOut;
                employeeAttendanceModelObj.AttendenceDate = d.InOutDate.ToString("dd-MM-yyyy");
                employeeAttendanceModelObj.INOutTime = d.InOutDate.ToString("hh:mm:ss");
                employeeAttendanceModelListObj.Add(employeeAttendanceModelObj);
            }
            return ExportExcelAttendence(ListToDataTable<EmployeeAttendanceModel>(employeeAttendanceModelListObj), Heading, showSlno, ColumnsToTake);
        }

        public static byte[] ExportExcelTimeSheet(List<TimeSheetModel> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            
            return ExportExcelAttendence(ListToDataTable<TimeSheetModel>(data), Heading, showSlno, ColumnsToTake);
        }

        public static byte[] ExportTimesheetExcel(List<TimeSheetModel> TimeSheetModelObj, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            List<Int64> userIDList = (from m in TimeSheetModelObj select m.userID).Distinct().ToList();
            using (ExcelPackage package = new ExcelPackage())
            {
                for (int i = 0; i < userIDList.Count; i++)
                {
                    
                    List<TimeSheetModel> TimeSheetModelObjForSingleEmp = (from m in TimeSheetModelObj
                                                                          where m.userID == userIDList[i]
                                                                          select m).ToList();
                    DataTable dataTable = ListToDataTable<TimeSheetModel>(TimeSheetModelObjForSingleEmp);
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                    int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                    if (showSrNo)
                    {
                        DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                        dataColumn.SetOrdinal(0);
                        int index = 1;
                        foreach (DataRow item in dataTable.Rows)
                        {
                            item[0] = index;
                            index++;
                        }
                    }


                    // add the content into the Excel file  
                    workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                    // autofit width of cells with small content  
                    int columnIndex = 1;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                        //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        //if (maxLength < 150)
                        //{
                        workSheet.Column(columnIndex).AutoFit();
                        //}


                        columnIndex++;
                    }

                    // format header - bold, yellow on black  
                    using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                    }

                    // format cells - add borders  
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }

                    // removed ignored columns  
                    for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                    {
                        if (i == 0 && showSrNo)
                        {
                            continue;
                        }
                        if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                        {
                            workSheet.DeleteColumn(i + 1);
                        }
                    }


                    if (!String.IsNullOrEmpty(heading))
                    {
                        workSheet.Cells["A1"].Value = heading;
                        workSheet.Cells["A1"].Style.Font.Size = 20;

                        workSheet.InsertColumn(1, 1);
                        workSheet.InsertRow(1, 1);
                        workSheet.Column(1).Width = 5;
                    }

                }
                result = package.GetAsByteArray();
            }

            return result;
        }


    }
}  
