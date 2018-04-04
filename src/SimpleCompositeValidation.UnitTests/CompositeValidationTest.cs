using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using Shouldly;
using SimpleCompositeValidation.Base;
using SimpleCompositeValidation.Extensions;
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
			validation.Validations.ShouldContain(firstNameMock.Object);
	        validation.Validations.ShouldContain(lastNameMock.Object);
	        validation.Validations.ShouldContain(hasDriverLicenseMock.Object);
	        validation.Validations.ShouldContain(ageValidationMock.Object);

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
            validation.Update("LastName");

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
            validation.Update("LastName");

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
            validation.Update("LastName", lastName);

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
			validation.IsValid.ShouldBeFalse();
            firstNameMock.Verify(x => x.Update(person.FirstName), Times.Never);
            lastNameMock.Verify(x => x.Update(person.LastName), Times.Never);
            hasDriverLicenseMock.Verify(x => x.Update(person.HasDriverLicense), Times.Never);
            ageValidationMock.Verify(x => x.Update(person.Age), Times.Never);
        }

	    [Fact]
	    public void Update_ForcingFailedValidationAndPassingSummaryMessage_ItInsertsTheSummaryMessage()
	    {
		    // Arrange
		    var person = Fixture.Create<Person>();
		    var summaryMessage = "#TestSummaryMessage";
		    var validation = new CompositeValidation<Person>(summaryMessage);
		    var stopValidation = new MustNotValidation<object>("Failing", x => true);
		    validation.Add(stopValidation, x => x);
		   

		    // Act
		    validation.Update(person);

			// Assert
			validation.IsValid.ShouldBeFalse();
		    validation.Failures.Count.ShouldBe(2);
		    validation.Failures.First().Message.ShouldBe(summaryMessage);
		}

	    [Fact]
	    public void NotEmpty_AddingNotEmptyString_ItAddsNotEmptyStringValidation()
	    {
			// Arrange
		    var validation = new CompositeValidation<Person>();

			// Act
		    validation.NotEmpty("AnyGroupName", x => x.FirstName);

		    // Assert
			validation.Validations.OfType<NotEmptyStringValidation>().Count().ShouldBe(1);
	    }

	    [Fact]
	    public void NotEmpty_AddingNotEmptyList_ItAddsNotEmptyEnumerableValidation()
	    {
		    // Arrange
		    var validation = new CompositeValidation<Person>();

		    // Act
		    validation.NotEmpty("AnyGroupName", x => new List<object>());

			// Assert
			validation.Validations.OfType<NotEmptyEnumerableValidation<object>>().Count().ShouldBe(1);
	    }

	    [Fact]
	    public void Null_AddingNullValidation_ItAddsNullValidation()
	    {
		    // Arrange
		    var validation = new CompositeValidation<Person>();

		    // Act
		    validation.Null("AnyGroupName", x => x);

		    // Assert
		    validation.Validations.Single().ShouldBeOfType<NullValidation>();
		    validation.Validations.OfType<NullValidation>().Single().AcceptNull.ShouldBeTrue();
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

	        firstNameMock.Setup(x => x.Message).Returns("FirstName is not valid");
	        lastNameMock.Setup(x => x.Message).Returns("LastName is not valid");
	        hasDriverLicenseMock.Setup(x => x.Message).Returns("HasDriverLicense is not valid");
	        ageValidationMock.Setup(x => x.Message).Returns("Age is not valid");

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
            public Person(string firstName, string lastName, bool hasDriverLicense, int age)
            {
                FirstName = firstName;
                LastName = lastName;
                HasDriverLicense = hasDriverLicense;
                Age = age;
            }

            public string FirstName { get; }
            public string LastName { get; }
            public bool HasDriverLicense { get; }
            public int Age { get; }
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

            public Mock<IValidation<string>> FirstNameMock { get; }
            public Mock<IValidation<string>> LastNameMock { get; }
            public Mock<IValidation<bool>> HasDriverLicenseMock { get; }
            public Mock<IValidation<int>> AgeValidationMock { get; }
        }
    }
}