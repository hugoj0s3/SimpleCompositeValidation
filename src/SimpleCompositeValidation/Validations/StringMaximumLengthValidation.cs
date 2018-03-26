using System;
using System.Collections.Generic;
using System.Text;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
    public class StringMaximumLengthValidation : Validation<string>
    {
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
                message = $"{groupName} the characters length limit is {MaximumLength}";
            }
            MaximumLength = maximumLength;
            Message = message;
        }

        public StringMaximumLengthValidation(
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
            if (Target.Length > MaximumLength)
            {
                failures.Add(new Failure(this));
            }
            return failures;
        }
    }
}
