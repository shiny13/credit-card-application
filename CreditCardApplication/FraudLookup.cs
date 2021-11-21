using System;
namespace CreditCardApplication
{
    public class FraudLookup
    {
        public virtual bool IsFraudRisk(CreditCardApplication application)
        {
            if (application.Lastname == "Smith")
            {
                return true;
            }

            return false;
        }
    }
}
