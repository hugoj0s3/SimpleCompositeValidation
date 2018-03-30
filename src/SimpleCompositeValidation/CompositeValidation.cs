using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation
{
    /// <summary>
    /// Composite Validation class. Updates the validations added, keeping a list of failures from those validations.
    /// </summary>
    /// <typeparam name="T">Type of the Target that will be validated</typeparam>
    public class CompositeValidation<T> : Validation<T> 
    {
        private readonly IDictionary<Type, IList<FuncValidation>> _validations 
            = new Dictionary<Type, IList<FuncValidation>>();

		/// <summary>
		/// Creates composite validation with a summary message that will be inserted in the top of failures list.
		/// </summary>
		/// <param name="target">Target to be validated</param>
		/// <param name="summaryMessage">Message that will be inserted as the first item in case of failure</param>
		public CompositeValidation(T target, string summaryMessage)
            : base(typeof(T).Name, summaryMessage, target)
        {
        }

		/// <summary>
		/// Creates composite validation without a summary message, choosing this constructor no message will be inserted in the top of failures list.
		/// </summary>
		/// <param name="target">Target to be validated</param>
		public CompositeValidation(T target)
            : this(target, null)
        {
        }
		/// <summary>
		///  Creates composite validation with a summary message that will be inserted in the top of failures list. 
		///  The target be initialized with the default value(default(T)).
		/// </summary>
		/// <param name="summaryMessage">Message that will be inserted as the first item in case of failure</param>
		public CompositeValidation(string summaryMessage)
            : this(default(T), summaryMessage)
        {
        }

		/// <summary>
		///  Creates composite validation without a summary message, choosing this constructor no message will be inserted in the top of failures list.
		///  The target be initialized with the default value(default(T)).
		/// </summary>
		public CompositeValidation()
            : this(default(T))
        {
        }

        /// <summary>
        /// Add a validation in the composition
        /// </summary>
        /// <typeparam name="TMember">type of the member</typeparam>
        /// <param name="validation">validation to validate this member</param>
        /// <param name="member">Path to obtain the member</param>
        /// <param name="stopIfInvalid">pass true to stop if it failed on this validation. default is false</param>
        /// <returns></returns>
        public CompositeValidation<T> Add<TMember>(
            IValidation<TMember> validation, 
            Func<T, TMember> member,
            bool stopIfInvalid = false)
        {
            if (!_validations.TryGetValue(typeof(TMember), out var validations))
            {
                validations = new List<FuncValidation>();
                _validations.Add(typeof(TMember), validations);
            }

            var funcValidation = new FuncValidation(validation, x => member.Invoke(x), x => validation.Update((TMember)x), stopIfInvalid);
            validations.Add(funcValidation);

            return this;
        }

		/// <summary>
		/// Update partially according with group name passed.
		/// </summary>
		/// <typeparam name="TMember">Type of the member</typeparam>
		/// <param name="groupName">Group name</param>
		/// <param name="value">value to be validated</param>
		/// <returns>Itself</returns>
		public IValidation<T> Update<TMember>(string groupName, TMember value)
        {
			
            var failures = Failures.Where(x => x.GroupName != groupName).ToList();

	        bool noFailuresBefore = !failures.Any();

            Update(_validations[typeof(TMember)]
                .Where(x => x.Validation.GroupName == groupName), failures, item => value, false);

	        if (failures.Any() && HasSummaryMessage && noFailuresBefore)
	        {
		        failures.Insert(0, new Failure(this));
	        }

			Failures = new ReadOnlyCollection<Failure>(failures);

            return this;
        }

		/// <summary>
		/// Update partially according with group name passed.
		/// </summary>
		/// <typeparam name="TMember">Type of the member</typeparam>
		/// <param name="groupName">Group name</param>
		/// <returns>Itself</returns>
		public IValidation<T> Update<TMember>(string groupName)
        {
            var failures = Failures.Where(x => x.GroupName != groupName).ToList();

	        var noFailuresBefore = !failures.Any();

			Update(_validations[typeof(TMember)]
                .Where(x => x.Validation.GroupName == groupName), failures);

            Failures = new ReadOnlyCollection<Failure>(failures);

	        if (failures.Any() && HasSummaryMessage && noFailuresBefore)
	        {
		        failures.Insert(0, new Failure(this));
	        }
			return this;
        }

        /// <summary>
        /// Validations added
        /// </summary>
        public IReadOnlyCollection<IValidation> Validations =>
            _validations.Values
                .SelectMany(x => x)
                .Select(x => x.Validation)
                .ToList().AsReadOnly();

        public bool HasSummaryMessage => !string.IsNullOrEmpty(Message);

        public string SummaryMessage => Message;

        /// <summary>
        /// Validates the target and return the list of failures
        /// It inserts a failure in the top of the list if there is a failure in any of validations added and the summary message was set in the constructor
        /// </summary>
        /// <returns>a list of failures, it might be empty if there are no fails</returns>
        protected override IList<Failure> Validate()
        {

            var failures = new List<Failure>();

            Update(_validations.Values.SelectMany(x => x), failures);

            if (failures.Any() && Message != null)
            {
                failures.Insert(0, new Failure(this));
            }

            return failures;
        }

        private void Update(IEnumerable<FuncValidation> validations, List<Failure> failures) 
        {
            Update(validations, failures, item => item.MemberFunc.Invoke(Target));
        }

        private void Update(IEnumerable<FuncValidation> validations, List<Failure> failures,
            Func<FuncValidation, object> func, bool verifyTargetNull = true)
        {
            if (Target == null && verifyTargetNull)
            {
                return;
            }

            foreach (var item in validations)
            {
                var targetMember = func.Invoke(item);
                item.UpdateAction.Invoke(targetMember);
                var itemFailures = item.Validation.Failures;

                if (!(item.Validation.IsValid) && item.StopIfInvalid)
                {
                    break;
                }

                failures.AddRange(itemFailures);
            }
        }

        private class FuncValidation
        {
            public FuncValidation(
                IValidation validation, 
                Func<T, object> memberFunc,
                Action<object> updateAction,
                bool stopIfInvalid)
            {
                Validation = validation;
                MemberFunc = memberFunc;
                StopIfInvalid = stopIfInvalid;
                UpdateAction = updateAction;
            }
            public IValidation Validation { get; }
            public Func<T, object> MemberFunc { get; }
            public Action<object> UpdateAction { get; }
            public bool StopIfInvalid { get; }
        }
    }

}
