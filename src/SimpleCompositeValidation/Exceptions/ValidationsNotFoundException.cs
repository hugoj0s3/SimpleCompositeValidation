using System;

namespace SimpleCompositeValidation.Exceptions
{
	public class ValidationsNotFoundException : Exception
	{
		public ValidationsNotFoundException()
			: base("There aren't any validations for the group name passed")
		{

		}
	}
}