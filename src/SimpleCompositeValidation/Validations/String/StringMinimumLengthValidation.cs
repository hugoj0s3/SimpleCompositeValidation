using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations.String
{
    /// <summary>
    ///  Validates a string requiring a minimum length.
    /// </summary>
    public class StringMinimumLengthValidation : Validation<string>
    {
        /// <summary>
        ///  Minimum length required
        /// </summary>
        public int MinimumLength { get; }

        public StringMinimumLengthValidation(
            string groupName,
            int minimumLength)
            : this(groupName, minimumLength, "{0} requires at least {1} characters")
        {
        }

        public StringMinimumLengthValidation(
            string groupName,
            int minimumLength,
            string target)
            : this(groupName, minimumLength, "{0} requires at least {1} characters", 1,target)
        {
        }

        public StringMinimumLengthValidation(
            string groupName, 
            int minimumLength, 
            string formatMessage = "{0} requires at least {1} characters", 
            int severity = 1,
            string target = null) 
            : base(groupName, formatMessage, target, severity)
        {
            MinimumLength = minimumLength;   
        }

	    public override string Message => string.Format(FormatMessage, GroupName, MinimumLength);

	    protected override IList<Failure> Validate()
        {
            var failures = new List<Failure>();
            if (Target.Length < MinimumLength)
            {
                failures.Add(new Failure(this));
            }
            return failures;
        }
    }
}
