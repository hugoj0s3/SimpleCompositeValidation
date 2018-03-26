using System;
using System.Collections.Generic;
using System.Text;
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
            var validation = new ValidationTest("groupName", "message");

            // Act
            var failure = new Failure(validation, customMessage);

            // Assert
            failure.Message.ShouldBe(customMessage);
            failure.GroupName.ShouldBe(validation.GroupName);
            failure.Severity.ShouldBe(validation.Severity);
        }

        private class ValidationTest : Validation<string>
        {
            public ValidationTest(string groupName, string message, int severity = 1) : base(groupName, message, severity)
            {
            }

            protected override IList<Failure> Validate()
            {
                return new List<Failure>();
            }
        }
    }
}
