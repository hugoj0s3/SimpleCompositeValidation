using System;
using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
	/// <summary>
	/// Validates a condition that must be true to keep validation valid.
	/// </summary>
	/// <typeparam name="T">Type of the Target that will be validated</typeparam>
	public class MustValidation<T> : Validation<T>  
    {
        public Func<T, bool> Rule { get; }

	    /// <summary>
	    /// Creates a validation with given parameters.
	    /// </summary>
	    /// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
	    /// <param name="rule">Condition</param>
	    public MustValidation(
		    string groupName,
		    Func<T, bool> rule)
		    : this(groupName, rule, default(T))
	    {

	    }

		/// <summary>
		/// Creates a validation with given parameters.
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="target">Target to be validated</param>
		/// <param name="rule">Condition</param>
		public MustValidation(
            string groupName,
            Func<T, bool> rule,
            T target)
            : this(groupName, rule, null, 1, target)
        {
           
        }

        /// <summary>
        /// Creates a validation with given parameters.
        /// </summary>
        /// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
        /// <param name="message">Default message to be applied in the failures</param>
        /// <param name="target">Target to be validated</param>
        /// <param name="rule">Condition</param>
        /// <param name="severity">Severity in case of failure</param>
        public MustValidation(
            string groupName,
            Func<T, bool> rule, 
            string message = null, 
            int severity = 1,
	        T target = default(T)) 
            : base(groupName, null, target, severity)
        {
            if (message == null)
            {
                message = $"{groupName} is not valid";
            }
            Rule = rule;
            Message = message;
        }

        protected override IList<Failure> Validate()
        {
            var failures = new List<Failure>();

            if (Target != null && !Rule.Invoke(Target))
            {
                failures.Add(new Failure(this));
            }

            return failures;
        }
    }
}
