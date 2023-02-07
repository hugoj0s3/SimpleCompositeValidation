using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Shouldly;
using SimpleCompositeValidation.Base;
using SimpleCompositeValidation.Extensions;
using SimpleCompositeValidation.UnitTests.Extensions;
using SimpleCompositeValidation.Validations;
using SimpleCompositeValidation.Validations.String;
using Xunit;

namespace SimpleCompositeValidation.UnitTests
{
    public class ManyValidationsTest
    {

        private const int MillisecondsTimeout = 500;

        private readonly ICompositeValidation<Person> _personValidation = new CompositeValidation<Person>()
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
                .Validation.ShouldBeOfType<RegExValidation>();

            _personValidation.Failures.Single(x => x.GroupName == nameof(Person.BirthDate))
                .Validation.ShouldBeOfType<MustNotValidation<DateTime>>();

			_personValidation.LastUpdate.ShouldBeCloseTo(DateTime.Now, MillisecondsTimeout);
	        foreach (var item in _personValidation.Validations)
	        {
		        item.LastUpdate.ShouldBeCloseTo(DateTime.Now, MillisecondsTimeout);
	        }
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

	        _personValidation.LastUpdate.ShouldBeCloseTo(DateTime.Now, MillisecondsTimeout);
	        foreach (var item in _personValidation.Validations)
	        {
		        item.LastUpdate.ShouldBeCloseTo(DateTime.Now, MillisecondsTimeout);
	        }
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

	        _personValidation.LastUpdate.ShouldBeCloseTo(DateTime.Now, MillisecondsTimeout);
	        foreach (var item in _personValidation.Validations)
	        {
		        item.LastUpdate.ShouldBeCloseTo(DateTime.Now, MillisecondsTimeout);
	        }
		}

	    [Fact]
	    public void UpdatePartially_UpdatingPartiallyAGroupName_ItRaisesFailuresCorrectly()
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
		    person.LastName = "Hugo";
		    _personValidation.Update(person, "LastName");

			// Assert 
			_personValidation.IsValid.ShouldBeFalse();
		    _personValidation.Failures.Count.ShouldBe(1);

		    _personValidation.Failures.Single(x => x.GroupName == nameof(Person.FirstName))
			    .Validation.ShouldBeOfType<NullValidation>();

	    }

	    [Fact]
	    public void UpdatePartially2_UpdatingPartiallyAGroupName_ItRaisesFailuresCorrectly()
	    {
		    //Arrange
		    var person = new Person()
		    {
			    FirstName = null,
			    LastName = "Hugo",
			    Email = "hugo@testemail.com.br",
			    Phone = "+5501234567",
			    BirthDate = DateTime.Now.AddYears(-20)
		    };

		    // Act
		    _personValidation.Update(person);
		    person.LastName = null;
		    _personValidation.Update("LastName");

		    // Assert 
		    _personValidation.IsValid.ShouldBeFalse();
		    _personValidation.Failures.Count.ShouldBe(2);

		    _personValidation.Failures.Single(x => x.GroupName == nameof(Person.FirstName))
			    .Validation.ShouldBeOfType<NullValidation>();

		    _personValidation.Failures.Single(x => x.GroupName == nameof(Person.LastName))
			    .Validation.ShouldBeOfType<NullValidation>();

		}

	    [Fact]
	    public void UpdatePartially3_UpdatingPartiallyAGroupName_ItRaisesFailuresCorrectlyWithTheSummaryMessage()
	    {
			//Arrange
		    var summaryMessage = "#TestSummaryMessage";
		    var validations = new CompositeValidation<Person>(null, null, summaryMessage)
			    .NotNull(nameof(Person.FirstName), x => x.FirstName)
			    .MinimumLength(nameof(Person.FirstName), x => x.FirstName, 3)
			    .MaximumLength(nameof(Person.FirstName), x => x.FirstName, 10)
			    .NotNull(nameof(Person.LastName), x => x.LastName)
			    .MinimumLength(nameof(Person.LastName), x => x.LastName, 3)
			    .MaximumLength(nameof(Person.LastName), x => x.LastName, 10);


			var person = new Person()
		    {
			    FirstName = "Hugo",
			    LastName = "Jose",
			    Email = "hugo@testemail.com.br",
			    Phone = "+5501234567",
			    BirthDate = DateTime.Now.AddYears(-20)
		    };

		    // Act
		    validations.Update(person);
		    validations.Update("LastName", "J");

		    // Assert 
		    validations.IsValid.ShouldBeFalse();
		    validations.Failures.Count.ShouldBe(2);

		    validations.Failures.First().Validation.ShouldBe(validations);

		    validations.Failures.Select(x => x.Validation)
			    .OfType<CompositeValidation<Person>>().Single()
			    .SummaryMessage.ShouldBe(summaryMessage);

		    validations.Failures.Single(x => x.GroupName == nameof(Person.LastName))
			    .Validation.ShouldBeOfType<StringMinimumLengthValidation>();
		}

	    [Fact]
	    public void Update_ConfiguringValidationWithAddForEach_ItUpdateTheValidationCorrectly()
	    {
		    // Arrange
			string summaryMessage = "#TestError";
		    var personValidation = new CompositeValidation<Person>(null, null, summaryMessage)
			    .NotNull(nameof(Person.FirstName), x => x.FirstName)
			    .MinimumLength(nameof(Person.FirstName), x => x.FirstName, 3)
			    .MaximumLength(nameof(Person.FirstName), x => x.FirstName, 10)
			    .NotNull(nameof(Person.LastName), x => x.LastName)
			    .MinimumLength(nameof(Person.LastName), x => x.LastName, 3)
			    .MaximumLength(nameof(Person.LastName), x => x.LastName, 10);

		    const string minimumPlayersMessage = "A Football team must have at least 11 players";
		    const string maximumPlayersMessage = "Including the substitutes a Football team can not have more 23 players";

		    var footballTeamValidation = new CompositeValidation<FootballTeam>()
			    .AddForEach(personValidation, team => team.Players)
			    .MinimumSize("Players", x => x.Players, 11, minimumPlayersMessage)
			    .MaximumSize("Players", x => x.Players, 23, maximumPlayersMessage);

			var footballTeam = new FootballTeam()
		    {
			    Name = "Liverpool",
			    Players = new List<Person>()
			    {
				    new Person()
				    {
					    FirstName = "Steven",
					    LastName = "Gerard"
				    },
				    new Person()
				    {
					    FirstName = "Invalid",
					    LastName = "a"
				    }

			    }
		    };

			// Act
		    footballTeamValidation.Update(footballTeam);

			// Assert
			footballTeamValidation.IsValid.ShouldBeFalse();
			footballTeamValidation.Failures.Count.ShouldBe(2);
			footballTeamValidation.Failures.First().Message.ShouldBe(summaryMessage);
		    footballTeamValidation.Failures.ElementAt(1).Message.ShouldBe(minimumPlayersMessage);
		}

	    [Fact]
	    public void Update2_ConfiguringValidationWithAddForEach_ItUpdateTheValidationCorrectly()
	    {
			// Arrange
		    var personValidation = new CompositeValidation<Person>()
			    .NotNull(nameof(Person.FirstName), x => x.FirstName)
			    .MinimumLength(nameof(Person.FirstName), x => x.FirstName, 3)
			    .MaximumLength(nameof(Person.FirstName), x => x.FirstName, 10)
			    .NotNull(nameof(Person.LastName), x => x.LastName)
			    .MinimumLength(nameof(Person.LastName), x => x.LastName, 3)
			    .MaximumLength(nameof(Person.LastName), x => x.LastName, 10);

		    const string maximumPlayersMessage = "a Football team can not have more 11 players";

		    var footballTeamValidation = new CompositeValidation<FootballTeam>()
			    .AddForEach(personValidation, team => team.Players)
			    .MaximumSize("Players", x => x.Players, 11, maximumPlayersMessage);

		    var footballTeam = new FootballTeam()
		    {
			    Name = "Liverpool 2005",
			    Players = new List<Person>()
			    {
				    new Person()
				    {
						FirstName = "Jerzy",
						LastName = "Dudek"
					},
				    new Person()
				    {
					    FirstName = "Steve",
					    LastName = "Finnan"
					},
				    new Person()
				    {
					    FirstName = "Jamie",
					    LastName = "Carragher"
					},
				    new Person()
				    {
					    FirstName = "Sami",
					    LastName = "Hyypia"
					},
				    new Person()
				    {
					    FirstName = "Djimi",
					    LastName = "Traoré"
					},
				    new Person()
				    {
					    FirstName = "Xabi",
					    LastName = "Alonso"
				    },
				    new Person()
				    {
					    FirstName = "Luis",
					    LastName = "García"
					},
				    new Person()
				    {
					    FirstName = "Steven",
					    LastName = "Gerrard"
					},
				    new Person()
				    {
					    FirstName = "John",
					    LastName = "Arne Riise"
					},
				    new Person()
				    {
					    FirstName = "Harry",
					    LastName = "Kewell"
					},
				    new Person()
				    {
					    FirstName = "Milan",
					    LastName = "Baroš"
					},
				    new Person()
				    {
					    FirstName = "Hugo",
					    LastName = "Jose"
				    }
				}
		    };

		    // Act
		    footballTeamValidation.Update(footballTeam);

		    // Assert
		    footballTeamValidation.IsValid.ShouldBeFalse();
		    footballTeamValidation.Failures.Count.ShouldBe(1);
		    footballTeamValidation.Failures.First().Message.ShouldBe(maximumPlayersMessage);
	    }

        [Fact]
        public void Update_NotNullAndNullForEach_RaisesErrors()
        {


            var footballTeamValidation = new CompositeValidation<FootballTeam>()
                .NotNullForEach("Players", x => x.Players.Select(y => y.LastName))
                .NullForEach("Players", x => x.Players);

            var footballTeam = new FootballTeam()
            {
                Name = "Liverpool 2005",
                Players = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "Jerzy",
                        LastName = "Dudek"
                    },
                    new Person()
                    {
                        FirstName = "Steve",
                        LastName = "Finnan"
                    },
                    new Person()
                    {
                        FirstName = "Jamie",
                        LastName = "Carragher"
                    },
                    new Person()
                    {
                        FirstName = "Sami",
                        LastName = "Hyypia"
                    },
                    new Person()
                    {
                        FirstName = "Djimi",
                        LastName = "Traoré"
                    },
                    new Person()
                    {
                        FirstName = "Xabi",
                        LastName = "Alonso"
                    },
                    new Person()
                    {
                        FirstName = "Luis",
                        LastName = "García"
                    },
                    new Person()
                    {
                        FirstName = "Steven",
                        LastName = "Gerrard"
                    },
                    new Person()
                    {
                        FirstName = "John",
                        LastName = "Arne Riise"
                    },
                    new Person()
                    {
                        FirstName = "Harry",
                        LastName = "Kewell"
                    },
                    new Person()
                    {
                        FirstName = "Milan",
                        LastName = "Baroš"
                    },
                    new Person()
                    {
                        FirstName = "Hugo",
                        LastName = null
                    }
                }
            };

            // Act
            footballTeamValidation.Update(footballTeam);

            // Assert
            footballTeamValidation.IsValid.ShouldBeFalse();
            footballTeamValidation.Failures.Count.ShouldBe(2);
        }


        [Fact]
        public void Update_MinimumAndMaximumSizeForEach_RaisesErrors()
        {

            var footballTeamValidation = new CompositeValidation<FootballTeam>()
                .MinimumLengthForEach("Players", x => x.Players.Select(y => y.LastName), 5)
                .MaximumLengthForEach("Players", x => x.Players.Select(y => y.FirstName), 5);

            var footballTeam = new FootballTeam()
            {
                Name = "Liverpool 2005",
                Players = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "Jerzy",
                        LastName = "Dudek"
                    },
                    new Person()
                    {
                        FirstName = "Steve",
                        LastName = "Finnan"
                    },
                    new Person()
                    {
                        FirstName = "Jamie",
                        LastName = "Carragher"
                    },
                    new Person()
                    {
                        FirstName = "Sami",
                        LastName = "Hyypia"
                    },
                    new Person()
                    {
                        FirstName = "Djimi",
                        LastName = "Traoré"
                    },
                    new Person()
                    {
                        FirstName = "Xabi",
                        LastName = "Alonso"
                    },
                    new Person()
                    {
                        FirstName = "Luis",
                        LastName = "García"
                    },
                    new Person()
                    {
                        FirstName = "Steven",
                        LastName = "Gerrard"
                    },
                    new Person()
                    {
                        FirstName = "John",
                        LastName = "Arne Riise"
                    },
                    new Person()
                    {
                        FirstName = "Harry",
                        LastName = "Kewell"
                    },
                    new Person()
                    {
                        FirstName = "Milan",
                        LastName = "Baroš"
                    },
                    new Person()
                    {
                        FirstName = "Hugooool",
                        LastName = "Jose"
                    }
                }
            };

            // Act
            footballTeamValidation.Update(footballTeam);

            // Assert
            footballTeamValidation.IsValid.ShouldBeFalse();
            footballTeamValidation.Failures.Count.ShouldBe(2);
        }


        [Fact]
        public void Update_RegExAndEmailForEach_RaisesErrors()
        {

            var footballTeamValidation = new CompositeValidation<FootballTeam>()
                .EmailForEach("Players", x => x.Players.Select(y => y.LastName))
                .RegExForEach("Players", x => x.Players.Select(y => y.FirstName), "^[a-zA-Z]+$");

            var footballTeam = new FootballTeam()
            {
                Name = "Liverpool 2005",
                Players = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "Jerzy",
                        LastName = "Dudek"
                    },
                    new Person()
                    {
                        FirstName = "Steve",
                        LastName = "Finnan"
                    },
                    new Person()
                    {
                        FirstName = "Jamie",
                        LastName = "Carragher"
                    },
                    new Person()
                    {
                        FirstName = "Sami",
                        LastName = "Hyypia"
                    },
                    new Person()
                    {
                        FirstName = "Djimi",
                        LastName = "Traoré"
                    },
                    new Person()
                    {
                        FirstName = "Xabi",
                        LastName = "Alonso"
                    },
                    new Person()
                    {
                        FirstName = "Luis",
                        LastName = "García"
                    },
                    new Person()
                    {
                        FirstName = "Steven",
                        LastName = "Gerrard"
                    },
                    new Person()
                    {
                        FirstName = "John",
                        LastName = "Arne Riise"
                    },
                    new Person()
                    {
                        FirstName = "Harry",
                        LastName = "Kewell"
                    },
                    new Person()
                    {
                        FirstName = "Milan",
                        LastName = "Baroš"
                    },
                    new Person()
                    {
                        FirstName = "Hugooool9",
                        LastName = "Jose"
                    }
                }
            };

            // Act
            footballTeamValidation.Update(footballTeam);

            // Assert
            footballTeamValidation.IsValid.ShouldBeFalse();
            footballTeamValidation.Failures.Count.ShouldBe(2);
        }


        [Fact]
        public void Update_NotEmptyForEach_NoErrors()
        {

            var footballTeamValidation = new CompositeValidation<FootballTeam>()
                .NotEmptyForEach("Players", x => x.Players.Select(y => y.LastName));
            

            var footballTeam = new FootballTeam()
            {
                Name = "Liverpool 2005",
                Players = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "Jerzy",
                        LastName = "Dudek"
                    },
                    new Person()
                    {
                        FirstName = "Steve",
                        LastName = "Finnan"
                    },
                    new Person()
                    {
                        FirstName = "Jamie",
                        LastName = "Carragher"
                    },
                    new Person()
                    {
                        FirstName = "Sami",
                        LastName = "Hyypia"
                    },
                    new Person()
                    {
                        FirstName = "Djimi",
                        LastName = "Traoré"
                    },
                    new Person()
                    {
                        FirstName = "Xabi",
                        LastName = "Alonso"
                    },
                    new Person()
                    {
                        FirstName = "Luis",
                        LastName = "García"
                    },
                    new Person()
                    {
                        FirstName = "Steven",
                        LastName = "Gerrard"
                    },
                    new Person()
                    {
                        FirstName = "John",
                        LastName = "Arne Riise"
                    },
                    new Person()
                    {
                        FirstName = "Harry",
                        LastName = "Kewell"
                    },
                    new Person()
                    {
                        FirstName = "Milan",
                        LastName = "Baroš"
                    },
                    new Person()
                    {
                        FirstName = "Hugooool9",
                        LastName = "Jose"
                    }
                }
            };

            // Act
            footballTeamValidation.Update(footballTeam);

            // Assert
            footballTeamValidation.IsValid.ShouldBeTrue();
            footballTeamValidation.Failures.Count.ShouldBe(0);
        }


        [Fact]
        public void Update_MustAndNotMust_NoErrors()
        {

            var footballTeamValidation = new CompositeValidation<FootballTeam>()
                .MustForEach("Players", x => x.Players, x => x?.FirstName != null)
                .MustNotForEach("Players", x => x.Players, x => x?.LastName == null);

            var footballTeam = new FootballTeam()
            {
                Name = "Liverpool 2005",
                Players = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "Jerzy",
                        LastName = "Dudek"
                    },
                    new Person()
                    {
                        FirstName = "Steve",
                        LastName = "Finnan"
                    },
                    new Person()
                    {
                        FirstName = "Jamie",
                        LastName = "Carragher"
                    },
                    new Person()
                    {
                        FirstName = "Sami",
                        LastName = "Hyypia"
                    },
                    new Person()
                    {
                        FirstName = "Djimi",
                        LastName = "Traoré"
                    },
                    new Person()
                    {
                        FirstName = "Xabi",
                        LastName = "Alonso"
                    },
                    new Person()
                    {
                        FirstName = "Luis",
                        LastName = "García"
                    },
                    new Person()
                    {
                        FirstName = "Steven",
                        LastName = "Gerrard"
                    },
                    new Person()
                    {
                        FirstName = "John",
                        LastName = "Arne Riise"
                    },
                    new Person()
                    {
                        FirstName = "Harry",
                        LastName = "Kewell"
                    },
                    new Person()
                    {
                        FirstName = "Milan",
                        LastName = "Baroš"
                    },
                    new Person()
                    {
                        FirstName = "Hugo",
                        LastName = "Jose"
                    }
                }
            };

            // Act
            footballTeamValidation.Update(footballTeam);

            // Assert
            footballTeamValidation.IsValid.ShouldBeTrue();
            footballTeamValidation.Failures.Count.ShouldBe(0);
        }



        private class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirthDate { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }

	    private class FootballTeam
	    {
			public string Name { get; set; }

			public IList<Person> Players { get; set; }
	    }
	}
}