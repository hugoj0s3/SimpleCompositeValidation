using System;

namespace SimpleCompositeValidation.Validations
{
	/// <summary>
	/// Validates a condition that must not be true to keep the validation valid.
	/// </summary>
	/// <typeparam name="T">Type of the Target that will be validated</typeparam>
	public class MustNotValidation<T> : MustValidation<T> 
    {

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
