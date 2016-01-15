using System;
using System.Collections.Generic;

namespace OnboardingConsumer.Models
{
    public class ClientDetails
    {
        //Identification
        public int ReykerId { get; set; }
        public string ExternalId { get; set; }

        //Personal Details

        public string Title { get; set; }
        public string Forenames { get; set; }
        public string Surname { get; set; }
        public string CountryOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Suffix { get; set; }
        public string NationalInsuranceNumber { get; set; }

        //Telephone Numbers

        public List<TelephoneNumber> TelephoneNumbers { get; set; }

        //Addresses 

        public List<ClientAddress> Addresses { get; set; }

        //Bank account details

        public ClientBankAccount BankAccount { get; set; }

        //Citizenship Details

        public List<ClientCitizenship> Citizenships { get; set; }

        //AML
        public ClientAml AML { get; set; }

        //Plan
        public List<ClientPlan> Plans { get; set; }

        //Error
        public string ErrorMessage { get; set; }
    }

    public class ClientPlan
    {
        public string Provider { get; set; }
        public string PlanType { get; set; }
        public int? PlanTypeId { get; set; }
        public string PlanName { get; set; }
        public string ExternalPlanId { get; set; }
    }

    public class TelephoneNumber
    {
        public string Number { get; set; }

        public string DialingCode { get; set; }

        public string TelephoneType { get; set; }

        public int TelephoneTypeId { get; set; }
    }

    public class ClientAddress
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public int CountryId { get; set; }

        public string AddressType { get; set; }

        public int AddressTypeId { get; set; }
    }

    public class ClientCitizenship
    {
        public string CountryOfResidency { get; set; }
        public string TaxIdentificationNumber { get; set; }
    }

    public class ClientBankAccount
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
    }

    public class ClientAml
    {
        public string AMLStatus { get; set; }
        public DateTime? AMLDate { get; set; }
        public string AMLScore { get; set; }
    }

    public class PlanTypeDetails
    {
        public int PlanTypeId { get; set; }
        public string PlanType { get; set; }
    }

    public class CountryDetails
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class DialingCodeDetails
    {
        public int DialingCodeId { get; set; }
        public string Location { get; set; }
        public string LocationFull { get; set; }
        public string IDC { get; set; }
    }

}