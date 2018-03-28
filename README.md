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


