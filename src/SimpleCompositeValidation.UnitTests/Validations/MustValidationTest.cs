using System.Linq;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class MustValidationTest
    {
        [Fact]
        public void Update_GivenFalseStatement_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            const string message = "It must be true";
            const int severtiy = 5;
            const string groupName = "InvalidOperation";
            var mustValidation = new MustValidation<bool>(groupName, false, x => x, message, severtiy);

            // Act
            var result = mustValidation.Update();

            // Assert
            mustValidation.ShouldBe(result);
            result.IsValid.ShouldBeFalse();
            result.Failures.Count.ShouldBe(1);
            result.Failures.Single().Message.ShouldBe(message);
            result.Failures.Single().GroupName.ShouldBe(groupName);
            result.Failures.Single().Severity.ShouldBe(severtiy);
        }

        [Fact]
        public void Update_GivenTrueStatement_ItUpdateToValidValidation()
        {
            // Arrange
            const string groupName = "InvalidOperation";
            var mustValidation = new MustValidation<bool>(groupName, true, x => x);

            // Act
            var result = mustValidation.Update();

            // Assert
            mustValidation.ShouldBe(result);
            mustValidation.IsValid.ShouldBeTrue();
            mustValidation.Failures.ShouldBeEmpty();
        }
    }
}
