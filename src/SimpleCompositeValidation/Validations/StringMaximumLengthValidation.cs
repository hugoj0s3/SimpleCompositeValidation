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
            int maximumLength)
            : this(groupName, maximumLength, null)
        {
        }

        public StringMaximumLengthValidation(
            string groupName,
            int maximumLength,
            string target)
            : this(groupName, maximumLength, null, 1, target)
        { 
        }

        public StringMaximumLengthValidation(
            string groupName, 
            int maximumLength, 
            string message = null, 
            int severity = 1,
            string target = null) 
            : base(groupName, message, target, severity)
        {
            if (message == null)
            {
                message = $"{groupName} the characters length limit is {maximumLength}";
            }
            MaximumLength = maximumLength;
            Message = message;
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
