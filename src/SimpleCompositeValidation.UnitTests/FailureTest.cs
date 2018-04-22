using System.Collections.Generic;
using Moq;
using Shouldly;
using SimpleCompositeValidation.Base;
using Xunit;

namespace SimpleCompositeValidation.UnitTests
{
    public class FailureTest
    {
        [Fact]
        public void Constructor_PassingNoDefaultMessage_TheFailedIsCreatedWithPassedMessage()
        {
            // Arrange
            string customMessage = "CustomMessage";
            var validation = new Mock<Validation<object>>("groupName", default(object)).Object;

            // Act
            var failure = new Failure(validation, customMessage);

            // Assert
            failure.Message.ShouldBe(customMessage);
            failure.GroupName.ShouldBe(validation.GroupName);
            failure.Severity.ShouldBe(validation.Severity);
        }

   
    }
}
