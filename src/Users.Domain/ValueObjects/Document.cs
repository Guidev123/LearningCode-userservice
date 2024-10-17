using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.ValueObjects
{
    public class Document : ValueObject
    {
        public string Number { get; private set; } = string.Empty;

        public Document(string number)
        {
            Number = number;
        }
        protected Document() { } // EF Relation

        public bool Validate()
        {
            if (Number.Length > 11)
                return false;

            while (Number.Length != 11)
                Number = '0' + Number;

            var equal = true;
            for (var i = 1; i < 11 && equal; i++)
                if (Number[i] != Number[0])
                    equal = false;

            if (equal || Number == "12345678909")
                return false;

            var numbers = new int[11];

            for (var i = 0; i < 11; i++)
                numbers[i] = int.Parse(Number[i].ToString());

            var sum = 0;
            for (var i = 0; i < 9; i++)
                sum += (10 - i) * numbers[i];

            var result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                    return false;
            }
            else if (numbers[9] != 11 - result)
                return false;

            sum = 0;
            for (var i = 0; i < 10; i++)
                sum += (11 - i) * numbers[i];

            result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                    return false;
            }
            else if (numbers[10] != 11 - result)
                return false;

            return true;
        }
    }
}
