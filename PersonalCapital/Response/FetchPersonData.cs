﻿﻿﻿using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record FetchPersonData(
    [property: JsonProperty(PropertyName = "id")] long Id,
    [property: JsonProperty(PropertyName = "phone")] string Phone,
    [property: JsonProperty(PropertyName = "name")] PersonName Name,
    [property: JsonProperty(PropertyName = "birthday")] PersonDate Birthday,
    [property: JsonProperty(PropertyName = "otherGovernmentIdDescription")] string OtherGovernmentIdDescription,
    [property: JsonProperty(PropertyName = "countryOfGovtIdIssue")] string CountryOfGovtIdIssue,
    [property: JsonProperty(PropertyName = "employerName")] string EmployerName,
    [property: JsonProperty(PropertyName = "occupation")] string Occupation,
    [property: JsonProperty(PropertyName = "natureOfBusiness")] string NatureOfBusiness,
    [property: JsonProperty(PropertyName = "numberOfYearsEmployed")] string NumberOfYearsEmployed,
    [property: JsonProperty(PropertyName = "gender")] string Gender,
    [property: JsonProperty(PropertyName = "countryOfCitizenship")] string CountryOfCitizenship,
    [property: JsonProperty(PropertyName = "countryOfBirth")] string CountryOfBirth,
    [property: JsonProperty(PropertyName = "countryOfResidency")] string CountryOfResidency,
    [property: JsonProperty(PropertyName = "natureOfAffiliationSelf")] string NatureOfAffiliationSelf,
    [property: JsonProperty(PropertyName = "natureOfAffiliationRelationship")] string NatureOfAffiliationRelationship,
    [property: JsonProperty(PropertyName = "natureOfAffiliationName")] string NatureOfAffiliationName,
    [property: JsonProperty(PropertyName = "secAffiliatedFirmName")] string SecAffiliatedFirmName,
    [property: JsonProperty(PropertyName = "secAffiliatedEmployeeYN")] string SecAffiliatedEmployee,
    [property: JsonProperty(PropertyName = "controlPersonYN")] string ControlPerson,
    [property: JsonProperty(PropertyName = "controlPersonFirmName")] string ControlPersonFirmName,
    [property: JsonProperty(PropertyName = "taxFilingStatus")] string TaxFilingStatus,
    [property: JsonProperty(PropertyName = "secondPersonContributing")] bool SecondPersonContributing,
    [property: JsonProperty(PropertyName = "retirementAge")] int RetirementAge,
    [property: JsonProperty(PropertyName = "retirementHorizon")] string RetirementHorizon,
    [property: JsonProperty(PropertyName = "secAffilatedPersonAndFirmName")] string SecAffilatedPersonAndFirmName,
    [property: JsonProperty(PropertyName = "employeeAnotherIBD")] string EmployeeAnotherIbd,
    [property: JsonProperty(PropertyName = "relatedEmployeeAnotherIBD")] string RelatedEmployeeAnotherIbd,
    [property: JsonProperty(PropertyName = "ibdNameEmployedAtOther")] string IbdNameEmployedAtOther,
    [property: JsonProperty(PropertyName = "ibdNameRelatedtoEmployee")] string IbdNameRelatedtoEmployee,
    [property: JsonProperty(PropertyName = "relatedOtherIBDFirstName")] string RelatedOtherIbdFirstName,
    [property: JsonProperty(PropertyName = "relatedOtherIBDLastName")] string RelatedOtherIbdLastName,
    [property: JsonProperty(PropertyName = "relatedOtherIBDRelationship")] string RelatedOtherIbdRelationship,
    [property: JsonProperty(PropertyName = "maritalStatus")] string MaritalStatus,
    [property: JsonProperty(PropertyName = "jobTitle")] string JobTitle,
    [property: JsonProperty(PropertyName = "targetPortfolioSource")] string TargetPortfolioSource,
    [property: JsonProperty(PropertyName = "targetPortfolioAccuracy")] decimal TargetPortfolioAccuracy,
    [property: JsonProperty(PropertyName = "targetPortfolioDetermined")] bool TargetPortfolioDetermined,
    [property: JsonProperty(PropertyName = "notCloseToRetirement")] bool NotCloseToRetirement,
    [property: JsonProperty(PropertyName = "networth")] PersonNetworth Networth,
    [property: JsonProperty(PropertyName = "hasAdvisedAccounts")] string HasAdvisedAccounts,
    [property: JsonProperty(PropertyName = "secAffiliatedEmployeeFamily")] string SecAffiliatedEmployeeFamily,
    [property: JsonProperty(PropertyName = "isSpouseDefaultBeneficiary")] bool IsSpouseDefaultBeneficiary,
    [property: JsonProperty(PropertyName = "relationship")] string Relationship,
    [property: JsonProperty(PropertyName = "isClientAgreementSigned")] bool IsClientAgreementSigned,
    [property: JsonProperty(PropertyName = "retired")] bool Retired,
    [property: JsonProperty(PropertyName = "ageAsInt")] int AgeAsInt,
    [property: JsonProperty(PropertyName = "emailAddress")] string EmailAddress,
    [property: JsonProperty(PropertyName = "userRetirementRange")] string UserRetirementRange,
    [property: JsonProperty(PropertyName = "secAffiliatedPersonFullName")] string SecAffiliatedPersonFullName,
    [property: JsonProperty(PropertyName = "userInputIncomeValue")] decimal UserInputIncomeValue,
    [property: JsonProperty(PropertyName = "age")] decimal Age,
    [property: JsonProperty(PropertyName = "userRiskTolerance")] string UserRiskTolerance,
    [property: JsonProperty(PropertyName = "isRetired")] bool IsRetired,
    [property: JsonProperty(PropertyName = "countryCode")] string CountryCode
);

public record PersonName(
    [property: JsonProperty(PropertyName = "firstName")] string FirstName,
    [property: JsonProperty(PropertyName = "middleName")] string MiddleName,
    [property: JsonProperty(PropertyName = "salutation")] string Salutation,
    [property: JsonProperty(PropertyName = "suffix")] string Suffix,
    [property: JsonProperty(PropertyName = "lastName")] string LastName
);

public record PersonDate(
    [property: JsonProperty(PropertyName = "year")] string Year,
    [property: JsonProperty(PropertyName = "month")] string Month,
    [property: JsonProperty(PropertyName = "day")] string Day,
    [property: JsonProperty(PropertyName = "empty")] bool Empty
);

public record PersonNetworth(
    [property: JsonProperty(PropertyName = "calculatedValuePreference")] string CalculatedValuePreference,
    [property: JsonProperty(PropertyName = "userInputNetworth")] decimal UserInputNetworth,
    [property: JsonProperty(PropertyName = "netWorth")] decimal NetWorth
);