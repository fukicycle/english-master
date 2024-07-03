namespace EnglishMaster.Client;

public sealed class UserSettings
{
    public UserSettings(bool isMute, int numberOfQuestion, long mode)
    {
        IsMute = isMute;
        NumberOfQuestion = numberOfQuestion;
        Mode = mode;
    }
    public bool IsMute { get; set; }
    public int NumberOfQuestion { get; set; }
    public long Mode { get; set; }
}
