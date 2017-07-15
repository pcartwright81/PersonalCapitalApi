namespace PersonalCapital.Api {
    public static class Constants {
        public static class Status {
            public const string Active = "ACTIVE";
            public const string Inactive = "INACTIVE";
        }

        public static class AuthLevel {
            public const string None = "NONE";
            public const string UserIdentified = "USER_IDENTIFIED";
            public const string UserRemembered = "USER_REMEMBERED";
            public const string DeviceAuthorized = "DEVICE_AUTHORIZED";
            public const string SessionAuthenticated = "SESSION_AUTHENTICATED";
        }
    }
}