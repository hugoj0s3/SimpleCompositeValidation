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
			IEnumerable<T> target = default(IEnumerable<T>),
			string formatMessage = "{0} must have at least {1} items",
			int severity = 1) 
		    : base(groupName, target, formatMessage,  severity)
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
