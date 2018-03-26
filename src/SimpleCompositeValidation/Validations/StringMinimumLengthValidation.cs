using System;
using System.Collections.Generic;
using System.Text;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
    public class StringMinimumLengthValidation : Validation<string>
    {
        public int MinimumLength { get; }
        public StringMinimumLengthValidation(
            string groupName, 
            string target, 
            int minimumLength, 
            string message = null, 
            int severity = 1) 
            : base(groupName, null, target, severity)
        {
            if (message == null)
            {
                message = $"{groupName} requires at least {minimumLength} characters";
            }
            MinimumLength = minimumLength;
            Message = message;
        }

        public StringMinimumLengthValidation(
            string groupName, 
            int minimumLength, 
            string message = null, 
            int severity = 1) 
            : this(groupName, null, minimumLength, message, severity)
        {
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
