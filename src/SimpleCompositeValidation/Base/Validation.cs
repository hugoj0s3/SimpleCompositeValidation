using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleCompositeValidation.Base
{
	/// <summary>
	/// Base abstract class for validations
	/// </summary>
	/// <typeparam name="T">Type of the Target that will be validated</typeparam>
    public abstract class Validation<T> : IValidation<T> 
    {
	    /// <inheritdoc />
		public T Target { get; protected set; }

	    /// <inheritdoc />
	    public int Severity { get; protected set; }

	    /// <inheritdoc />
	    public string GroupName { get; protected set; }

	    /// <inheritdoc />
	    public string Message { get; protected set; }

	    /// <inheritdoc />
	    public IReadOnlyCollection<Failure> Failures { get; protected set; }

	    /// <inheritdoc />
	    public bool IsValid => !Failures.Any();

		/// <summary>
		/// Creates a validation with given parameters.
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="message">Default message to be applied in the failures</param>
		/// <param name="target">Target to be validated</param>
		/// <param name="severity">Severity in case of failure</param>
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

		/// <summary>
		/// Creates a validation with given parameters. The target be initialized with the default value(default(T)).
		/// </summary>
		/// <param name="groupName">Group name to group your validations, it can be a property name for example</param>
		/// <param name="message">Default message to be applied in the failures</param>
		/// <param name="severity">Severity in case of failure</param>
		protected Validation(
            string groupName, string message, int severity = 1)
            : this(groupName, message, default(T), severity)
        {

        }

		/// <summary>
		/// Validates the target and return the list of failures
		/// </summary>
		/// <returns>a list of failures, it might be empty if there are no fails</returns>
		protected abstract IList<Failure> Validate();

	    /// <inheritdoc />
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

	    /// <inheritdoc />
	    public IValidation<T> Update(T target)
        {
            Target = target;
            return Update();
        }
      
    }
}
