using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
    public sealed class NullValidation<T> : IValidation<T> 
    {

        public bool AcceptNull { get; }
        public string Message { get; }
        public int Severity { get; }
        public string GroupName { get; }
        public IReadOnlyCollection<Failure> Failures { get; private set; }
        public bool IsValid => !Failures.Any();
        public T Target { get; private set; }

        public NullValidation(
            string groupName, 
            T target, 
            bool acceptNull = false, 
            string message = null, 
            int severity = 1
            ) 
        {
            if (message == null)
            {
                var nullText = acceptNull ? string.Empty : "not";
                message = $"{groupName} must {nullText} be null";
            }

            GroupName = groupName;
            Target = target;
            AcceptNull = acceptNull;
            Message = message;
            Severity = severity;
            Failures = new List<Failure>().AsReadOnly();
        }

        public NullValidation(
            string groupName, 
            bool acceptNull = false, 
            string message = null, 
            int severity = 1
            ) 
            : this(groupName, default(T), acceptNull, message, severity)
        {
            
        }
        
        public IValidation<T> Update()
        {
            var failures = Validate();
            Failures = new ReadOnlyCollection<Failure>(failures);
            return this;
        }

        public IValidation<T> Update(T target)
        {
            Target = target;
            return Update();
        }

        private IList<Failure> Validate()
        {
            var failures = new List<Failure>();
            if ((!AcceptNull && Target == null) || (AcceptNull && Target != null))
            {
                failures.Add(new Failure(this));
            }

            return failures;
        }
    }
}
