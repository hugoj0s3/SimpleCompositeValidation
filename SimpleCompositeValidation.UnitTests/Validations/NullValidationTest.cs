using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var validation = new NullValidation<object>(groupName, new object(), false, message, severtiy);

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
            const string groupName = "NullObject";
            const string message = "TestError";
            const int severtiy = 999;
            var validation = new NullValidation<object>(groupName, null, true, message, severtiy);

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
