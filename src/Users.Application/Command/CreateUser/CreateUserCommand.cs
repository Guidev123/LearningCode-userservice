using MediatR;
using System.ComponentModel.DataAnnotations;
using Users.Application.DTOs;
using Users.Application.Responses;
using Users.Domain.Entities;
using Users.Domain.ValueObjects;

namespace Users.Application.Command.CreateUser
{
    public class CreateUserCommand : IRequest<Response<GetUserDTO?>>
    {
        public CreateUserCommand(string fullName, string phone, string document,
                                     string email, string password, DateTime birthDate)
        {
            FullName = fullName;
            Phone = phone;
            Document = document;
            Email = email;
            Password = password;
            BirthDate = birthDate;
        }

        public string FullName { get; private set; }
        public string Phone { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime BirthDate { get; private set; }

        public User MapToEntity() => new(FullName,
                                         new Email(Email),
                                         Password,
                                         new Document(Document),
                                         Phone,
                                         BirthDate);
    }
}
