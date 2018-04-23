using System;
using System.Collections.Generic;
using SimpleCompositeValidation.Base;
using SimpleCompositeValidation.Validations;
using SimpleCompositeValidation.Validations.Collections;
using SimpleCompositeValidation.Validations.String;

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
        public static ICompositeValidation<T> NotNull<T>(this ICompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member, 
            string formatMessage = "{0} must not be null", 
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, default(object), formatMessage, severity, false), member);
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
        public static ICompositeValidation<T> Null<T>(this ICompositeValidation<T> thisValidation,
            string groupName,
            Func<T, object> member,
            string formatMessage = "{0} must be null",
            int severity = 1)
        {
            thisValidation.Add(new NullValidation(groupName, default(object), formatMessage, severity, true), member);
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
        public static ICompositeValidation<T> Must<T, TMember>(this ICompositeValidation<T> thisValidation,
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
        public static ICompositeValidation<T> MustNot<T, TMember>(this ICompositeValidation<T> thisValidation,
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
        public static ICompositeValidation<T> MinimumLength<T>(this ICompositeValidation<T> thisValidation, 
            string groupName,
            Func<T, string> member,
            int minimumLength, 
            string formatMessage = "{0} requires at least {1} characters",
            int severity = 1)
        {
            thisValidation.Add(new StringMinimumLengthValidation(groupName, minimumLength, default(string), formatMessage, severity), member);
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
        public static ICompositeValidation<T> MaximumLength<T>(this ICompositeValidation<T> thisValidation, 
            string groupName,
            Func<T, string> member,
            int maximumLength, 
            string formatMessage = "{0} the characters length limit is {1}",

			int severity = 1)
        {
            thisValidation.Add(new StringMaximumLengthValidation(groupName, maximumLength, default(string), formatMessage, severity), member);
            return thisValidation;
        }

		/// <summary>
		/// Shortcut to add EnumerableMinimumSizeValidation
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TMember"></typeparam>
		/// <param name="thisValidation"></param>
		/// <param name="groupName"></param>
		/// <param name="member"></param>
		/// <param name="minimumSize"></param>
		/// <param name="formatMessage"></param>
		/// <param name="severity"></param>
		/// <returns></returns>
		public static ICompositeValidation<T> MinimumSize<T, TMember>(this ICompositeValidation<T> thisValidation,
		    string groupName,
		    Func<T, IEnumerable<TMember>> member,
		    int minimumSize,
		    string formatMessage = "{0} must have at least {1} items",
		    int severity = 1)
	    {
		    thisValidation.Add(new EnumerableMinimumSizeValidation<TMember>(groupName, minimumSize, default(IEnumerable<TMember>), formatMessage, severity), member);
		    return thisValidation;
	    }

		/// <summary>
		/// Shortcut to add EnumerableMaximumSizeValidation
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TMember"></typeparam>
		/// <param name="thisValidation"></param>
		/// <param name="groupName"></param>
		/// <param name="member"></param>
		/// <param name="maximumSize"></param>
		/// <param name="formatMessage"></param>
		/// <param name="severity"></param>
		/// <returns></returns>
		public static ICompositeValidation<T> MaximumSize<T, TMember>(this ICompositeValidation<T> thisValidation,
		    string groupName,
		    Func<T, IEnumerable<TMember>> member,
		    int maximumSize,
		    string formatMessage = "{0} must have at least {1} items",
		    int severity = 1)
	    {
		    thisValidation.Add(new EnumerableMaximumSizeValidation<TMember>(groupName, maximumSize, default(IEnumerable<TMember>), formatMessage, severity), member);
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
		public static ICompositeValidation<T> RegEx<T>(this ICompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string pattern,
            string formatMessage = "{0} is not valid",
            int severity = 1)
        {
            thisValidation.Add(new RegExValidation(groupName, pattern, default(string), formatMessage, severity), member);
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
        public static ICompositeValidation<T> Email<T>(this ICompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string formatMessage = "{0} Email is not valid",
            int severity = 1)
        {
            thisValidation.Add(new EmailValidation(groupName, default(string), formatMessage, severity), member);
            return thisValidation;
        }

		/// <summary>
		/// Shortcut to add a NotEmptyStringValidation
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="thisValidation"></param>
		/// <param name="groupName"></param>
		/// <param name="member"></param>
		/// <param name="formatMessage"></param>
		/// <param name="severity"></param>
		/// <returns></returns>
		public static ICompositeValidation<T> NotEmpty<T>(this ICompositeValidation<T> thisValidation,
			string groupName,
			Func<T, string> member,
			string formatMessage = "{0} can not be empty",
			int severity = 1)
		{
			thisValidation.Add(new NotEmptyStringValidation(groupName, default(string), formatMessage, severity), member);
			return thisValidation;
		}

		/// <summary>
		/// Short to add a NotEmptyEnumerableValidation
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TMember"></typeparam>
		/// <param name="thisValidation"></param>
		/// <param name="groupName"></param>
		/// <param name="member"></param>
		/// <param name="formatMessage"></param>
		/// <param name="severity"></param>
		/// <returns></returns>
		public static ICompositeValidation<T> NotEmpty<T, TMember>(this ICompositeValidation<T> thisValidation,
		    string groupName,
		    Func<T, IEnumerable<TMember>> member,
		    string formatMessage = "{0} can not be empty",
		    int severity = 1)
	    {
		    thisValidation.Add(new NotEmptyEnumerableValidation<TMember>(groupName, default(IEnumerable<TMember>), formatMessage, severity), member);
			return thisValidation;
	    }
	}


}
