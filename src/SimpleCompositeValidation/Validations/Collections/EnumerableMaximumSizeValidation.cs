using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation.Validations.Collections
{
	public class EnumerableMaximumSizeValidation<T> : Validation<IEnumerable<T>>
	{
		public int MaximumSize { get; }

		public EnumerableMaximumSizeValidation(
			string groupName, 
			int maximumSize,
			IEnumerable<T> target = default(IEnumerable<T>),
			string formatMessage = "{0} can not have more than {1} items",
			int severity = 1) 
			: base(groupName, target, formatMessage, severity)
		{
			MaximumSize = maximumSize;
		}

		protected override IList<Failure> Validate()
		{
			var failures = new List<Failure>();

			if (Target.Count() > MaximumSize)
			{
				failures.Add(new Failure(this));
			}

			return failures;
		}

		public override string Message => string.Format(FormatMessage, GroupName, MaximumSize);
	}
}
