using System;
using SimpleCompositeValidation.Validations;

namespace SimpleCompositeValidation.Extensions
{
    public static class CompositeValidationExtension
    {
        public static CompositeValidation<T> NotNull<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member, 
            string message = null, 
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, false, message, severity), member);
            return thisValidation;
        }

        public static CompositeValidation<T> Null<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, true, message, severity), member);
            return thisValidation;
        }

        public static CompositeValidation<T> Must<T, TMember>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, TMember> member,
            Func<TMember, bool> rule,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new MustValidation<TMember>(groupName, rule), member);
            return thisValidation;
        }

        public static CompositeValidation<T> MustNot<T, TMember>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, TMember> member,
            Func<TMember, bool> rule,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new MustNotValidation<TMember>(groupName, rule), member);
            return thisValidation;
        }

        public static CompositeValidation<T> MinimumLength<T>(this CompositeValidation<T> thisValidation, 
            string groupName,
            Func<T, string> member,
            int minimumLength, 
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new StringMinimumLengthValidation(groupName, minimumLength, message, severity), member);
            return thisValidation;
        }

        public static CompositeValidation<T> MaximumLength<T>(this CompositeValidation<T> thisValidation, 
            string groupName,
            Func<T, string> member,
            int maximumLength, 
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new StringMaximumLengthValidation(groupName, maximumLength, message, severity), member);
            return thisValidation;
        }

        public static CompositeValidation<T> RegEx<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string pattern,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new RegExValidation(groupName, pattern, null, message, severity), member);
            return thisValidation;
        }

        public static CompositeValidation<T> Email<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new EmailValidation(groupName, null, message, severity), member);
            return thisValidation;
        }
    }


}
