# SimpleValidationComposite 
 Simple validation library based on composition. The idea is you create single and simple validations, and then you add those validations to validate an entire model. 
  

## Compositing:

     CompositeValidation<Person> Validation = new CompositeValidation<Person>()
                .NotNull(nameof(Person.FirstName), x => x.FirstName) //Adding NullValidation
                .MinimumLength(nameof(Person.FirstName), x => x.FirstName, 3) //Adding StringMinimumLengthValidation
                .MaximumLength(nameof(Person.FirstName), x => x.FirstName, 10) //Adding StringMaximumLengthValidation
                .Email(nameof(Person.Email), x => x.Email) // Adding EmailValidation
                .RegEx(nameof(Person.Phone), x => x.Phone, @"^[0-9\-\+]{9,15}$") //Adding RegExValidation
                .MustNot(nameof(Person.BirthDate), x => x.BirthDate, x => x.Year < 1850) //Adding MustNotValidation
                .Must(nameof(Person.BirthDate), x => x.BirthDate, x => x < DateTime.Now) //Adding MustValidation
                .Add(new CustomValidation(nameof(Person)), x => x); //Adding CustomValidation

            if (!Validation.Update(person).IsValid)
            {
                IReadOnlyCollection<Failure> failures = Validation.Failures;
            }


## Creating a custom validate:
        public class CustomValidation : Validation<Person>
        {
            public CustomValidation(
                string groupName, 
                Person target = null) 
                : base(groupName, message:"A person who are under 16 can not have driver license", target:target, severity:1)
            {
            }

            protected override IList<Failure> Validate()
            {
                var failures = new List<Failure>();
                if (Target.HasDriverLicense && Target.Age < 16)
                {
                    failures.Add(new Failure(this));
                }

                return failures;
            }
        }
