﻿using System;
namespace CreditCardApplication
{
    public enum CreditCardApplicationDecision
    {
        Unknown,
        AutoAccepted,
        AutoDeclined,
        ReferredToHuman,
        ReferredToHumanFraudRisk
    }
}
