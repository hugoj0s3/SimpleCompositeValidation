using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations.Collections
{
    public class EnumerableMinimumSizeValidation<T> : Validation<IEnumerable<T>>
    {
	    public int MinimumSize { get; }

	    public EnumerableMinimumSizeValidation(
		    string groupName,
			int minimumSize,
			IEnumerable<T> target,
			string formatMessage = "{0} must have at least {1} items",
			int severity = 1) 
		    : base(groupName, formatMessage, target, severity)
	    {
		    MinimumSize = minimumSize;
	    }

	    public EnumerableMinimumSizeValidation(
		    string groupName, 
		    int minimumSize,
		    string formatMessage = "{0} must have at least {1} items",
			int severity = 1) 
		    : base(groupName, formatMessage, severity)
	    {
		    MinimumSize = minimumSize;
	    }

	    protected override IList<Failure> Validate()
	    {
		    var failures = new List<Failure>();

		    if (Target.Count() < MinimumSize)
		    {
			    failures.Add(new Failure(this));
		    }

		    return failures;
	    }

	    public override string Message => string.Format(FormatMessage, GroupName, MinimumSize);

	}
}
