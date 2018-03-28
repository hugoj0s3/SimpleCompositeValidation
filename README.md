# SimpleValidationComposite 
 Simple validation library based on composition. You create single and simple validations, and then you add those validations to validate an entire model. 
  

## Compositing:
```csharp
     CompositeValidation<Person> Validation = new CompositeValidation<Person>()
                .NotNull(nameof(Person.FirstName), x => x.FirstName) 
                .MinimumLength(nameof(Person.FirstName), x => x.FirstName, 3) 
                .MaximumLength(nameof(Person.FirstName), x => x.FirstName, 10) 
                .Email(nameof(Person.Email), x => x.Email) 
                .RegEx(nameof(Person.Phone), x => x.Phone, @"^[0-9\-\+]{9,15}$") 
                .MustNot(nameof(Person.BirthDate), x => x.BirthDate, x => x.Year < 1850) 
                .Must(nameof(Person.BirthDate), x => x.BirthDate, x => x < DateTime.Now) 
                .Add(new CustomValidation(nameof(Person)), x => x); 

            if (!Validation.Update(person).IsValid)
            {
                IReadOnlyCollection<Failure> failures = Validation.Failures;
            }
```


## Creating a custom validation:
```csharp
        public class CustomValidation : Validation<Person>
        {
            private const string message = "A person who are under 16 can not have driver license";
            public CustomValidation(
                string groupName, 
                Person target = null) 
                : base(groupName, message, target, 1)
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
```
