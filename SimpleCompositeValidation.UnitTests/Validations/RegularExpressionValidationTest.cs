using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class RegularExpressionValidationTest
    {
        [Fact]
        public void Update_GivenUnMatchedPattern_ItUpdateItSelfToAnInvalidValidation()
        {
            // Arrange
            var groupName = "Name";
            var pattern = "^[A-Za-z]+$";
            var target = "HUgo*9)82";
            var defaultSeverity = 1;
            var defaultMessage = "Name is not valid";

            var validation = 
                new RegularExpressionValidation(groupName, pattern, target);

            // Act
            var result = validation.Update();

            // Assert
            result.ShouldBe(validation);
            result.IsValid.ShouldBeFalse();
            result.Failures.Count.ShouldBe(1);
            result.Failures.Single().Message.ShouldBe(defaultMessage);
            result.Failures.Single().GroupName.ShouldBe(groupName);
            result.Failures.Single().Severity.ShouldBe(defaultSeverity);
        }

        [Fact]
        public void Update_GivenMatchedPattern_ItUpdateItSelfToValidValidation()
        {
            // Arrange
            var groupName = "Name";
            var pattern = "^[A-Za-z]+$";
            var target = "Hugo";

            var validation =
                new RegularExpressionValidation(groupName, pattern, target);

            // Act
            var result = validation.Update();

            // Assert
            result.ShouldBe(validation);
            result.IsValid.ShouldBeTrue();
            result.Failures.ShouldBeEmpty();
        }
    }
}
