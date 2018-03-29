using System.Linq;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class EmailValidationTest
    {
        [Fact]
        public void Update_ParsingInvalidEmail_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            var groupName = "Email";
            var target = "testHugo#gmail.com";
            var message = "TestError";
            var severity = 999;

            var validation =
                new EmailValidation(groupName, target, message, severity);

            // Act
            var result = validation.Update();

            // Assert
            validation.ShouldBe(result);
            result.IsValid.ShouldBeFalse();
            result.Failures.Count.ShouldBe(1);
            result.Failures.Single().Message.ShouldBe(message);
            result.Failures.Single().GroupName.ShouldBe(groupName);
            result.Failures.Single().Severity.ShouldBe(severity);
        }

        [Fact]
        public void Update_ParsingValidEmail_ItUpdateItSelfToAnValidValidation()
        {
            // Arrange
            var groupName = "Email";
            var target = "testhugo@gmail.com";
       
            var validation =
                new EmailValidation(groupName, target);

            // Act
            var result = validation.Update();

            // Assert
            validation.ShouldBe(result);
            result.IsValid.ShouldBeTrue();
            result.Failures.ShouldBeEmpty();
        }
    }
}

