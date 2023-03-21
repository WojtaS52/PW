using System;
using System.Xml.Serialization;
using Data;

namespace Logic
{
    public class LogicClass
    {
        DataClass dane = new DataClass(0);

        public int GetCounter()
        {
            return dane.Counter;
        }

        public string MessegeTransmitter()
        {
            dane.Counter += 1;
            //DateTime today = DateTime.Today;
            string messege = "Hello! World ! simple messege nr: " + dane.Counter + "\nObecna data: "+ DateTime.Now.ToString("M/d/yyyy"); ;//narazie nie pokazuje godziny
            return messege;
        }

        public void buttonHandle()  
        {

            string message = "Simple MessageBox";
            //MessageBox.Show(message);


        }

        public string buttonAuthors()
        {
            string messege = " Mateusz Rybicki 242518\n Wojciech Swiderski 242551\n";
            return messege;
        }

    }
}
