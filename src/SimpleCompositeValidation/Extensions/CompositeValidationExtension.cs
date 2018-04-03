using System;
using System.Collections.Generic;
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
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> NotNull<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member, 
            string formatMessage = "{0} must not be null", 
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, formatMessage, false, severity), member);
            return thisValidation;
        }
        /// <summary>
        /// Shortcut to add a NullValidation that accept null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> Null<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member,
            string formatMessage = "{0} must be null",
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, formatMessage, true,  severity), member);
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
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
        public static CompositeValidation<T> Must<T, TMember>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, TMember> member,
            Func<TMember, bool> rule,
            string formatMessage = "{0} is not valid",
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
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> MustNot<T, TMember>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, TMember> member,
            Func<TMember, bool> rule,
            string formatMessage = "{0} is not valid",
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
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> MinimumLength<T>(this CompositeValidation<T> thisValidation, 
            string groupName,
            Func<T, string> member,
            int minimumLength, 
            string formatMessage = "{0} requires at least {1} characters",
            int severity = 1)
        {
            thisValidation.Add(new StringMinimumLengthValidation(groupName, minimumLength, formatMessage, severity), member);
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
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
        public static CompositeValidation<T> MaximumLength<T>(this CompositeValidation<T> thisValidation, 
            string groupName,
            Func<T, string> member,
            int maximumLength, 
            string formatMessage = "{0} the characters length limit is {1}",

			int severity = 1)
        {
            thisValidation.Add(new StringMaximumLengthValidation(groupName, maximumLength, formatMessage, severity), member);
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
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> RegEx<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string pattern,
            string formatMessage = "{0} is not valid",
            int severity = 1)
        {
            thisValidation.Add(new RegExValidation(groupName, pattern, formatMessage, severity), member);
            return thisValidation;
        }

        /// <summary>
        ///  Shortcut to add a EmailValidation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValidation"></param>
        /// <param name="groupName"></param>
        /// <param name="member"></param>
        /// <param name="formatMessage"></param>
        /// <param name="severity"></param>
        /// <returns>Itself</returns>
        public static CompositeValidation<T> Email<T>(this CompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string formatMessage = "{0} Email is not valid",
            int severity = 1)
        {
            thisValidation.Add(new EmailValidation(groupName, formatMessage, severity), member);
            return thisValidation;
        }

		public static CompositeValidation<T> NotEmpty<T>(this CompositeValidation<T> thisValidation,
			string groupName,
			Func<T, string> member,
			string formatMessage = "{0} can not be empty",
			int severity = 1)
		{
			thisValidation.Add(new NotEmptyStringValidation(groupName, formatMessage, severity), member);
			return thisValidation;
		}

	    public static CompositeValidation<T> NotEmpty<T, TMember>(this CompositeValidation<T> thisValidation,
		    string groupName,
		    Func<T, IEnumerable<TMember>> member,
		    string formatMessage = "{0} can not be empty",
		    int severity = 1)
	    {
		    thisValidation.Add(new NotEmptyEnumerableValidation<TMember>(groupName, formatMessage, severity), member);
			return thisValidation;
	    }
	}


}
