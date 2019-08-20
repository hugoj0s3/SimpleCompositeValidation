using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCompositeValidation.Validations.String.Urls
{
    public class HostUrlValidation : UrlValidation
    {
        private readonly string host = "www.facebook.com";

        public HostUrlValidation(string groupName, 
            string host,
            string formatMessage = "{0} - URL is not valid", 
            string target = null, 
            int severity = 1) : base(groupName, formatMessage, target, severity)
        {
            this.host = host;
        }

        protected override IList<Failure> Validate()
        {
            var failures = base.Validate();
            if (failures.Count <= 0 && base.Url.Host != host)
            {
                failures.Add(new Failure(this));
            }

            return failures;
        }
    }
}
