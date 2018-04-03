using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class NotEmptyStringValidationTest
    {
		[Fact]
	    public void Update_GivenEmptyString_ItUpdateItSelfToAnInvalidValidation()
	    {
		    // Arrange
		    const string message = "It must not be empty";
		    const int severtiy = 5;
		    const string groupName = "InvalidOperation";
		    var validation = new NotEmptyStringValidation(groupName, message, severtiy, string.Empty);

		    // Act
		    var result = validation.Update();

			// Assert
		    validation.ShouldBe(result);
		    result.IsValid.ShouldBeFalse();
		    result.Failures.Count.ShouldBe(1);
		    result.Failures.Single().Message.ShouldBe(message);
		    result.Failures.Single().GroupName.ShouldBe(groupName);
		    result.Failures.Single().Severity.ShouldBe(severtiy);
	    }

	    [Fact]
	    public void Update_GivenNotEmptyString_ItUpdateItSelfToValidValidation()
	    {
		    // Arrange
		    const string groupName = "InvalidOperation";
		    var validation = new NotEmptyStringValidation(groupName, "abc");

		    // Act
		    var result = validation.Update();

		    // Assert
		    validation.ShouldBe(result);
		    result.IsValid.ShouldBeTrue();
		    result.Failures.Count.ShouldBe(0);
	    }

	    [Fact]
	    public void Update_UpdatingWithEmptyString_ItUpdateItSelfToAnValidValidation()
	    {
			// Arrange
		    const string defaultMessage = "InvalidOperation can not be empty";
		    const int defaultSeverity = 1;
		    const string groupName = "InvalidOperation";
		    var validation = new NotEmptyStringValidation(groupName);

		    // Act
		    var result = validation.Update("");

		    // Assert
		    validation.ShouldBe(result);
		    result.IsValid.ShouldBeFalse();
		    result.Failures.Count.ShouldBe(1);
		    result.Failures.Single().Message.ShouldBe(defaultMessage);
		    result.Failures.Single().GroupName.ShouldBe(groupName);
		    result.Failures.Single().Severity.ShouldBe(defaultSeverity);
		}
	}
}
