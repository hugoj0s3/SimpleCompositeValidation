using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations.String
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
            int maximumLength,
            string target = default(string),
			string formatMessage = "{0} the characters length limit is {1}", 
            int severity = 1
            ) 
            : base(groupName, target, formatMessage, severity)
        {
            MaximumLength = maximumLength;
        }

	    public override string Message => string.Format(FormatMessage, GroupName, MaximumLength);

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
