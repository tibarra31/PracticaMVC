using OfficeOpenXml;
using PracticaMVC.EN;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.BL
{
    public class Generics_BL
    {
        public DBResponse<byte[]> GrabaArchivoExcelSimple(DataTable dataTable, string tituloExcel, string nombreArchivo, string rutaPlantilla)
        {
            return Utils.ExportarExcel.GrabaArchivoExcelSimple(dataTable, tituloExcel, nombreArchivo, rutaPlantilla);
        }

        public DBResponse<Boolean> CargaMasivaUsuarios(string pathArchivo)
        {
            var dbResponse = new DBResponse<Boolean>();
            var listado = new List<Usuarios>();
            listado = getDataUsuarios(pathArchivo);
            if (listado.Count > 0)
            {
                foreach (var item in listado)
                {
                    var insertUsuario = new Usuarios_BL().UpsertUsuario(item);
                }
            }

            return dbResponse;
        }

        public List<Usuarios> getDataUsuarios(string pathArchivo)
        {
            var listado = new List<Usuarios>();
            FileInfo fileInfo = new FileInfo(pathArchivo);
            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet.Dimension == null)
            {
                return listado;
            }
            int rows = worksheet.Dimension.Rows;
            for (int i = 1; i <= rows; i++)
            {
                Usuarios item = new Usuarios();
                if (worksheet.Cells[i, 1].Value == null)
                {
                    break;
                }

                item.IdUsuario = 0;
                item.Activo = true;
                item.IdEstatusRegistro = 1;
                item.IdPerfil = 2;
                item.IdPerfilRol = 5;
                item.Usuario = worksheet.Cells[i, 1].Value.ToString();
                item.Password = worksheet.Cells[i, 2].Value.ToString();

                listado.Add(item);
            }
            return listado;
        }
    }
}
