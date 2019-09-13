namespace FbRestaurantsBot.Core.Helpers
{
    public static class Constants
    {
        public const string FacebookSettings = "FacebookSettings";
        public const string ZomatoSettings = "ZomatoSettings";
        public const string QueryMode = "hub.mode";
        public const string QueryChallenge = "hub.challenge";
        public const string QueryVerifyToken = "hub.verify_token";
        public const string Forbidden = "Forbidden";
        public const string InternalServerError = "Internal Server Error";
        public const string BadRequest = "Bad Request";
        public const string BasicResponseMessage = "Please send your location in order to get nearby restaurants info";
        public const string LocationAttachment = "location";
        public const string WebHookVerified = "WEBHOOK VERIFIED";
        public const string ObjectNotEqualPage = "Object is not equal to page";
        public const string VerificationExceptionMessage = "Unable to verify webhook";
        public const string MessageNotSent = "An error occured when trying to send a response message";
        public const string ApplicationJson = "application/json";
        public const string StringConversionFormat = "0.0000";
        public const string ApiCallExceptionMessage = "Api call failed";
    }
}