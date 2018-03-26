using System;

namespace SimpleCompositeValidation.Validations
{
    public class MustNotValidation<T> : MustValidation<T> 
    {
        public MustNotValidation(
            string groupName,
            T target,
            Func<T, bool> rule,
            string message = null,
            int severity = 1)
            : base(groupName, target, x => !rule.Invoke(x), message, severity)
        {
         
        }

        public MustNotValidation(
            string groupName,
            Func<T, bool> rule,
            string message = null,
            int severity = 1)
            : base(groupName, x => !rule.Invoke(x), message, severity)
        {

        }

    }
}
