using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace PracticaMVC.API.Utils
{
    public class FilesHelper
    {
        public static bool UplodDocument(MemoryStream stream, string folder, string name)
        {
            try
            {
                stream.Position = 0;
                string dirDocumentos = ConfigurationManager.AppSettings["DirectorioDocumentos"].ToString() + folder;
                var path = Path.Combine(dirDocumentos, name);
                if (!Directory.Exists(dirDocumentos))
                {
                    Directory.CreateDirectory(dirDocumentos);
                }
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}