using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation
{
    public class CompositeValidation<T> : Validation<T> 
    {
        private readonly IDictionary<Type, IList<FuncValidation>> _validations 
            = new Dictionary<Type, IList<FuncValidation>>();

        public CompositeValidation(T target)
            : base(typeof(T).Name, null, target)
        {

        }

        public CompositeValidation()
            : this(default(T))
        {

        }

        public Validation<T> Add<TMember>(
            IValidation<TMember> validation, 
            Func<T, TMember> func,
            bool stopIfInvalid = false)
        {
            if (!_validations.TryGetValue(typeof(TMember), out var validations))
            {
                validations = new List<FuncValidation>();
                _validations.Add(typeof(TMember), validations);
            }

            var funcValidation = new FuncValidation(validation, x => func.Invoke(x), x => validation.Update((TMember)x), stopIfInvalid);
            validations.Add(funcValidation);

            return this;
        }

        protected override IList<Failure> Validate()
        {

            var failures = new List<Failure>();
         
            Update(_validations.Values.SelectMany(x => x), failures);

            return failures;
        }

        public IValidation<T> Update<TMember>(string groupName, TMember value)
        {
            var failures = this.Failures.Where(x => x.GroupName != groupName).ToList();

            Update(_validations[typeof(TMember)]
                .Where(x => x.Validation.GroupName == groupName), failures, item => value, false);

            this.Failures = new ReadOnlyCollection<Failure>(failures);

            return this;
        }

        public IValidation<T> Update<TMember>(string groupName)
        {
            var failures = this.Failures.Where(x => x.GroupName != groupName).ToList();
            Update(_validations[typeof(TMember)]
                .Where(x => x.Validation.GroupName == groupName), failures);

            this.Failures = new ReadOnlyCollection<Failure>(failures);

            return this;
        }

        

        private void Update(IEnumerable<FuncValidation> validations, List<Failure> failures) 
        {
            Update(validations, failures, item => item.MemberFunc.Invoke(this.Target));
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
