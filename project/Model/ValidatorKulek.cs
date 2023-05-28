using Logika;
using Model.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ValidatorKulek : InterfaceValidator<int>
    {
        private readonly int min;
        private readonly int max;

        public ValidatorKulek(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public ValidatorKulek() : this(int.MinValue) { }

        public ValidatorKulek(int min) : this(min, int.MaxValue) { }

        public bool IsValid(int val)
        {
            return val >= min && val <= max;
        }

        public bool IsInvalid(int val)
        {
            return !IsValid(val);
        }
    }
}
