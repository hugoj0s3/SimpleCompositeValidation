using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompositeValidation.Validations
{
    public class NotEmptyEnumerableValidation<T> : MustValidation<IEnumerable<T>>
    {
	    private static readonly Func<IEnumerable<T>, bool> NotEmptyRule = x => x.Any();

	    public NotEmptyEnumerableValidation(string groupName) 
		    : base(groupName, NotEmptyRule, "{0} can not be empty")
		{
	    }

	    public NotEmptyEnumerableValidation(string groupName, IEnumerable<T> target) 
		    : base(groupName, NotEmptyRule, "{0} can not be empty", 1, target)
		{
	    }

	    public NotEmptyEnumerableValidation(
		    string groupName, 
		    string formatMessage = "{0} can not be empty", 
		    int severity = 1, 
		    IEnumerable<T> target = default(IEnumerable<T>)
		    ) : base(groupName, NotEmptyRule, formatMessage, severity, target)
	    {
	    }
    }
}
