namespace ServiceBus.Infrastructure.Validation
{
    /// <summary>
    /// The commandValidator interface.
    /// </summary>
    public interface ICommandValidator
    {
        /// <summary>
        /// Validates all properties with annotations.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="validationPath">The validation path option. Default is Flat</param>
        void Validate(
            object objectToValidate,
            ValidationPath validationPath = ValidationPath.Flat);
    }
}