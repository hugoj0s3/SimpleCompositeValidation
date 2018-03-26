using System;
using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
    public class MustValidation<T> : Validation<T>  
    {
        public Func<T, bool> Rule { get; }
        public MustValidation(
            string groupName, 
            T target, 
            Func<T, bool> rule, 
            string message = null, 
            int severity = 1) 
            : base(groupName, null, target, severity)
        {
            if (message == null)
            {
                message = $"{groupName} is not valid";
            }
            Rule = rule;
            Message = message;
        }

        public MustValidation(
            string groupName, 
            Func<T, bool> rule, 
            string message = null, 
            int severity = 1) 
            : this(groupName, default(T), rule, message, severity)
        {
           
        }

        protected override IList<Failure> Validate()
        {
            var failures = new List<Failure>();

            if (Target != null && !Rule.Invoke(Target))
            {
                failures.Add(new Failure(this));
            }

            return failures;
        }
    }
}
