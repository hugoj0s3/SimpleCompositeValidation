using System;
using System.Collections.Generic;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation
{
	public interface ICompositeValidation<T> : IValidation<T>
	{
		bool HasSummaryMessage { get; }
		string SummaryMessage { get; }
		IReadOnlyCollection<IValidation> Validations { get; }
		CompositeValidation<T> Add<TMember>(IValidation<TMember> validation, Func<T, TMember> member);
		CompositeValidation<T> AddForEach<TMember>(IValidation<TMember> validation, Func<T, IEnumerable<TMember>> members);
		IValidation<T> Update(string groupName);
		IValidation<T> Update(T target, string groupName);
		IValidation<T> Update<TMember>(string groupName, TMember value);
	}
}