using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleCompositeValidation.Base
{
    public abstract class Validation<T> : IValidation<T> 
    {
        public T Target { get; protected set; }

        public int Severity { get; protected set; }
        public string GroupName { get; protected set; }
        public string Message { get; protected set; }

        protected Validation(
            string groupName,
            string message,
            T target,  int severity = 1)
        {
            GroupName = groupName;
            Target = target;
            Message = message;
            Severity = severity;
            Failures = new List<Failure>().AsReadOnly();
        }

        protected Validation(
            string groupName, string message, int severity = 1)
            : this(groupName, message, default(T), severity)
        {

        }

        protected abstract IList<Failure> Validate();

        public IValidation<T> Update()
        {
            if (Target == null)
            {
                return this;
            }

            var failures = Validate();
            Failures = new ReadOnlyCollection<Failure>(failures);
            return this;
        }

        public IValidation<T> Update(T target)
        {
            Target = target;
            return Update();
        }

        public IReadOnlyCollection<Failure> Failures { get; protected set; } 

        public bool IsValid => !Failures.Any();

      
    }
}
