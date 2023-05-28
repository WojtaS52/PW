using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane.Logi
{
    public static class Global
    {
        //katalogi tutaj 
        public static readonly string BaseDataDirPath =
        Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments
            ),
            "kuleczki_mr_ws",
            "folder_z_kolizjami");

        public static void DirIsValid(bool writePath = false)
        {
            if (!Directory.Exists(BaseDataDirPath))
            {
                Directory.CreateDirectory(BaseDataDirPath);
            }
            //try - catch do uprawnien zapisu
            try
            {
                using var fs = File.Create(
                    Path.Combine(BaseDataDirPath, Path.GetRandomFileName()),
                    1,
                    FileOptions.DeleteOnClose);
            }
            catch (Exception e)
            {
                throw new Exception("Bazowy dokument zapisu logów nie posiada uprawnien do zapisu!!! error!!!!", e);
            }

            if (writePath) Console.WriteLine($"Dane sciezka = {BaseDataDirPath}");
        }

        public static void UsunKatalog()
        {
            //tutaj dodac usuwanie katalogu
        }
    }
}
