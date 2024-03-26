namespace EnglishMaster.Shared
{
    public static class ApplicationSettings
    {
        public static ApplicationMode Mode = ApplicationMode.Dev;
        public static byte[] JWT_KEY = new byte[64];

        //DO NOT CHANGE THIS ID
        public const string APPLICATION_ID = "7bc622cfe27a4a9d90314625a82b673b";
    }
}
