using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
    /// <summary>
    ///  Validates a string requiring a maximum length.
    /// </summary>
    public class StringMaximumLengthValidation : Validation<string>
    {
        /// <summary>
        /// Maxixum length required.
        /// </summary>
        public int MaximumLength { get; }

        public StringMaximumLengthValidation(
            string groupName, 
            string target, 
            int maximumLength, 
            string message = null, 
            int severity = 1) 
            : base(groupName, null, target, severity)
        {
            if (message == null)
            {
                message = $"{groupName} the characters length limit is {maximumLength}";
            }
            MaximumLength = maximumLength;
            Message = message;
        }

        public StringMaximumLengthValidation(
            string groupName, 
            int maximumLength, 
            string message = null, 
            int severity = 1) 
            : this(groupName, null, maximumLength, message, severity)
        {
        }

	    /// <inheritdoc />
	    protected override IList<Failure> Validate()
        {
            var failures = new List<Failure>();
            if (Target.Length > MaximumLength)
            {
                failures.Add(new Failure(this));
            }
            return failures;
        }
    }
}
