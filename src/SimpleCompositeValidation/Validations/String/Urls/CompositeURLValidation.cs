using SimpleCompositeValidation.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCompositeValidation.Validations.String.Urls
{
    public static class CompositeURLValidationExtension
    {
        public static ICompositeValidation<T> Url<T>(this ICompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string formatMessage = "{0} URL is not valid",
            int severity = 1)
        {
            thisValidation.Add(new UrlValidation(groupName, null, formatMessage, severity), member);
            return thisValidation;
        }


        public static ICompositeValidation<T> HostUrl<T>(this ICompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string host,
            string formatMessage = "{0} URL is not valid",
            int severity = 1)
        {
            thisValidation.Add(new HostUrlValidation(groupName, host, null, formatMessage, severity), member);
            return thisValidation;
        }

        public static ICompositeValidation<T> HostUrl<T>(this ICompositeValidation<T> thisValidation,
            string groupName,
            Func<T, string> member,
            string[] hosts,
            string formatMessage = "{0} URL is not valid",
            int severity = 1)
        {
            thisValidation.Add(new HostUrlValidation(groupName, hosts, null, formatMessage, severity), member);
            return thisValidation;
        }

    }
}
