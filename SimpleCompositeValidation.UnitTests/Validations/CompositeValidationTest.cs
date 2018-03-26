using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Moq;
using SimpleCompositeValidation.Base;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations
{
    public class CompositeValidationTest
    {
        private readonly CompositeValidation<Person> validation;
        private static readonly Fixture Fixture = new Fixture();
        public CompositeValidationTest()
        {
            validation = new CompositeValidation<Person>();
        }

        [Fact]
        public void Test()
        {
            // Arrange
            var person = Fixture.Create<Person>();
            var firstNameMock = new Mock<IValidation<string>>();
            var lastNameMock = new Mock<IValidation<string>>();
            var hasDriverLicenseMock = new Mock<IValidation<bool>>();
            var ageValidationMock = new Mock<IValidation<int>>();

            validation.Add(firstNameMock.Object, x => x.FirstName);
            validation.Add(lastNameMock.Object, x => x.LastName);
            validation.Add(hasDriverLicenseMock.Object, x => x.HasDriverLicense);
            validation.Add(ageValidationMock.Object, x => x.Age);

            SetupEmptyFailures(firstNameMock, lastNameMock);
            SetupEmptyFailures(hasDriverLicenseMock);
            SetupEmptyFailures(ageValidationMock);

            // Act
            validation.Update(person);

            // Assert
            firstNameMock.Verify(x => x.Update(person.FirstName), Times.Once);
            lastNameMock.Verify(x => x.Update(person.LastName), Times.Once);
            hasDriverLicenseMock.Verify(x => x.Update(person.HasDriverLicense), Times.Once);
            ageValidationMock.Verify(x => x.Update(person.Age), Times.Once);
        }

        private void SetupEmptyFailures<T>(params Mock<IValidation<T>>[] validations)
        {
            foreach (var item in validations)
            {
                item.Setup(x => x.Failures).Returns(new List<Failure>().AsReadOnly);
            }
        }

        private class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool HasDriverLicense { get; set; }
            public int Age { get; set; }
        }

    }
}
