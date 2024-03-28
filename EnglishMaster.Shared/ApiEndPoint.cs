namespace EnglishMaster.Shared
{
    public static class ApiEndPoint
    {
        private const string BASE_PATH_V1 = "/api/v1/";
        public const string USER = $"{BASE_PATH_V1}users";
        public const string DICTIONARY = $"{BASE_PATH_V1}dictionaries";
        public const string LEVEL = $"{BASE_PATH_V1}levels";
        public const string LOGIN = $"{BASE_PATH_V1}login";
        public const string PART_OF_SPEECH = $"{BASE_PATH_V1}part-of-speeches";
        public const string QUESTION = $"{BASE_PATH_V1}questions";
        public const string ACHIEVEMENT = $"{BASE_PATH_V1}achievmenets";

    }
}