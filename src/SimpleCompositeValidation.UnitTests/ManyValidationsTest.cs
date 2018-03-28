using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using SimpleCompositeValidation.Extensions;
using SimpleCompositeValidation.Validations;
using Xunit;

namespace SimpleCompositeValidation.UnitTests
{
    public class ManyValidationsTest
    {
        private readonly CompositeValidation<Person> _personValidation = new CompositeValidation<Person>()
            .NotNull(nameof(Person.FirstName), x => x.FirstName)
            .MinimumLength(nameof(Person.FirstName), x => x.FirstName, 3)
            .MaximumLength(nameof(Person.FirstName), x => x.FirstName, 10)
            .NotNull(nameof(Person.LastName), x => x.LastName)
            .MinimumLength(nameof(Person.LastName), x => x.LastName, 3)
            .MaximumLength(nameof(Person.LastName), x => x.LastName, 10)
            .NotNull(nameof(Person.Email), x => x.Email)
            .Email(nameof(Person.Email), x => x.Email)
            .RegEx(nameof(Person.Phone), x => x.Phone, @"^[0-9\-\+]{9,15}$")
            .MustNot(nameof(Person.BirthDate), x => x.BirthDate, x => x.Year < 1850)
            .Must(nameof(Person.BirthDate), x => x.BirthDate, x => x < DateTime.Now);

        [Fact]
        public void Update1_AddingAllValidation_ItRaisesFailuresCorrectly()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = "ab",
                LastName = "ab",
                Email = "test#gmail.com",
                Phone = "ABC994847",
                BirthDate = new DateTime(1849, 01, 01)
            };

            // Act
            _personValidation.Update(person);

            // Assert 
            _personValidation.IsValid.ShouldBeFalse();
            _personValidation.Failures.Count.ShouldBe(5);

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.FirstName))
                .Validation.ShouldBeOfType<StringMinimumLengthValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.LastName))
                .Validation.ShouldBeOfType<StringMinimumLengthValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.Email))
                .Validation.ShouldBeOfType<EmailValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.Phone))
                .Validation.ShouldBeOfType<RegularExpressionValidation>();

            _personValidation.Failures.ElementAt(4).GroupName.ShouldBe(nameof(Person.BirthDate));
            _personValidation.Failures.ElementAt(4).Validation.ShouldBeOfType<MustNotValidation<DateTime>>();
        }

        [Fact]
        public void Update2_AddingAllValidation_ItRaisesFailuresCorrectly()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = "abcsdfsdghytruyuio789o 98p0p´rtretret",
                LastName = "abcsdfsdghytruyuio789o 98p0p´rtretret",
                Email = null,
                Phone = "+5501234567",
                BirthDate = DateTime.Now.AddDays(1)
            };

            // Act
            _personValidation.Update(person);

            // Assert 
            _personValidation.IsValid.ShouldBeFalse();
            _personValidation.Failures.Count.ShouldBe(4);

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.FirstName))
                .Validation.ShouldBeOfType<StringMaximumLengthValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.LastName))
                .Validation.ShouldBeOfType<StringMaximumLengthValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.Email))
                .Validation.ShouldBeOfType<NullValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.BirthDate))
                .Validation.ShouldBeOfType<MustValidation<DateTime>>();
        }

        [Fact]
        public void Update3_AddingAllValidation_ItRaisesFailuresCorrectly()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = null,
                LastName = null,
                Email = "hugo@testemail.com.br",
                Phone = "+5501234567",
                BirthDate = DateTime.Now.AddYears(-20)
            };

            // Act
            _personValidation.Update(person);

            // Assert 
            _personValidation.IsValid.ShouldBeFalse();
            _personValidation.Failures.Count.ShouldBe(2);

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.FirstName))
                .Validation.ShouldBeOfType<NullValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.LastName))
                .Validation.ShouldBeOfType<NullValidation>();

        }

        private class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirthDate { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}