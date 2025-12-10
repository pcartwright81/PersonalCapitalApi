using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public record ParticipantContext(
        [property: JsonProperty("indId")] string IndId,
        [property: JsonProperty("sponsorAcCuCode")] string SponsorAcCuCode,
        [property: JsonProperty("participantRegistrationId")] string ParticipantRegistrationId,
        [property: JsonProperty("empowerSponsorId")] string EmpowerSponsorId,
        [property: JsonProperty("empowerPersonId")] string EmpowerPersonId,
        [property: JsonProperty("participantHostName")] string ParticipantHostName,
        [property: JsonProperty("empowerSponsorName")] string EmpowerSponsorName
    );
}