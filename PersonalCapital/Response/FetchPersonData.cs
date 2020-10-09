using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class FetchPersonData
    {
        [JsonProperty(PropertyName = "id")] public long Id { get; set; }

        [JsonProperty(PropertyName = "phone")] public string Phone { get; set; }

        [JsonProperty(PropertyName = "name")] public PersonName Name { get; set; }

        [JsonProperty(PropertyName = "birthday")]
        public PersonDate Birthday { get; set; }

        [JsonProperty(PropertyName = "otherGovernmentIdDescription")]
        public string OtherGovernmentIdDescription { get; set; }

        [JsonProperty(PropertyName = "countryOfGovtIdIssue")]
        public string CountryOfGovtIdIssue { get; set; }

        [JsonProperty(PropertyName = "employerName")]
        public string EmployerName { get; set; }

        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }

        [JsonProperty(PropertyName = "natureOfBusiness")]
        public string NatureOfBusiness { get; set; }

        [JsonProperty(PropertyName = "numberOfYearsEmployed")]
        public string NumberOfYearsEmployed { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "countryOfCitizenship")]
        public string CountryOfCitizenship { get; set; }

        [JsonProperty(PropertyName = "countryOfBirth")]
        public string CountryOfBirth { get; set; }

        [JsonProperty(PropertyName = "countryOfResidency")]
        public string CountryOfResidency { get; set; }

        [JsonProperty(PropertyName = "natureOfAffiliationSelf")]
        public string NatureOfAffiliationSelf { get; set; }

        [JsonProperty(PropertyName = "natureOfAffiliationRelationship")]
        public string NatureOfAffiliationRelationship { get; set; }

        [JsonProperty(PropertyName = "natureOfAffiliationName")]
        public string NatureOfAffiliationName { get; set; }

        [JsonProperty(PropertyName = "secAffiliatedFirmName")]
        public string SecAffiliatedFirmName { get; set; }

        [JsonProperty(PropertyName = "secAffiliatedEmployeeYN")]
        public string SecAffiliatedEmployee { get; set; }

        [JsonProperty(PropertyName = "controlPersonYN")]
        public string ControlPerson { get; set; }

        [JsonProperty(PropertyName = "controlPersonFirmName")]
        public string ControlPersonFirmName { get; set; }

        [JsonProperty(PropertyName = "taxFilingStatus")]
        public string TaxFilingStatus { get; set; }

        [JsonProperty(PropertyName = "secondPersonContributing")]
        public bool SecondPersonContributing { get; set; }

        [JsonProperty(PropertyName = "retirementAge")]
        public int RetirementAge { get; set; }

        [JsonProperty(PropertyName = "retirementHorizon")]
        public string RetirementHorizon { get; set; }

        [JsonProperty(PropertyName = "secAffilatedPersonAndFirmName")]
        public string SecAffilatedPersonAndFirmName { get; set; }

        [JsonProperty(PropertyName = "employeeAnotherIBD")]
        public string EmployeeAnotherIbd { get; set; }

        [JsonProperty(PropertyName = "relatedEmployeeAnotherIBD")]
        public string RelatedEmployeeAnotherIbd { get; set; }

        [JsonProperty(PropertyName = "ibdNameEmployedAtOther")]
        public string IbdNameEmployedAtOther { get; set; }

        [JsonProperty(PropertyName = "ibdNameRelatedtoEmployee")]
        public string IbdNameRelatedtoEmployee { get; set; }

        [JsonProperty(PropertyName = "relatedOtherIBDFirstName")]
        public string RelatedOtherIbdFirstName { get; set; }

        [JsonProperty(PropertyName = "relatedOtherIBDLastName")]
        public string RelatedOtherIbdLastName { get; set; }

        [JsonProperty(PropertyName = "relatedOtherIBDRelationship")]
        public string RelatedOtherIbdRelationship { get; set; }

        [JsonProperty(PropertyName = "maritalStatus")]
        public string MaritalStatus { get; set; }

        [JsonProperty(PropertyName = "jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty(PropertyName = "targetPortfolioSource")]
        public string TargetPortfolioSource { get; set; }

        [JsonProperty(PropertyName = "targetPortfolioAccuracy")]
        public decimal TargetPortfolioAccuracy { get; set; }

        [JsonProperty(PropertyName = "targetPortfolioDetermined")]
        public bool TargetPortfolioDetermined { get; set; }

        [JsonProperty(PropertyName = "notCloseToRetirement")]
        public bool NotCloseToRetirement { get; set; }

        [JsonProperty(PropertyName = "networth")]
        public PersonNetworth Networth { get; set; }

        [JsonProperty(PropertyName = "hasAdvisedAccounts")]
        public string HasAdvisedAccounts { get; set; }

        [JsonProperty(PropertyName = "secAffiliatedEmployeeFamily")]
        public string SecAffiliatedEmployeeFamily { get; set; }

        [JsonProperty(PropertyName = "isSpouseDefaultBeneficiary")]
        public bool IsSpouseDefaultBeneficiary { get; set; }

        [JsonProperty(PropertyName = "relationship")]
        public string Relationship { get; set; }

        [JsonProperty(PropertyName = "isClientAgreementSigned")]
        public bool IsClientAgreementSigned { get; set; }

        [JsonProperty(PropertyName = "retired")]
        public bool Retired { get; set; }

        [JsonProperty(PropertyName = "ageAsInt")]
        public int AgeAsInt { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "userRetirementRange")]
        public string UserRetirementRange { get; set; }

        [JsonProperty(PropertyName = "secAffiliatedPersonFullName")]
        public string SecAffiliatedPersonFullName { get; set; }

        [JsonProperty(PropertyName = "userInputIncomeValue")]
        public decimal UserInputIncomeValue { get; set; }

        [JsonProperty(PropertyName = "age")] public decimal Age { get; set; }

        [JsonProperty(PropertyName = "userRiskTolerance")]
        public string UserRiskTolerance { get; set; }

        [JsonProperty(PropertyName = "isRetired")]
        public bool IsRetired { get; set; }

        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }
    }

    public class PersonName
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "salutation")]
        public string Salutation { get; set; }

        [JsonProperty(PropertyName = "suffix")]
        public string Suffix { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
    }

    public class PersonDate
    {
        [JsonProperty(PropertyName = "year")] public string Year { get; set; }

        [JsonProperty(PropertyName = "month")] public string Month { get; set; }

        [JsonProperty(PropertyName = "day")] public string Day { get; set; }

        [JsonProperty(PropertyName = "empty")] public bool Empty { get; set; }
    }

    public class PersonNetworth
    {
        [JsonProperty(PropertyName = "calculatedValuePreference")]
        public string CalculatedValuePreference { get; set; }

        [JsonProperty(PropertyName = "userInputNetworth")]
        public decimal UserInputNetworth { get; set; }

        [JsonProperty(PropertyName = "netWorth")]
        public decimal NetWorth { get; set; }
    }
}