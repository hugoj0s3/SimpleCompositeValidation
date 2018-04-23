using System;

namespace SimpleCompositeValidation.Validations.String
{
    public class NotEmptyStringValidation : MustNotValidation<string>
    {
	    private static readonly Func<string, bool> NotEmptyRule = x => x == string.Empty;


	    public NotEmptyStringValidation(string groupName,
		    string target = default(string),
			string formatMessage = "{0} can not be empty", 
		    int severity = 1
		    ) 
		    : base(groupName, NotEmptyRule, target, formatMessage, severity)
	    {
	    }
    }
}
