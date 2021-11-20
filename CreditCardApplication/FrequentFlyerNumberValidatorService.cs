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

        void IFrequentFlyerNumberValidator.IsValid(string frequentFlyerNumber, out bool isValid)
        {
            throw new NotImplementedException();
        }
    }
}
