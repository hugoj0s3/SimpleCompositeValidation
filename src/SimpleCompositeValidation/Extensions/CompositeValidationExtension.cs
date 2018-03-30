using System;
using SimpleCompositeValidation.Validations;

namespace SimpleCompositeValidation.Extensions
{
    public static class CompositeValidationExtension
    {
        /// <summary>
        /// Shortcut to add a NullValidation that not accept null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> NotNull<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member, 
            string message = null, 
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, false, message, severity), member);
            return thisValidation;
        }
        /// <summary>
        /// Shortcut to add a NullValidation that accept null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> Null<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, true, message, severity), member);
            return thisValidation;
        }

        /// <summary>
        /// Shotcut to add a MustValidation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="rule"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Short to add a MustValidation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="rule"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
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

        /// <summary>
        /// Shortcut to add a StringMinimumLengthValidation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="minimumLength"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
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

        /// <summary>
        /// Shortcut to add a StringMaximumLengthValidation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="maximumLength"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  Shortcut to add a RegExValition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="pattern"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> RegEx<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string pattern,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new RegExValidation(groupName, pattern, message, severity), member);
            return thisValidation;
        }

        /// <summary>
        ///  Shortcut to add a EmailValidation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> Email<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string message = null,
            int severity = 1)
        {
            thisValidation.Add(new EmailValidation(groupName, message, severity), member);
            return thisValidation;
        }
    }


}
