using System;

namespace SimpleCompositeValidation.Validations.String
{
    public class NotEmptyStringValidation : MustNotValidation<string>
    {
	    private static readonly Func<string, bool> NotEmptyRule = x => x == string.Empty;

		public NotEmptyStringValidation(string groupName) 
			: base(groupName, NotEmptyRule, "{0} can not be empty", 1, null)
		{
	    }

	    public NotEmptyStringValidation(string groupName,  string target) :
			base(groupName, NotEmptyRule, "{0} can not be empty", 1, target)
		{
	    }

	    public NotEmptyStringValidation(string groupName,  
		    string formatMessage = "{0} can not be empty", 
		    int severity = 1, 
		    string target = default(string)) 
		    : base(groupName, NotEmptyRule, formatMessage, severity, target)
	    {
			
	    }
    }
}
