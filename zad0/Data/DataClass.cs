using System;

namespace Data
{
    public class DataClass
    {
        private int counter = 0;

        public DataClass(int counter)
        {
            this.counter = counter;
        }

        public int Counter { get => counter; set => counter = value; }
    }
}
