using System.Collections.Generic;
using System.Text.RegularExpressions;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
    public class RegularExpressionValidation : Validation<string>
    {
        public string Pattern { get; }

        public RegularExpressionValidation(
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
