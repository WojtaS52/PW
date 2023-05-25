using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane.Logi
{


    public struct LogAccess
    {
        public readonly string TimeStamp;
        public readonly int LineNumber;
        public readonly string Msg;

        public readonly LogSetting Setting;

        public LogAccess(string timeStamp, int lineNumber, string message, LogSetting setting)
        {
            TimeStamp = DateTime.Now.ToString("dd-MM-yyyy - HH:mm:ss:fff");     //fff - milisekundy(1/1000, bo w sumie nie wiem czy to to samo) w takim programies twierdzilismy ze chyba przyda sie do logów bo raczej sekunda to za bardzo ogolny czas ale nie wiem jestem studentem ; dobra skoncz ten referat
            LineNumber = lineNumber;
            Msg = message;
            Setting = setting;
        }
    }
}
