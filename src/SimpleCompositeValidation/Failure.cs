using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation
{
    public class Failure
    {
        public IValidation Validation { get; }

        public Failure(IValidation validation)
        {
            Validation = validation;
            Message = validation.Message;
        }

        public Failure(IValidation validation, string message)
            : this(validation)
        {
            Message = message;
        }


        public string GroupName => Validation.GroupName;
        public string Message { get; }
        public int Severity => Validation.Severity;
    }
}