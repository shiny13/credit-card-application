using System;
namespace CreditCardApplication
{
    public class FraudLookup
    {
        public bool IsFraudRisk(CreditCardApplication application)
        {
            return CheckApplication(application);
        }

        protected virtual bool CheckApplication(CreditCardApplication application)
        {
            if (application.Lastname == "Smith")
            {
                return true;
            }

            return false;
        }
    }
}
