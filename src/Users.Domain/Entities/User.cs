using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Enums;
using Users.Domain.ValueObjects;

namespace Users.Domain.Entities
{
    public class User
    {
        public User(string fullName, Email email, string password,
                    Document document, string phone, DateTime birthDate)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Email = email;
            Password = password;
            Document = document;
            Phone = phone;
            BirthDate = birthDate;
            UserType = ECustomerType.Customer;
            LastLogin = DateTime.Now;
            IsDeleted = false;
        }
        protected User() { } // EF Relation

        public Guid Id { get; }
        public string FullName { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public Email Email { get; private set; } = null!;
        public string Password { get; private set; } = string.Empty;
        public Document Document { get; private set; } = null!;
        public ECustomerType UserType { get; private set; }
        public DateTime BirthDate { get; private set; }
        public DateTime LastLogin {  get; private set; }    
        public bool IsDeleted { get; private set; }
        public void UpdateLastLoginDate() => LastLogin = DateTime.Now;
        public void SetEntityAsDeleted() => IsDeleted = true;
        public void CryptographyPassword(string password) => Password = password;
    }
}
