using System.Configuration;

namespace TestApplication {
    public static class Settings {
        public static string Username {
            get {
                var username = ConfigurationManager.AppSettings["username"];
                return string.IsNullOrEmpty(username) ? null : username;
            }
        }
        public static string SessionFile {
            get {
                var sessionFile = ConfigurationManager.AppSettings["sessionFile"];
                return string.IsNullOrEmpty(sessionFile) ? null : sessionFile;
            }
        }
        public static string KeystoreTarget {
            get {
                var keystoreTarget = ConfigurationManager.AppSettings["keystoreTarget"];
                return string.IsNullOrEmpty(keystoreTarget) ? null : keystoreTarget;
            }
        }
    }
}
