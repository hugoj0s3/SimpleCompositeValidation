using System.Linq;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{

    public class MustNotValidationTest
    {
        [Fact]
        public void Update_GivenTrueStatement_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            const string message = "It must be true";
            const int severtiy = 5;
            const string groupName = "InvalidOperation";
            var mustNotValidation = new MustNotValidation<bool>(groupName, x => x, message, severtiy, true);

            // Act
            var result = mustNotValidation.Update();

            // Assert
            result.ShouldBe(mustNotValidation);
            result.IsValid.ShouldBeFalse();
            result.Failures.Count.ShouldBe(1);
            result.Failures.Single().Message.ShouldBe(message);
            result.Failures.Single().GroupName.ShouldBe(groupName);
            result.Failures.Single().Severity.ShouldBe(severtiy);
        }

        [Fact]
        public void Update_GivenFalseStatement_ItUpdateToValidValidation()
        {
            // Arrange
            const string groupName = "InvalidOperation";
            var mustNotValidation = new MustNotValidation<bool>(groupName, x => x, false);

            // Act
            var result = mustNotValidation.Update();

            // Assert
            mustNotValidation.ShouldBe(result);
            mustNotValidation.IsValid.ShouldBeTrue();
            mustNotValidation.Failures.ShouldBeEmpty();
        }
    }
}
