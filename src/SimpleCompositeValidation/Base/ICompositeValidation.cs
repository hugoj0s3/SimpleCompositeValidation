using System;
using System.Collections.Generic;

namespace SimpleCompositeValidation.Base
{
	public interface ICompositeValidation<T> : IValidation<T>
	{
		bool HasSummaryMessage { get; }
		string SummaryMessage { get; }
		IReadOnlyCollection<IValidation> Validations { get; }
		ICompositeValidation<T> Add<TMember>(IValidation<TMember> validation, Func<T, TMember> member);
		ICompositeValidation<T> AddForEach<TMember>(IValidation<TMember> validation, Func<T, IEnumerable<TMember>> members);
		ICompositeValidation<T> Update(string groupName);
		ICompositeValidation<T> Update(T target, string groupName);
		ICompositeValidation<T> Update<TMember>(string groupName, TMember value);
	}
}