using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations
{
	/// <summary>
	/// Validates if object can be null or not.
	/// </summary>
    public sealed class NullValidation: IValidation<object> 
    {
		/// <summary>
		/// Define if validation accept or not accept null.
		/// </summary>
        public bool AcceptNull { get; }

	    /// <inheritdoc />
	    public string Message { get; }

	    /// <inheritdoc />
	    public int Severity { get; }

	    /// <inheritdoc />
	    public string GroupName { get; }

	    /// <inheritdoc />
	    public IReadOnlyCollection<Failure> Failures { get; private set; }

	    /// <inheritdoc />
	    public bool IsValid => !Failures.Any();

	    /// <inheritdoc />
	    public object Target { get; private set; }

		/// <summary>
		/// Creates a null validation.
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="message">Default message to be applied in the failures</param>
		/// <param name="target">Target to be validated. by default it is null</param>
		/// <param name="acceptNull">Define if validation accept or not accept null.</param>
		/// <param name="severity">Severity in case of failure</param>
		public NullValidation(
            string groupName,
            object target = null, 
            bool acceptNull = false, 
            string message = null, 
            int severity = 1
            ) 
        {
            if (message == null)
            {
                var nullText = acceptNull ? string.Empty : "not ";
                message = $"{groupName} must {nullText}be null";
            }

            GroupName = groupName;
            Target = target;
            AcceptNull = acceptNull;
            Message = message;
            Severity = severity;
            Failures = new List<Failure>().AsReadOnly();
        }
        
        public IValidation<object> Update()
        {
            var failures = Validate();
            Failures = new ReadOnlyCollection<Failure>(failures);
            return this;
        }

        public IValidation<object> Update(object target)
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
