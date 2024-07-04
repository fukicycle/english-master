namespace EnglishMaster.Client;

public sealed class UserSettings
{
    public UserSettings(string? voiceIdentity, int numberOfQuestion, long mode)
    {
        VoiceIdentity = voiceIdentity;
        NumberOfQuestion = numberOfQuestion;
        Mode = mode;
    }
    public string? VoiceIdentity { get; set; }
    public int NumberOfQuestion { get; set; }
    public long Mode { get; set; }
}
