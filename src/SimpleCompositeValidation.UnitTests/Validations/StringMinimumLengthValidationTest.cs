using System.Linq;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class StringMinimumLengthValidationTest
    {
        [Fact]
        public void Update_ParsingStringWithLessThan4Chars_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            var groupName = "StringMinimum";
            var invalidString = "abc";
            var defaultMessage = $"{groupName} requires at least 4 characters";
            var defaultSeverity = 1;
            var validation = new StringMinimumLengthValidation(groupName, 4, invalidString);
            
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
        public void Update_ParsingStringWithMoreThan4Chars_ItUpdateItSelfToValidValidation()
        {
            // Arrange
            var groupName = "StringMinimum";
            var validString = "abcde";
            var validation = new StringMinimumLengthValidation(groupName, 4);

            // Act
            var result = validation.Update(validString);

            // Assert
            validation.ShouldBe(result);
            result.IsValid.ShouldBeTrue();
            result.Failures.ShouldBeEmpty();
        }
    }
}
