# Simple Validation Composite 
 Simple validation library based on composition. It aims to simplify the composition and reuse of validations logics. The idea is: You create single and simple validations, and then you add those validations to validate an entire model. 
  
  
[![Build status](https://ci.appveyor.com/api/projects/status/a2js0psc0gnviccx?svg=true)](https://ci.appveyor.com/project/hugoj0s3/simplecompositevalidation)


## Validating a model:
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
Note that NotNull, MinimumLength, MaximumLength, Email, RegEx, MustNot, Must are shortcuts to add specifics kind of validations.


## Creating a custom validation:
```csharp
public class CustomValidation : Validation<Person>
{
		    public CustomValidation(string groupName, string formatMessage, Person target, int severity = 1) 
			    : base(groupName, formatMessage, target, severity)
		    {
		    }

		    public CustomValidation(string groupName, string formatMessage, int severity = 1) 
			    : base(groupName, formatMessage, severity)
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
