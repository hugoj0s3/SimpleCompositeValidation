using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using SimpleCompositeValidation.Validations.Collections;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations.Collections
{
	public class EnumerableMaximumSizeValidationTest
	{
		[Fact]
		public void Update_AddingListThatDoesNotMeetTheMaximumSizeRequirements_ItUpdateItSelfToAnValidValidation()
		{
			// Arrange
			var defaultMessage = "Numbers can not have more than 4 items";
			var numbers = new List<int>() { 1, 2, 3, 4, 5 };
			var validation = new EnumerableMaximumSizeValidation<int>("Numbers", 4, numbers);

			// Act
			var result = validation.Update();

			// Assert
			validation.ShouldBe(result);
			result.IsValid.ShouldBeFalse();
			result.Failures.Count.ShouldBe(1);
			result.Failures.Single().Message.ShouldBe(defaultMessage);
			result.Failures.Single().GroupName.ShouldBe("Numbers");
			result.Failures.Single().Severity.ShouldBe(1);
		}

		[Fact]
		public void Update_AddingListThatMeetTheMaximumSizeRequirements_ItUpdateItSelfToValidValidation()
		{
			// Arrange
			var numbers = new List<int>() { 1, 2, 3, 4, 5 };
			var validation = new EnumerableMaximumSizeValidation<int>("Numbers", 5);

			// Act
			var result = validation.Update(numbers);

			// Assert
			validation.ShouldBe(result);
			result.IsValid.ShouldBeTrue();
			result.Failures.ShouldBeEmpty();
		}
	}
}
