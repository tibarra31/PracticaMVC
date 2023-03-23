using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.BL.Utils
{
    public class ExportarExcel
    {
        /// <summary>
        /// Genera un archivo de excel con un listado de datos usando la plantilla ListadoSimple.xls
        /// </summary>
        /// <param name="dataTable">DataTable con el conjunto de datos a exportar.</param>
        /// <param name="tituloExcel">Título que tendrá el reporte.</param>
        /// <param name="nombreArchivo">Nombre sugerido para el archivo resultante.</param>
        public static DBResponse<byte[]> GrabaArchivoExcelSimple(DataTable dataTable, string tituloExcel, string nombreArchivo, string rutaPlantilla)
        {
            DBResponse<byte[]> resultado = new DBResponse<byte[]>();
            string plantillaArchivo = "ListadoSimple.xls";
            string templateFile = rutaPlantilla + "\\" + plantillaArchivo;

            int constanteFIRSTROW = String.IsNullOrEmpty(tituloExcel) ? 1 : 2; // Primer renglón del listado (corresponde al encabezado)

            string reportLegend = string.Empty;

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            FileStream file = new FileStream(templateFile, FileMode.Open, FileAccess.Read);
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            file.Close();

            ISheet sheet1 = hssfworkbook.GetSheetAt(0);

            if (!String.IsNullOrEmpty(tituloExcel))
            {
                IFont fontTitulo = hssfworkbook.CreateFont();
                fontTitulo.Color = HSSFColor.Blue.Index;
                fontTitulo.Boldweight = (short)FontBoldWeight.Bold;
                fontTitulo.FontHeightInPoints = 20;
                ICellStyle estiloTitulo = hssfworkbook.CreateCellStyle();
                estiloTitulo.SetFont(fontTitulo);
                ICell celdaTitulo = sheet1.CreateRow(0).CreateCell(0);
                celdaTitulo.SetCellValue(tituloExcel);
                celdaTitulo.CellStyle = estiloTitulo;
            }

            IFont fontEncabezado = hssfworkbook.CreateFont();
            fontEncabezado.Color = HSSFColor.Blue.Index;
            fontEncabezado.Boldweight = (short)FontBoldWeight.Bold;
            ICellStyle estiloEncabezado = hssfworkbook.CreateCellStyle();
            estiloEncabezado.SetFont(fontEncabezado);
            IRow rowEncabezados = sheet1.CreateRow(constanteFIRSTROW - 1);

            int totalColumnas = dataTable.Columns.Count;

            for (int c = 0; c < dataTable.Columns.Count; c++)
            {
                string value = dataTable.Columns[c].ColumnName;
                ICell celda = rowEncabezados.CreateCell(c);
                celda.SetCellValue(value);
                celda.CellStyle = estiloEncabezado;
            }

            ICellStyle estiloRenglonNormal = hssfworkbook.CreateCellStyle();
            estiloRenglonNormal.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            estiloRenglonNormal.FillPattern = FillPattern.SolidForeground;
            ICellStyle estiloRenglonAlterno = hssfworkbook.CreateCellStyle();
            estiloRenglonAlterno.FillForegroundColor = HSSFColor.White.Index;
            estiloRenglonAlterno.FillPattern = FillPattern.SolidForeground;

            // Coloca los datos del data table
            bool esRenglonNormal = true;
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                IRow row = sheet1.CreateRow(constanteFIRSTROW + r);
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    string value;
                    ICell celda = row.CreateCell(c);

                    if (dataTable.Columns[c].DataType == Type.GetType("System.DateTime"))
                    {
                        if (dataTable.Rows[r][c].ToString() != "")
                            value = DateTime.Parse(dataTable.Rows[r][c].ToString()).ToString("dd/MM/yyyy");
                        else
                            value = "";
                    }
                    else if (dataTable.Columns[c].DataType == Type.GetType("System.Boolean"))
                        value = dataTable.Rows[r][c].ToString().Equals("True") ? "Sí" : "No";
                    else
                        value = dataTable.Rows[r][c].ToString();

                    celda.SetCellValue(value);

                    if (esRenglonNormal)
                    {
                        celda.CellStyle = estiloRenglonNormal;
                    }
                    else
                    {
                        celda.CellStyle = estiloRenglonAlterno;
                    }


                }
                esRenglonNormal = !esRenglonNormal;
            }

            //Ajusta columnas
            for (int c = 0; c <= totalColumnas; c++)
            {
                sheet1.AutoSizeColumn(c);
            }

            string filePath = rutaPlantilla + "temp" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream FWriteStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                hssfworkbook.Write(FWriteStream);
            }

            //write excel into memory stream
            using (var exportData = new MemoryStream())
            {
                using (FileStream FreadStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[FreadStream.Length];
                    FreadStream.Read(bytes, 0, (int)FreadStream.Length);

                    FreadStream.Seek(0, SeekOrigin.Begin);
                    exportData.Write(bytes, 0, (int)FreadStream.Length);
                }

                //Eliminamos el archivo de apoyo
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

                resultado.Data = exportData.GetBuffer();
            }

            return resultado;
        }   
    }
}
