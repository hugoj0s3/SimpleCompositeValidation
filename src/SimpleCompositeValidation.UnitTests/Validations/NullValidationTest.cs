using System.Linq;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class NullValidationTest
    {
        [Fact]
        public void Update_PassingNullObject_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            const string groupName = "NullObject";
            const string message = "TestError";
            const int severtiy = 999;
            var validation = new NullValidation(groupName, new object(), message, severtiy, false);

            // Act
            var result = validation.Update(null);

            // Assert
            result.ShouldBe(validation);
            result.IsValid.ShouldBeFalse();
            result.Failures.Count.ShouldBe(1);
            result.Failures.Single().Message.ShouldBe(message);
            result.Failures.Single().GroupName.ShouldBe(groupName);
            result.Failures.Single().Severity.ShouldBe(severtiy);
        }

        [Fact]
        public void Update_PassingNewObject_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            const string groupName = "Object";
            var message = $"{groupName} must be null";
            const int severtiy = 1;
            var validation = new NullValidation(groupName, null, message, severtiy, true);

            // Act
            var result = validation.Update(new object());

            // Assert 
            result.IsValid.ShouldBeFalse();
            result.Failures.Count.ShouldBe(1);
            result.Failures.Single().Message.ShouldBe(message);
            result.Failures.Single().GroupName.ShouldBe(groupName);
            result.Failures.Single().Severity.ShouldBe(severtiy);
        }
    }
}
