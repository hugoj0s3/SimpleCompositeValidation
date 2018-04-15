using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleCompositeValidation.Base;
using SimpleCompositeValidation.Exceptions;

namespace SimpleCompositeValidation
{
    /// <typeparam name="T">Type of the Target that will be validated</typeparam>
    /// <summary>
    /// Composite Validation class. Updates the validations added, keeping a list of failures from those validations.
    /// </summary>
    /// <typeparam name="T">Type of the Target that will be validated</typeparam>
    public class CompositeValidation<T> : Validation<T>, ICompositeValidation<T>
	{
        private readonly IList<FuncValidation> _validations = new List<FuncValidation>();

		/// <summary>
		/// Creates composite validation with a summary formatMessage that will be inserted in the top of failures list.
		/// </summary>
		/// <param name="target">Target to be validated</param>
		/// <param name="summaryMessage">Message that will be inserted as the first item in case of failure</param>
		public CompositeValidation(T target, string summaryMessage)
            : base(typeof(T).Name, string.Empty, target)
		{
			SummaryMessage = summaryMessage;
		}

		/// <summary>
		/// Creates composite validation without a summary formatMessage, choosing this constructor no formatMessage will be inserted in the top of failures list.
		/// </summary>
		/// <param name="target">Target to be validated</param>
		public CompositeValidation(T target)
            : this(target, string.Empty)
        {
        }
		/// <summary>
		///  Creates composite validation with a summary formatMessage that will be inserted in the top of failures list. 
		///  The target be initialized with the default value(default(T)).
		/// </summary>
		/// <param name="summaryMessage">FormatMessage that will be inserted as the first item in case of failure</param>
		public CompositeValidation(string summaryMessage)
            : this(default(T), summaryMessage)
        {
        }

		/// <summary>
		///  Creates composite validation without a summary formatMessage, choosing this constructor no formatMessage will be inserted in the top of failures list.
		///  The target be initialized with the default value(default(T)).
		/// </summary>
		public CompositeValidation()
            : this(default(T))
        {
        }

        /// <summary>
        /// Add a validation in the composition
        /// </summary>
        /// <typeparam name="TMember">type of the members</typeparam>
        /// <param name="validation">validation to validate this members</param>
        /// <param name="member">Path to obtain the members</param>
        /// <returns></returns>
        public CompositeValidation<T> Add<TMember>(
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

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TMember"></typeparam>
		/// <param name="validation"></param>
		/// <param name="members"></param>
		/// <returns></returns>
	    public CompositeValidation<T> AddForEach<TMember>(
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

	    /// <summary>
		/// Update partially according with group name passed.
		/// </summary>
		/// <typeparam name="TMember">Type of the members</typeparam>
		/// <param name="groupName">Group name</param>
		/// <param name="value">value to be validated</param>
		/// <returns>Itself</returns>
		public IValidation<T> Update<TMember>(string groupName, TMember value)
        {

	        var newFailures = Update(_validations.Where(x => x.Validation.GroupName == groupName), item => value);

	        return UpdateList(groupName, newFailures);
		}

		/// <summary>
		/// Update partially according with group name passed.
		/// </summary>
		/// <param name="groupName">Group name</param>
		/// <returns>Itself</returns>
		public IValidation<T> Update(string groupName)
		{

			var newFailures = Update(_validations.Where(x => x.Validation.GroupName == groupName));

			return UpdateList(groupName, newFailures);
		}

		/// <summary>
		/// Update partially according with group name and target passed.
		/// </summary>
		/// <param name="target">Target to be validated</param>
		/// <param name="groupName">Group name</param>
		/// <returns>Itself</returns>
		public IValidation<T> Update(T target, string groupName)
	    {
		    Target = target;
		    return Update(groupName);
	    }

		/// <summary>
		/// Validations added
		/// </summary>
		public IReadOnlyCollection<IValidation> Validations => 
			_validations
				.Select(x => x.Validation)
				.ToList()
				.AsReadOnly();

        public bool HasSummaryMessage => !string.IsNullOrEmpty(SummaryMessage);

        public string SummaryMessage { get; protected set; }

	    public override string Message => SummaryMessage;

	    /// <summary>
        /// Validates the target and return the list of failures
        /// It inserts a failure in the top of the list if there is a failure in any of validations added and the summary formatMessage was set in the constructor
        /// </summary>
        /// <returns>a list of failures, it might be empty if there are no fails</returns>
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
	    private IValidation<T> UpdateList(string groupName, List<Failure> newFailures)
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
