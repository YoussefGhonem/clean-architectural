using Elearninig.Base.Application.FluentValidation.Helpers;
using PhoneNumbers;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Elearninig.Base.Application.FluentValidation.Validators;

public static class Validators
{
    /// <summary>
    /// Checks if a email address is valid.
    /// </summary>
    /// <param name="email"></param>
    /// <returns>bool</returns>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('.') || email.Trim().EndsWith(".")) return false;
        try
        {
            var adder = new MailAddress(email);
            return adder.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if a string is a valid http url.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>bool</returns>
    public static bool IsValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                                                 && uriResult.Scheme == Uri.UriSchemeHttp;

    /// <summary>
    /// Checks if zip code is valid in specified country.
    /// </summary>
    /// <param name="zipCode"></param>
    /// <param name="countryCode">Country code to check in ex. 'EG','US',...</param>
    /// <returns>bool</returns>
    public static bool IsValidZipCode(string zipCode, string countryCode)
        => !string.IsNullOrEmpty(zipCode)
           && !string.IsNullOrEmpty(countryCode)
           && CountriesZipCodesMatchers.Data.ContainsKey(countryCode)
           && Regex.Match(CountriesZipCodesMatchers.Data[countryCode], zipCode).Success;

    /// <summary>
    /// Checks if a fax number is valid.
    /// the valid fax number can start with a + sign and contains at least 6 digits
    /// </summary>
    /// <param name="faxNumber"></param>
    /// <returns>bool</returns>
    public static bool IsValidFaxNumber(string faxNumber) =>
        !string.IsNullOrWhiteSpace(faxNumber) && faxNumber.Length is > 8 and < 15 && !faxNumber.Any(char.IsLetter);
    // && Regex.Match(@"/^\+?[0-9]{6,}$/", faxNumber).Success;

    /// <summary>
    /// Checks if phone number is valid in specified country.
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="countryCode">Country code to check in ex. 'EG','US',...</param>
    /// <returns>bool</returns>
    public static bool IsValidPhoneNumber(string phoneNumber, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber)
            || string.IsNullOrWhiteSpace(countryCode)
            || phoneNumber.Trim().StartsWith("-")
            || phoneNumber.Trim().EndsWith("-")
           ) return false;
        try
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            return phoneUtil.IsValidNumberForRegion(phoneUtil.Parse(phoneNumber, countryCode), countryCode);
        }
        catch
        {
            return false;
        }
    }
}