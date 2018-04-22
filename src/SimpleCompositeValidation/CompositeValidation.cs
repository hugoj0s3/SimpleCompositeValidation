using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleCompositeValidation.Base;
using SimpleCompositeValidation.Exceptions;

namespace SimpleCompositeValidation
{
	/// <inheritdoc cref="ICompositeValidation{T}" />
	public class CompositeValidation<T> : Validation<T>, ICompositeValidation<T>
	{
        private readonly IList<FuncValidation> _validations = new List<FuncValidation>();


		public CompositeValidation(
			T target = default(T),
			string groupName = null,
			string summaryMessage = "")
			: base(groupName ?? typeof(T).Name, target, summaryMessage)
		{
			SummaryMessage = summaryMessage;
		}

		/// <inheritdoc />
		public ICompositeValidation<T> Add<TMember>(
            IValidation<TMember> validation, 
            Func<T, TMember> member)
        {
	        var addOnlyFirstMessage = 
		        (validation as ICompositeValidation<TMember>)?.HasSummaryMessage ?? false;

	        var funcValidation = new FuncValidation
            (
	            validation, 
	            x => member.Invoke(x), 
	            x => validation.Update((TMember)x),
				addOnlyFirstMessage
	        );

            _validations.Add(funcValidation);

            return this;
        }

		/// <inheritdoc />
		public ICompositeValidation<T> AddForEach<TMember>(
		    IValidation<TMember> validation,
		    Func<T, IEnumerable<TMember>> members)
	    {
		    var addOnlyFirstMessage =
			    (validation as ICompositeValidation<TMember>)?.HasSummaryMessage ?? false;

			var funcValidation = new FuncValidation
		    (
			    validation, 
			    x => members.Invoke(x),
			    x =>
			    {
				    ValidateItems(validation, x);
			    },
			    addOnlyFirstMessage

			);
		    _validations.Add(funcValidation);

		    return this;
	    }

		/// <inheritdoc />
		public ICompositeValidation<T> Update<TMember>(string groupName, TMember value)
        {

	        var newFailures = Update(_validations.Where(x => x.Validation.GroupName == groupName), item => value);

	        return UpdateList(groupName, newFailures);
		}

		/// <inheritdoc />
		public ICompositeValidation<T> Update(string groupName)
		{

			var newFailures = Update(_validations.Where(x => x.Validation.GroupName == groupName));

			return UpdateList(groupName, newFailures);
		}

		/// <inheritdoc />
		public ICompositeValidation<T> Update(T target, string groupName)
	    {
		    Target = target;
		    return Update(groupName);
	    }

		/// <inheritdoc />
		public IReadOnlyCollection<IValidation> Validations => 
			_validations
				.Select(x => x.Validation)
				.ToList()
				.AsReadOnly();

		/// <inheritdoc />
		public bool HasSummaryMessage => !string.IsNullOrEmpty(SummaryMessage);

		/// <inheritdoc />
		public string SummaryMessage { get; protected set; }

		/// <inheritdoc />
		public override string Message => SummaryMessage;

		/// <inheritdoc />
		protected override IList<Failure> Validate()
        {

            var failures = Update(_validations);

            if (failures.Any() && HasSummaryMessage)
            {
                failures.Insert(0, new Failure(this));
            }

            return failures;
        }

        private List<Failure> Update(IEnumerable<FuncValidation> validations) 
        {

            return Update(validations, item => Target == null ? null : item.MemberFunc.Invoke(Target));
        }

        private List<Failure> Update(IEnumerable<FuncValidation> validations,
            Func<FuncValidation, object> func)
        {
			var failures = new List<Failure>();

	        var funcValidations = validations.ToList();

	        if (!funcValidations.Any())
	        {
		        throw new ValidationsNotFoundException();
	        }

            foreach (var item in funcValidations)
            {
                var targetMember = func.Invoke(item);
                item.UpdateAction.Invoke(targetMember);
                var itemFailures = item.Validation.Failures;
	            if (item.AddOnlyFirstMessage)
	            {
					failures.Add(itemFailures.FirstOrDefault());
	            }
	            else
	            {
		            failures.AddRange(itemFailures);
				}

			}

	        return failures;
        }
	    private ICompositeValidation<T> UpdateList(string groupName, List<Failure> newFailures)
	    {
		    var oldFailures = Failures.Where(x => x.GroupName != groupName).ToList();

		    if (newFailures.Any() && HasSummaryMessage && !oldFailures.Any())
		    {
			    oldFailures.Insert(0, new Failure(this));
		    }

			Failures = new ReadOnlyCollection<Failure>(oldFailures.Concat(newFailures).ToList());

		    return this;
	    }

	    private static void ValidateItems<TMember>(IValidation<TMember> validation, object items)
	    {
		    foreach (var item in (IEnumerable<TMember>)items)
		    {
			    validation.Update(item);
		    }
	    }

		private class FuncValidation
        {
            public FuncValidation(
                IValidation validation, 
                Func<T, object> memberFunc,
                Action<object> updateAction, 
                bool addOnlyFirstMessage = false)
            {
                Validation = validation;
                MemberFunc = memberFunc;
                UpdateAction = updateAction;
	            AddOnlyFirstMessage = addOnlyFirstMessage;
            }
            public IValidation Validation { get; }
            public Func<T, object> MemberFunc { get; }
            public Action<object> UpdateAction { get; }
			public bool AddOnlyFirstMessage { get; }
        }
    }
}
