using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
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
            : this(groupName, minimumLength, null)
        {
        }

        public StringMinimumLengthValidation(
            string groupName,
            int minimumLength,
            string target)
            : this(groupName, minimumLength, null, 1,target)
        {
        }

        public StringMinimumLengthValidation(
            string groupName, 
            int minimumLength, 
            string message = null, 
            int severity = 1,
            string target = null) 
            : base(groupName, message, target, severity)
        {
            if (message == null)
            {
                message = $"{groupName} requires at least {minimumLength} characters";
            }
            MinimumLength = minimumLength;
            Message = message;
        }

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
