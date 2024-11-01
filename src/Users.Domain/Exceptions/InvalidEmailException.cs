﻿namespace Users.Domain.Exceptions
{
    [Serializable]
    internal class InvalidEmailException : Exception
    {
        public InvalidEmailException()
        {
        }

        public InvalidEmailException(string? message) : base(message)
        {
        }

        public InvalidEmailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}