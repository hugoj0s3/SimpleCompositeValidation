using System;

namespace SimpleCompositeValidation.Validations
{
	/// <summary>
	/// Validates a condition that must not be true to keep validation valid.
	/// </summary>
	/// <typeparam name="T">Type of the Target that will be validated</typeparam>
	public class MustNotValidation<T> : MustValidation<T> 
    {
		/// <summary>
		/// Creates a validation with given parameters.
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="message">Default message to be applied in the failures</param>
		/// <param name="target">Target to be validated</param>
		/// <param name="rule">Condition</param>
		/// <param name="severity">Severity in case of failure</param>
		public MustNotValidation(
            string groupName,
            T target,
            Func<T, bool> rule,
            string message = null,
            int severity = 1)
            : base(groupName, target, x => !rule.Invoke(x), message, severity)
        {
         
        }

		/// <summary>
		/// Creates a validation with given parameters. The target be initialized with the default value(default(T)).
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="message">Default message to be applied in the failures</param>
		/// <param name="rule">Condition</param>
		/// <param name="severity">Severity in case of failure</param>
		public MustNotValidation(
            string groupName,
            Func<T, bool> rule,
            string message = null,
            int severity = 1)
            : base(groupName, x => !rule.Invoke(x), message, severity)
        {

        }

    }
}
