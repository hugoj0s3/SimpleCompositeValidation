using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;

namespace SimpleCompositeValidation.UnitTests.Extensions
{
    public static class DateTimeExtensions
    {
	    public static void ShouldBeCloseTo(this DateTime thisDate, DateTime closeDate, int milliseconds)
	    {
			thisDate.ShouldBeLessThanOrEqualTo(closeDate.AddMilliseconds(milliseconds));
		    thisDate.ShouldBeGreaterThanOrEqualTo(closeDate.AddMilliseconds(-milliseconds));
	    }
    }
}
