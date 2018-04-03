using SimpleCompositeValidation.Base;

namespace SimpleCompositeValidation
{
    public class Failure
    {
		/// <summary>
		/// Validation that generates this failure.
		/// </summary>
        public IValidation Validation { get; }

		/// <summary>
		/// Creates a failure with the default values pre defined on validation passed.
		/// </summary>
		/// <param name="validation">Validation that generates this failure.</param>
		public Failure(IValidation validation)
        {
            Validation = validation;
            Message = validation.Message;
        }

		/// <summary>
		/// Creates a failure with the default values predefined on validation passed except the formatMessage.
		/// </summary>
		/// <param name="validation">Validation that generates this failure.</param>
		/// <param name="message">FormatMessage of the failure</param>
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