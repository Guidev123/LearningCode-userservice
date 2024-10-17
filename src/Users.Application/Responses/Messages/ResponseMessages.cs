using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Responses.Messages
{
    public enum ResponseMessages
    {
        [Description("Success: User has been created successfully.")]
        USER_CREATED_SUCCESS,
        [Description("Error: Unable to create user.")]
        USER_CREATION_FAILED,
        [Description("Error: User already exists.")]
        USER_ALREADY_EXISTS,
        [Description("Success: User has been deleted successfully.")]
        USER_DELETED_SUCCESS,
        [Description("Error: Unable to delete user.")]
        USER_DELETION_FAILED,
        [Description("Error: User not found.")]
        USER_NOT_FOUND,
        [Description("Success: Login successful.")]
        LOGIN_SUCCESS,
        [Description("Error: Invalid credentials.")]
        INVALID_CREDENTIALS,
        [Description("Error: Account is locked.")]
        ACCOUNT_LOCKED,
        [Description("Error: Login failed.")]
        LOGIN_FAILED,
        [Description("Success: User data retrieved successfully.")]
        USER_RETRIEVED_SUCCESS,
        [Description("Error: Unable to retrieve user data.")]
        USER_RETRIEVAL_FAILED
    }
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null)
                    return attribute.Description;
            }

            return value.ToString();
        }
    }
}
