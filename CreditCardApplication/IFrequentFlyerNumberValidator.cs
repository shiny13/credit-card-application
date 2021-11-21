using System;
namespace CreditCardApplication
{
    public interface ILicenseData
    {
        string LicenseKey { get; }
    }

    public interface IServiceInformation
    {
        ILicenseData License { get; }
    }

    public interface IFrequentFlyerNumberValidator
    {
        bool IsValid(string frequentFlyerNumber);
        void IsValid(string frequentFlyerNumber, out bool isValid);

        //public string LicenseKey { get; }
        IServiceInformation ServiceInformation { get; }

        ValidationMode ValidationMode { get; set; }

        event EventHandler ValidatorLookupPerformed;
    }
}
