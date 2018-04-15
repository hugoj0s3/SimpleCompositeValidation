using System.Collections.Generic;
using System.Linq;
using Shouldly;
using SimpleCompositeValidation.Validations.Collections;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations.Collections
{
    public class NotEmptyEnumerableTest
    {
		[Fact]
	    public void Update_GivenEmptyList_ItUpdateItSelfToAnInvalidValidation()
	    {
		    // Arrange
		    const string message = "It must not be empty";
		    const int severtiy = 5;
		    const string groupName = "InvalidOperation";
		    var validation = new NotEmptyEnumerableValidation<object>(groupName, message, severtiy, new List<object>());

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
	    public void Update_GivenNotEmptyList_ItUpdateItSelfToValidValidation()
	    {
		    // Arrange
		    const string groupName = "InvalidOperation";
		    var validation = new NotEmptyEnumerableValidation<object>(groupName, new List<object>(){1});

		    // Act
		    var result = validation.Update();

		    // Assert
		    validation.ShouldBe(result);
		    result.IsValid.ShouldBeTrue();
		    result.Failures.Count.ShouldBe(0);
	    }

	    [Fact]
	    public void Update_UpdatingWithEmptyList_ItUpdateItSelfToAnValidValidation()
	    {
			// Arrange
		    const string defaultMessage = "InvalidOperation can not be empty";
		    const int defaultSeverity = 1;
		    const string groupName = "InvalidOperation";
		    var validation = new NotEmptyEnumerableValidation<object>(groupName);

		    // Act
		    var result = validation.Update(new List<object>());

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
