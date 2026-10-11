using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace TextileShop.Web.Localization;

/// <summary>Localizes field names, default validation messages and model-binding messages.</summary>
public sealed class LocalizedMvcOptionsSetup : IConfigureOptions<MvcOptions>
{
    private readonly IStringLocalizer<SharedResource> localizer;

    public LocalizedMvcOptionsSetup(IStringLocalizer<SharedResource> localizer)
    {
        this.localizer = localizer;
    }

    public void Configure(MvcOptions options)
    {
        options.ModelMetadataDetailsProviders.Add(new DisplayNameProvider(localizer));
        options.ModelMetadataDetailsProviders.Add(new DefaultValidationMessageProvider());

        var messages = options.ModelBindingMessageProvider;
        messages.SetValueMustNotBeNullAccessor(_ => localizer["A value is required."]);
        messages.SetMissingBindRequiredValueAccessor(name => localizer["A value for the '{0}' parameter or property was not provided.", name]);
        messages.SetMissingKeyOrValueAccessor(() => localizer["A value is required."]);
        messages.SetAttemptedValueIsInvalidAccessor((value, name) => localizer["The value '{0}' is not valid for {1}.", value, name]);
        messages.SetUnknownValueIsInvalidAccessor(name => localizer["The value provided for {0} is invalid.", name]);
        messages.SetValueIsInvalidAccessor(value => localizer["The value '{0}' is invalid.", value]);
        messages.SetValueMustBeANumberAccessor(name => localizer["The field {0} must be a number.", name]);
        messages.SetNonPropertyAttemptedValueIsInvalidAccessor(value => localizer["The value '{0}' is not valid.", value]);
        messages.SetNonPropertyUnknownValueIsInvalidAccessor(() => localizer["The provided value is invalid."]);
        messages.SetNonPropertyValueMustBeANumberAccessor(() => localizer["The field must be a number."]);
    }

    /// <summary>Field labels come from [Display(Name)] or the property name; both are looked up in the resources.</summary>
    private sealed class DisplayNameProvider : IDisplayMetadataProvider
    {
        private readonly IStringLocalizer<SharedResource> localizer;

        public DisplayNameProvider(IStringLocalizer<SharedResource> localizer)
        {
            this.localizer = localizer;
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context.Key.MetadataKind != ModelMetadataKind.Property || context.Key.Name is null)
            {
                return;
            }

            var display = context.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
            var key = display?.Name ?? Humanize(context.Key.Name);
            context.DisplayMetadata.DisplayName = () => localizer[key].Value;
        }

        private static string Humanize(string propertyName)
        {
            var words = Regex.Replace(propertyName, "(?<=[a-z0-9])(?=[A-Z])", " ");
            return words.Length <= 1 ? words : words[..1] + words[1..].ToLowerInvariant();
        }
    }

    /// <summary>
    /// Gives validation attributes without a message their English default as an explicit message,
    /// so the data-annotation localizer can look it up in the resources.
    /// </summary>
    private sealed class DefaultValidationMessageProvider : IValidationMetadataProvider
    {
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var attribute in context.ValidationMetadata.ValidatorMetadata.OfType<ValidationAttribute>())
            {
                if (!string.IsNullOrEmpty(attribute.ErrorMessage) || !string.IsNullOrEmpty(attribute.ErrorMessageResourceName))
                {
                    continue;
                }

                attribute.ErrorMessage = attribute switch
                {
                    RequiredAttribute => "The {0} field is required.",
                    StringLengthAttribute { MinimumLength: > 0 } => "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.",
                    StringLengthAttribute => "The field {0} must be a string with a maximum length of {1}.",
                    RangeAttribute => "The field {0} must be between {1} and {2}.",
                    EmailAddressAttribute => "The {0} field is not a valid e-mail address.",
                    PhoneAttribute => "The {0} field is not a valid phone number.",
                    _ => null
                };
            }
        }
    }
}
