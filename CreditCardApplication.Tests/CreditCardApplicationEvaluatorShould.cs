using System;
using Moq;
using Xunit;

namespace CreditCardApplication.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 19 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            // return true when "x" is passed into it
            //mockValidator.Setup(x => x.IsValid("x")).Returns(true);

            // return true when any string param is passed into it
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // return true only when a specific param with valid lambda expression is passed into it
            //mockValidator.Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith("y")))).Returns(true);

            // returns true when a range of data is passed into it
            // Inclusive means including "a" and "z", Exclusive means excluding "a" and "z" in the range
            //mockValidator.Setup(x => x.IsValid(It.IsIn<string>("a", "z", Moq.Range.Inclusive))).Returns(true);

            // returns true when a set of data is passed into it
            //mockValidator.Setup(x => x.IsValid(It.IsIn<string>("x", "y", "z"))).Returns(true);

            // return true when a valid param matches the provided regular expression
            mockValidator.Setup(x => x.IsValid(It.IsRegex("[a-z]"))).Returns(true);
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "y"
            };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplicationsOutDemo()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // setup mock method when using out param
            bool isValid = true;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            // to setup method using ref of an object:
            //var person1 = new Person();
            //var person2 = new Person();
            //var mockGateway = new Mock<IGateway>();
            //mockGateway.Setup(x => x.Execute(ref person1)).Return(-1);
            //var sut = new Processor(mockGateway.Object);
            //sut.Process(person1); //IGateway.Excecute() returns -1
            //sut.Process(person2); //IGateway.Excecute() returns 0 because 0 is default

            // to setup method using ref of an object with It.Ref<>.IsAny:
            //var person1 = new Person();
            //var person2 = new Person();
            //var mockGateway = new Mock<IGateway>();
            //mockGateway.Setup(x => x.Execute(ref It.Ref<Person>.IsAny)).Return(-1);
            //var sut = new Processor(mockGateway.Object);
            //sut.Process(person1); //IGateway.Excecute() returns -1
            //sut.Process(person2); //IGateway.Excecute() returns -1
            //public class Boss : Person {}
            //sut.Process(new Boss()); // IGateway.Execute() return -1

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_000,
                Age = 42
            };

            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            // Setup all the mock objects individually in the heirarchy chain
            //var mockLicenseData = new Mock<ILicenseData>();
            //mockLicenseData.Setup(x => x.LicenseKey).Returns(GetLicenseKeyExpiryString);
            //var mockServiceInfo = new Mock<IServiceInformation>();
            //mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);

            //var mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);
            //mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // Setup just the last method in the heirarchy by accessing all the objects
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns(GetLicenseKeyExpiryString);
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication { Age = 42 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        string GetLicenseKeyExpiryString()
        {
            // Eg. read from vendor-supplied constants file
            return "EXPIRED";
        }

        [Fact]
        public void UseDetailedLookupForOlderApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            // For individually setting up properties to remember
            //mockValidator.SetupProperty(x => x.ValidationMode);

            // To setup all properties to remember in the mock object
            // This method should be called BEFORE any properties are setup for the mock
            mockValidator.SetupAllProperties();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 30 };

            sut.Evaluate(application);

            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }

        [Fact]
        public void ValidateFrequentFlyerNumberForLowIncomeApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                FrequentFlyerNumber = "q"
            };

            sut.Evaluate(application);

            //mockValidator.Verify(x => x.IsValid("q"));

            // Passing a custom error message.
            //mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), "Frequest flyer numbers should be validated.");

            // Specifying a specific number of calls for the method
            //mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Exactly(2));
            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void NotValidateFrequentFlyerNumberForHighIncomeApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 100_000
            };

            sut.Evaluate(application);

            // Specify that the method should never be called.
            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CheckLicenseKeyForLowIncomeApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 99_000
            };

            sut.Evaluate(application);

            // check if the get property is called
            mockValidator.VerifyGet(x => x.ServiceInformation.License.LicenseKey);

            // Check if the get property is called at least once
            mockValidator.VerifyGet(x => x.ServiceInformation.License.LicenseKey, Times.AtLeastOnce);
        }

        [Fact]
        public void SetDetailedLookupForOlderApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 30 };

            sut.Evaluate(application);

            // check if the set property is called
            mockValidator.VerifySet(x => x.ValidationMode = ValidationMode.Detailed);

            // Check if the set property is called at least once
            mockValidator.VerifyGet(x => x.ServiceInformation.License.LicenseKey, Times.AtLeastOnce);
        }
    }
}
