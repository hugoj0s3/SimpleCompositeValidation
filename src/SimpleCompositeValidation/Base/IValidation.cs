using System;
using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Base
{
	/// <summary>
	/// Base interface for validation
	/// </summary>
	public interface IValidation
	{
		/// <summary>
		/// Severity in case of failure
		/// </summary>
		int Severity { get; }

		/// <summary>
		/// Group name to group your validations, it can be a property name for example
		/// </summary>
		string GroupName { get; }

		/// <summary>
		/// Default formatMessage to be applied in the failures
		/// </summary>
		string Message { get; }

		/// <summary>
		/// List of failures
		/// </summary>
		IReadOnlyCollection<Failure> Failures { get; }

		/// <summary>
		/// True if the target is valid
		/// False if there are any failures.
		/// </summary>
		bool IsValid { get; }

		/// <summary>
		/// Last update performed. Typically It is initialized with DateTime.MinValue
		/// </summary>
		DateTime LastUpdate { get; }

	}

	/// <summary>
	/// Base interface for typed validation
	/// </summary>
	/// <typeparam name="T">Type of the Target that will be validated</typeparam>
	public interface IValidation<T> : IValidation
    {
		/// <summary>
		/// Target to be validated
		/// </summary>
        T Target { get; }

		/// <summary>
		/// Update failures list and IsValid property
		/// </summary>
		/// <returns>Itself</returns>
		IValidation<T> Update();

	    /// <summary>
	    /// Update failures list, IsValid property, Target property
	    /// </summary>
	    /// <returns>Itself</returns>
		IValidation<T> Update(T target);
    }
}