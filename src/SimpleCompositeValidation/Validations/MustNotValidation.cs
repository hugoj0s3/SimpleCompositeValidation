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
        /// <param name="formatMessageult formatMessage to be applied in the failures</param>
        /// <param name="target">Target to be validated</param>
        /// <param name="rule">Condition</param>
        /// <param name="severity">Severity in case of failure</param>
        public MustNotValidation(
            string groupName,
            Func<T, bool> rule,
            T target = default(T),
			string formatMessage = "{0} is not valid",
            int severity = 1
            )
            : base(groupName, x => !rule.Invoke(x), target, formatMessage, severity)
        {

        }

    }
}
