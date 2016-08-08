using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace OnboardingConsumer.Models
{
    [Serializable]
    public class OnboardingClientDetails
    {
        //Personal Details
        [Required]
        public string Title { get; set; }
        [Required]
        public string Forenames { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int CountryOfBirth { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string EmailType { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

        //Additional Personal Details
        public string Suffix { get; set; }
        public string NationalInsuranceNumber { get; set; }

        // Primary Address
        [Required]
        public OnboardingPrimaryAddress PrimaryAddress { get; set; }

        //Additional Addresses 
        public List<OnboardingAddress> AdditionalAddresses { get; set; }

        //Primary Telephone
        [Required]
        public OnboardingTelephoneNumber PrimaryTelephone { get; set; }

        //Additional Telephone
        public List<OnboardingTelephoneNumber> AdditionalTelephone { get; set; }

        //Bank account details
        [Required]
        public OnboardingBankAccount BankAccount { get; set; }

        //Citizenship Details
        [Required]
        public OnboardingCitizenship PrimaryCitizenship { get; set; }

        public List<OnboardingCitizenship> AdditionalCitizenship { get; set; }

        //External Provider Info
        [Required]
        public string ExternalCustomerId { get; set; }
        [Required]
        public string ExternalPlanId { get; set; }

        [Required]
        public int PlanType { get; set; }

    }

    public class OnboardingAddress
    {
        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }

        public string County { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public int Country { get; set; }
        [Required]
        public int AddressType { get; set; }
    }

    public class OnboardingPrimaryAddress
    {
        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }

        public string County { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public int Country { get; set; }
    }

    public class OnboardingTelephoneNumber
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public int DialingCode { get; set; }
        [Required]
        public int TelephoneType { get; set; }
    }

    public class OnboardingBankAccount
    {
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string SortCode { get; set; }
    }

    public class OnboardingCitizenship
    {
        public int CountryOfResidency { get; set; }
        public string TaxIdentificationNumber { get; set; }
    }

}

