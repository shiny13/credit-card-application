using System;
namespace CreditCardApplication
{
    public class FrequentFlyerNumberValidatorService : IFrequentFlyerNumberValidator
    {
        public FrequentFlyerNumberValidatorService()
        {
        }

        bool IFrequentFlyerNumberValidator.IsValid(string frequentFlyerNumber)
        {
            throw new NotImplementedException();
        }

        public event EventHandler ValidatorLookupPerformed;

        void IFrequentFlyerNumberValidator.IsValid(string frequentFlyerNumber, out bool isValid)
        {
            throw new NotImplementedException();
        }

        //public string LicenseKey
        //{
        //    get
        //    {
        //        throw new NotImplementedException("For demo purposes.");
        //    }
        //}

        public IServiceInformation ServiceInformation => throw new NotImplementedException();

        public ValidationMode ValidationMode
        {
            get => throw new NotImplementedException("For demo purposes");
            set => throw new NotImplementedException("For demo purposes");
        }
    }
}
