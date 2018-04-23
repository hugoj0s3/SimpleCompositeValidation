# Simple Composite Validation 
Lite and simple validation library based on composition. It aims to simplify the composition and reuse of validations logics. You create single and simple validations, and then you add those validations to validate an entire model.

[![Build status](https://ci.appveyor.com/api/projects/status/a2js0psc0gnviccx?svg=true)](https://ci.appveyor.com/project/hugoj0s3/simplecompositevalidation) 

[Complete documentation](https://github.com/hugoj0s3/SimpleCompositeValidation/wiki/1.-Getting-started)

##  Compositing validation:
```csharp
 ICompositeValidation<Person> Validation = new CompositeValidation<Person>()
    .NotNull(nameof(Person.FirstName), x => x.FirstName)
    .MinimumLength(nameof(Person.FirstName), x => x.FirstName, 3)
    .MaximumLength(nameof(Person.FirstName), x => x.FirstName, 10)
    .Email(nameof(Person.Email), x => x.Email)
    .RegEx(nameof(Person.Phone), x => x.Phone, @"^[0-9\-\+]{9,15}$")
    .MustNot(nameof(Person.BirthDate), x => x.BirthDate, x => x.Year < 1850)
    .Must(nameof(Person.BirthDate), x => x.BirthDate, x => x < DateTime.Now)
    .Add(new CustomValidation(nameof(Person)), x => x);    
```

## Validating the model:

```csharp
if (!Validation.Update(person).IsValid)
{
   IReadOnlyCollection<Failure> failures = Validation.Failures;
}
```

## Creating a custom validation:
```csharp
public class CustomValidation : Validation<Person>
{
	private const string DefaultValidationMessage
	    = "{0} - Person under 16 can not have drive license";

	public CustomValidation(
	    string groupName, 
	    Person target = default(Person), 
	    string formatMessage = DefaultValidationMessage, 
	    int severity = 1) 
	    : base(groupName, target, formatMessage, severity)
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
