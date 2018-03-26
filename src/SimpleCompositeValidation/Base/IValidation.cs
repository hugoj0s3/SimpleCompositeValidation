using System.Collections.Generic;

namespace SimpleCompositeValidation.Base
{
    public interface IValidation
    {
        int Severity { get; }
        string GroupName { get; }
        string Message { get; }
        IReadOnlyCollection<Failure> Failures { get; }
        bool IsValid { get; }
    }
    public interface IValidation<T> : IValidation
    {
        T Target { get; }
        IValidation<T> Update();
        IValidation<T> Update(T target);
    }
}