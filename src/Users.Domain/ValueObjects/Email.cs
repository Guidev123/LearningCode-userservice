using System.Text.RegularExpressions;
using Users.Domain.Exceptions;

namespace Users.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        protected Email() { } //EF Relation
        public Email(string address)
        {
            if (string.IsNullOrEmpty(address) || address.Length < 5)
                throw new InvalidEmailException();

            Address = address.ToLower().Trim();
            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            if (!Regex.IsMatch(address, pattern))
                throw new InvalidEmailException();
        }

        public string Address { get; } = string.Empty;
    }
}
