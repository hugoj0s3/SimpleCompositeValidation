using System;
using System.Collections.Generic;

namespace SimpleCompositeValidation.Base
{
	/// <summary>
	/// Performs all validations added.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICompositeValidation<T> : IValidation<T>
	{
		/// <summary>
		/// Returns true if it has summary message. 
		/// </summary>
		bool HasSummaryMessage { get; }
		/// <summary>
		/// Summary message this text is added in the top of failure list in case of falure.
		/// </summary>
		string SummaryMessage { get; }
		/// <summary>
		/// List of validations added
		/// </summary>
		IReadOnlyCollection<IValidation> Validations { get; }
		/// <summary>
		/// Add Validation
		/// </summary>
		/// <typeparam name="TMember">Type of member</typeparam>
		/// <param name="validation">Validation</param>
		/// <param name="member">Member that will be passed as parameter to peform this validation</param>
		/// <returns></returns>
		ICompositeValidation<T> Add<TMember>(IValidation<TMember> validation, Func<T, TMember> member);

		/// <summary>
		///  Add Validation for each member of collection
		/// </summary>
		/// <typeparam name="TMember">Type of member</typeparam>
		/// <param name="validation">Validation</param>
		/// <param name="members">Members</param>
		/// <returns></returns>
		ICompositeValidation<T> AddForEach<TMember>(IValidation<TMember> validation, Func<T, IEnumerable<TMember>> members);
		/// <summary>
		/// Update the list of failure partially according with the group name.
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		ICompositeValidation<T> Update(string groupName);

		/// <summary>
		/// Change the target and update the list of failure partially according with the group name.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="groupName"></param>
		/// <returns></returns>
		ICompositeValidation<T> Update(T target, string groupName);

		/// <summary>
		/// Update the list of failure partially according with the group name.
		/// </summary>
		/// <typeparam name="TMember"></typeparam>
		/// <param name="groupName"></param>
		/// <param name="value">value that will passed as parameter</param>
		/// <returns></returns>
		ICompositeValidation<T> Update<TMember>(string groupName, TMember value);
	}
}