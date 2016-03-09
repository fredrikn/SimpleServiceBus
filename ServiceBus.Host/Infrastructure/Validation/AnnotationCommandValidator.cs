namespace ServiceBus.Infrastructure.Validation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;

    /// <summary>
    /// Validator for property annotations.
    /// </summary>
    internal class AnnotationCommandValidator : ICommandValidator
    {
        /// <summary>
        /// Validates all properties with annotations.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="validationPath">The validation path option.</param>
        public void Validate(object objectToValidate, ValidationPath validationPath = ValidationPath.Flat)
        {
            var validationExceptions = new List<ValidationException>();

            // The HashSet for the validatedObjects parameter is used to make sure not the same object is validated.
            // Some object use circular references and that can lead to a stack overflow exception.
            Validate(objectToValidate, validationExceptions, new HashSet<object>(EqualityComparer<object>.Default), validationPath);
        }


        private static void Validate(
                                     object objectToValidate,
                                     ICollection<ValidationException> validationExceptions,
                                     ISet<object> validatedObjects,
                                     ValidationPath validationPath)
        {
            if (!validatedObjects.Add(objectToValidate) || objectToValidate == null)
                return;

            foreach (var property in objectToValidate.GetType().GetProperties())
            {
                if (HaveValidationAttributes(property) || IsDataContractMember(property))
                {
                    if (!property.CanRead)
                        throw new InvalidOperationException(
                            $"Can't use ValidationAttributes on Property '{property.Name}' because it doesn't have a get");

                    ValidatePropertyValue(property, objectToValidate);
                }

                if (IsObject(property) && validationPath == ValidationPath.Deep)
                {
                    ValidateObject(objectToValidate, validationExceptions, property, validatedObjects);
                }
            }
        }


        private static void ValidatePropertyValue(PropertyInfo property, object objectToValidate)
        {
            var validationAttributes = property
                .GetCustomAttributes(typeof(ValidationAttribute), true)
                .OfType<ValidationAttribute>()
                .ToArray();

            if (validationAttributes.Length <= 0)
                return;

            var value = property.GetValue(objectToValidate, null);

            foreach (var validationAttribute in validationAttributes)
                validationAttribute.Validate(value, property.Name);
        }


        private static bool IsObject(PropertyInfo property)
        {
            return !property.PropertyType.IsValueType && !property.PropertyType.IsAssignableFrom(typeof(string));
        }


        private static void ValidateObject(
                                           object objectToValidate,
                                           ICollection<ValidationException> validationExceptions,
                                           PropertyInfo property,
                                           ISet<object> validatedObjects)
        {
            var value = property.GetValue(objectToValidate, null);

            var nestedItems = (value is IEnumerable && !(value is string)) ? (IEnumerable)value : new[] { value };

            foreach (var item in nestedItems)
                Validate(item, validationExceptions, validatedObjects, ValidationPath.Deep);
        }

        private static bool IsDataContractMember(PropertyInfo property)
        {
            return property.GetCustomAttributes(true).Any(a => a is MessageBodyMemberAttribute || a is MessageHeaderAttribute);
        }

        private static bool HaveValidationAttributes(PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(ValidationAttribute), true).Any();
        }
    }
}