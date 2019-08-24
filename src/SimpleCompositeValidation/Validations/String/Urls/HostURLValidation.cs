using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompositeValidation.Validations.String.Urls
{
    public class HostUrlValidation : UrlValidation
    {
        private readonly string[] hosts;

        public HostUrlValidation(string groupName, 
            string[] hosts,
            string formatMessage = "{0} - URL is not valid", 
            string target = null, 
            int severity = 1) : base(groupName, formatMessage, target, severity)
        {
            this.hosts = hosts ?? throw new ArgumentNullException(nameof(hosts));
        }

        public HostUrlValidation(string groupName,
            string host,
            string formatMessage = "{0} - URL is not valid",
            string target = null,
            int severity = 1) : base(groupName, formatMessage, target, severity)
        {
            this.hosts = new string[]{host};
        }


        protected override IList<Failure> Validate()
        {
            var failures = base.Validate();
            if (failures.Count <= 0 && !hosts.Contains(base.Url.Host))
            {
                failures.Add(new Failure(this));
            }

            return failures;
        }
    }
}
