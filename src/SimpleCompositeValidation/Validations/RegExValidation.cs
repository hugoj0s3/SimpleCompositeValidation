using System.Collections.Generic;
using System.Text.RegularExpressions;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
	/// <summary>
	/// Validation according with regular expression
	/// </summary>
    public class RegExValidation : Validation<string>
    {
		/// <summary>
		/// Regular expression
		/// </summary>
        public string Pattern { get; }

		/// <summary>
		/// Creates Regular expression validation
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="pattern">Regular expression</param>
		/// <param name="target">Target to be validated</param>
		/// <param name="message">Default message to be applied in the failures</param>
		/// <param name="severity">Severity in case of failure</param>
		public RegExValidation(
            string groupName, 
            string pattern,
            string target = null,
            string message = null,
            int severity = 1) 
            : base(groupName, message, target, severity)
        {
            if (message == null)
            {
                message = $"{groupName} is not valid";
            }
            Pattern = pattern;
            Message = message;
        }

	    /// <inheritdoc />
	    protected override IList<Failure> Validate()
        {
            var failures = new List<Failure>();

            if (!Regex.IsMatch(Target, Pattern))
            {
                failures.Add(new Failure(this));
            }

            return failures;
        }
    }
}
