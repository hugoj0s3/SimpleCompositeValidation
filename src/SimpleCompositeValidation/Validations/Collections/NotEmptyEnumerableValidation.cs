using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCompositeValidation.Validations.Collections
{
    public class NotEmptyEnumerableValidation<T> : MustValidation<IEnumerable<T>>
    {
	    private static readonly Func<IEnumerable<T>, bool> NotEmptyRule = x => x.Any();

	    public NotEmptyEnumerableValidation(
		    string groupName,
		    IEnumerable<T> target = default(IEnumerable<T>),
			string formatMessage = "{0} can not be empty", 
		    int severity = 1
		    ) : base(groupName, NotEmptyRule, target, formatMessage, severity)
	    {
	    }
    }
}
