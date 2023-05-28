using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Dane.Logi
{
    public class LogWriter : InterfejsLogWriter
    {
        private readonly string _logPlikSciezka;

        public void Dispose()
        {
            Global.UsunKatalog();
        }

        public LogWriter(string nazwaPliku = "")
        {
            Global.DirIsValid();

            if(String.IsNullOrWhiteSpace(nazwaPliku)) nazwaPliku = $"kolizje({DateTime.Now:yyyy-MM-dd' 'HH-mm-ss}).log";
            //$"Kolizje({DateTime.Now:'D'yyyy-MM-dd'T'HH-mm-ss}).log"
            _logPlikSciezka = Path.Combine(Global.BaseDataDirPath, nazwaPliku);
        }

        public void Write(IEnumerable<LogAccess> logAccesses)
        {
            var lg = new StringBuilder();

            
            foreach(var logAccess in logAccesses)
            {
                lg.Append("| ").Append(logAccess.TimeStamp)
                    .Append(" | ").Append(logAccess.Setting)
                    .Append(" # ").Append(logAccess.LineNumber)
                    .Append("\t").Append(logAccess.Msg).AppendLine();
            }

            //tutaj try-catch do obslugi bledow wyjatkow i smoli wie co jeszcze,
            //hehe Aftertable knows too

            try
            {
                File.AppendAllText(_logPlikSciezka, lg.ToString());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw;
            }

        }

    }
}
