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
	    public RegExValidation(
		    string groupName,
		    string pattern)
		    : this(groupName, pattern, null)
	    {
	    }

		/// <summary>
		/// Creates Regular expression validation
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="pattern">Regular expression</param>
		/// <param name="target">Target to be validated</param>
		public RegExValidation(
            string groupName,
            string pattern,
            string target)
            : this(groupName, pattern, "{0} is not valid", 1, target)
        {
        }

		/// <summary>
		/// Creates Regular expression validation
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="pattern">Regular expression</param>
		/// <param name="target">Target to be validated</param>
		/// <param name="formatMessage">format of message to be applied in the failures "{0} is the groupName"</param>
		/// <param name="severity">Severity in case of failure</param>
		public RegExValidation(
            string groupName, 
            string pattern,
            string formatMessage = "{0} is not valid",
            int severity = 1,
		    string target = null) 
            : base(groupName, formatMessage, target, severity)
        {
            Pattern = pattern;
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
