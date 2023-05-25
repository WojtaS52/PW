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
            "project-collisons");


    }
}
