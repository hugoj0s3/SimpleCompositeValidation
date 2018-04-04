using System.Linq;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class StringMaximumLengthValidationTest
    {
        [Fact]
        public void Update_ParsingStringWithMoreThan4Chars_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            var groupName = "StringMaximum";
            var invalidString = "abcde";
            var defaultMessage = $"{groupName} the characters length limit is 4";
            var defaultSeverity = 1;
            var validation = new StringMaximumLengthValidation(groupName, 4, invalidString);
            
            // Act
            var result = validation.Update();

            // Assert
            validation.ShouldBe(result);
            result.IsValid.ShouldBeFalse();
            result.Failures.Count.ShouldBe(1);
            result.Failures.Single().Message.ShouldBe(defaultMessage);
            result.Failures.Single().GroupName.ShouldBe(groupName);
            result.Failures.Single().Severity.ShouldBe(defaultSeverity);
        }

        [Fact]
        public void Update_ParsingStringWithLessThan4Chars_ItUpdateItSelfToValidValidation()
        {
            // Arrange
            var groupName = "StringMaximum";
            var validString = "abc";
            var validation = new StringMaximumLengthValidation(groupName, 4);

            // Act
            var result = validation.Update(validString);

            // Assert
            validation.ShouldBe(result);
            result.IsValid.ShouldBeTrue();
            result.Failures.ShouldBeEmpty();
        }
    }
}
