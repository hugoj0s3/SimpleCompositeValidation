using System;
using System.Collections.Generic;
using System.Text;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations.String.Urls
{
    public class UrlValidation : Validation<string>
    {
        protected Uri Url { get; private set; }
        public UrlValidation(string groupName, string target = default(string), string formatMessage = "{0} - URL is not valid",  int severity = 1) 
            : base(groupName, target, formatMessage, severity)
        {
        }

        protected override IList<Failure> Validate()
        {
            var failures = new List<Failure>();
            if (!Uri.TryCreate(Target, UriKind.Absolute, out var result))
            {
                failures.Add(new Failure(this));
            }

            Url = result;

            return failures;
        }
    }
}
