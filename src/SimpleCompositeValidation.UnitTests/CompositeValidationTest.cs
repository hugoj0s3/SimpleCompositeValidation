using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Moq;
using SimpleCompositeValidation.Base;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests
{
    public class CompositeValidationTest
    {
  
        private static readonly Fixture Fixture = new Fixture();

        [Fact]
        public void Update_AddingManyValidation_ItUpdatesAllUpdateMethodsFromValidationsAdded()
        {
            // Arrange
            var person = Fixture.Create<Person>();
            var validation = new CompositeValidation<Person>(person);
            var allValidation = MockAllValidations(validation);
            var firstNameMock = allValidation.FirstNameMock;
            var lastNameMock = allValidation.LastNameMock;
            var hasDriverLicenseMock = allValidation.HasDriverLicenseMock;
            var ageValidationMock = allValidation.AgeValidationMock;

            // Act
            validation.Update();

            // Assert
            firstNameMock.Verify(x => x.Update(person.FirstName), Times.Once);
            lastNameMock.Verify(x => x.Update(person.LastName), Times.Once);
            hasDriverLicenseMock.Verify(x => x.Update(person.HasDriverLicense), Times.Once);
            ageValidationMock.Verify(x => x.Update(person.Age), Times.Once);
        }

        [Fact]
        public void Update_PassingGroupName_ItUpdatesValidationAccordingWithGroupNamePassed()
        {
            // Arrange
           
            var person = Fixture.Create<Person>();
            var validation = new CompositeValidation<Person>(person);
            var allValidation = MockAllValidations(validation);
            var firstNameMock = allValidation.FirstNameMock;
            var lastNameMock = allValidation.LastNameMock;
            var hasDriverLicenseMock = allValidation.HasDriverLicenseMock;
            var ageValidationMock = allValidation.AgeValidationMock;
            

            // Act
            validation.Update<string>("LastName");

            // Assert
            firstNameMock.Verify(x => x.Update(person.FirstName), Times.Never);
            lastNameMock.Verify(x => x.Update(person.LastName), Times.Once);
            hasDriverLicenseMock.Verify(x => x.Update(person.HasDriverLicense), Times.Never);
            ageValidationMock.Verify(x => x.Update(person.Age), Times.Never);
        }

        [Fact]
        public void Update_PassingGroupNameAndKeepingTargetNull_NoValidationAreCalled()
        {
            // Arrange
            var person = Fixture.Create<Person>();
            var validation = new CompositeValidation<Person>(person);
            var allValidation = MockAllValidations(validation);
            var firstNameMock = allValidation.FirstNameMock;
            var lastNameMock = allValidation.LastNameMock;
            var hasDriverLicenseMock = allValidation.HasDriverLicenseMock;
            var ageValidationMock = allValidation.AgeValidationMock;

            // Act
            validation.Update<string>("LastName");

            // Assert
            firstNameMock.Verify(x => x.Update(person.FirstName), Times.Never);
            lastNameMock.Verify(x => x.Update(person.LastName), Times.Once);
            hasDriverLicenseMock.Verify(x => x.Update(person.HasDriverLicense), Times.Never);
            ageValidationMock.Verify(x => x.Update(person.Age), Times.Never);
        }

        [Fact]
        public void Update_PassingGroupNameAndValue_ItUpdatesValidationAccordingWithGroupNamePassed()
        {
            // Arrange
            var validation = new CompositeValidation<Person>();
            var lastName = Guid.NewGuid().ToString();
            var allValidation = MockAllValidations(validation);
            var lastNameMock = allValidation.LastNameMock;

            // Act
            validation.Update<string>("LastName", lastName);

            // Assert
            lastNameMock.Verify(x => x.Update(lastName), Times.Once);
        }

        [Fact]
        public void Update_ForcingFailedValidation_ItStopsOnThisValidation()
        {
            // Arrange
            var person = Fixture.Create<Person>();
            var validation = new CompositeValidation<Person>();
            var stopValidation = new MustNotValidation<object>("stops", x => true);
            validation.Add(stopValidation, x => x, true);
            var allValidation = MockAllValidations(validation);
            var firstNameMock = allValidation.FirstNameMock;
            var lastNameMock = allValidation.LastNameMock;
            var hasDriverLicenseMock = allValidation.HasDriverLicenseMock;
            var ageValidationMock = allValidation.AgeValidationMock;

            // Act
            validation.Update(person);

            // Assert
            firstNameMock.Verify(x => x.Update(person.FirstName), Times.Never);
            lastNameMock.Verify(x => x.Update(person.LastName), Times.Never);
            hasDriverLicenseMock.Verify(x => x.Update(person.HasDriverLicense), Times.Never);
            ageValidationMock.Verify(x => x.Update(person.Age), Times.Never);
        }

        [Fact]
        public void Update_ForcingNull_ItNeverPerformsValidation()
        {
            // Arrange
            var validation = new CompositeValidation<Person>();
            var allValidation = MockAllValidations(validation);
            var firstNameMock = allValidation.FirstNameMock;
            var lastNameMock = allValidation.LastNameMock;
            var hasDriverLicenseMock = allValidation.HasDriverLicenseMock;
            var ageValidationMock = allValidation.AgeValidationMock;

            // Act
            validation.Update<string>("LastName");

            // Assert
            firstNameMock.Verify(x => x.Update(It.IsAny<string>()), Times.Never);
            lastNameMock.Verify(x => x.Update(It.IsAny<string>()), Times.Never);
            hasDriverLicenseMock.Verify(x => x.Update(It.IsAny<bool>()), Times.Never);
            ageValidationMock.Verify(x => x.Update(It.IsAny<int>()), Times.Never);
        }


        private ValidationsMocked MockAllValidations(CompositeValidation<Person> validation)
        {
            var firstNameMock = new Mock<IValidation<string>>();
            var lastNameMock = new Mock<IValidation<string>>();
            var hasDriverLicenseMock = new Mock<IValidation<bool>>();
            var ageValidationMock = new Mock<IValidation<int>>();

            SetupEmptyFailures(firstNameMock, lastNameMock);
            SetupEmptyFailures(hasDriverLicenseMock);
            SetupEmptyFailures(ageValidationMock);

            firstNameMock.Setup(x => x.GroupName).Returns("FirstName");
            lastNameMock.Setup(x => x.GroupName).Returns("LastName");
            hasDriverLicenseMock.Setup(x => x.GroupName).Returns("HasDriverLicense");
            ageValidationMock.Setup(x => x.GroupName).Returns("Age");

            validation.Add(firstNameMock.Object, x => x.FirstName);
            validation.Add(lastNameMock.Object, x => x.LastName);
            validation.Add(hasDriverLicenseMock.Object, x => x.HasDriverLicense);
            validation.Add(ageValidationMock.Object, x => x.Age);

            return new ValidationsMocked(firstNameMock, lastNameMock, hasDriverLicenseMock, ageValidationMock);
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

        private class ValidationsMocked
        {
            public ValidationsMocked(Mock<IValidation<string>> firstNameMock, Mock<IValidation<string>> lastNameMock,
                Mock<IValidation<bool>> hasDriverLicenseMock, Mock<IValidation<int>> ageValidationMock)
            {
                FirstNameMock = firstNameMock;
                LastNameMock = lastNameMock;
                HasDriverLicenseMock = hasDriverLicenseMock;
                AgeValidationMock = ageValidationMock;
            }

            public Mock<IValidation<string>> FirstNameMock { get; private set; }
            public Mock<IValidation<string>> LastNameMock { get; private set; }
            public Mock<IValidation<bool>> HasDriverLicenseMock { get; private set; }
            public Mock<IValidation<int>> AgeValidationMock { get; private set; }
        }
    }
}