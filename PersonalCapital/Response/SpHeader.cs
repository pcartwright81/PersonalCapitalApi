using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    // 2. Header Section
    public record SpHeader(
        [property: JsonProperty("SP_HEADER_VERSION")] int SpHeaderVersion,
        [property: JsonProperty("userStage")] string UserStage,
        [property: JsonProperty("isDelegate")] bool IsDelegate,
        [property: JsonProperty("userGuid")] string UserGuid,
        [property: JsonProperty("participantContext")] ParticipantContext ParticipantContext,
        [property: JsonProperty("betaTester")] bool BetaTester,
        [property: JsonProperty("accountsMetaData")] List<string> AccountsMetaData,
        [property: JsonProperty("tenantName")] string TenantName,
        [property: JsonProperty("success")] bool Success,
        [property: JsonProperty("qualifiedLead")] bool QualifiedLead,
        [property: JsonProperty("developer")] bool Developer,
        [property: JsonProperty("personId")] long PersonId,
        [property: JsonProperty("authLevel")] string AuthLevel,
        [property: JsonProperty("username")] string Username,
        [property: JsonProperty("status")] string Status
    );
}