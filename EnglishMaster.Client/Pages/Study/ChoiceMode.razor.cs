using EnglishMaster.Shared.Dto.Response;
using Fukicycle.Tool.AppBase.Components.Dialog;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages.Study;
public partial class ChoiceMode
{
    [Parameter]
    public long LevelId { get; set; } = 0;

    [Parameter]
    public long PartOfSpeechId { get; set; } = 0;

    [Parameter]
    public bool AutoStart { get; set; } = false;

    private long _partOfSpeechId = 0;
    private long _levelId = 0;
    private QuestionResponseDto? _question = null;
    private List<PartOfSpeechResponseDto> _partOfSpeeches = new List<PartOfSpeechResponseDto>();
    private List<LevelResponseDto> _levles = new List<LevelResponseDto>();
    private bool _isAnswered = false;
    private bool _isCorrect = false;
    private int _questionNumber = 0;
    private bool _isMute = false;

    protected override async Task OnInitializedAsync()
    {
        StateContainer.IsLoading = true;
        UserSettings userSettings = await SettingService.LoadAsync();
        _isMute = string.IsNullOrEmpty(userSettings.VoiceIdentity);
        await SpeakService.SetVoiceAsync(userSettings.VoiceIdentity);
        _partOfSpeeches = await ExecuteAsync(PartOfSpeechClientService.GetPartOfSpeechesAsync);
        _levles = await ExecuteAsync(LevelClientService.GetLevelsAsync);
        StateContainer.IsLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        _partOfSpeechId = PartOfSpeechId;
        _levelId = LevelId;
        if (AutoStart)
        {
            await StartButtonOnClick();
        }
    }

    private async Task StartButtonOnClick()
    {
        StateContainer.IsLoading = true;
        QuestionClientService.SetPartOfSpeechId(_partOfSpeechId);
        QuestionClientService.SetLevelId(_levelId);
        await ExecuteAsync(QuestionClientService.InitializeAsync);
        _question = QuestionClientService.GetQuestion(out _questionNumber);
        await SpeakService.Speak(_question?.Word ?? "");
        StateContainer.IsLoading = false;
    }

    private void OptionButtonOnClick(long wordId, string answerMeaning)
    {
        if (_question == null)
        {
            StateContainer.DialogContent = new DialogContent("Unexpected error has occured.", DialogType.Error);
            return;
        }
        _isCorrect = QuestionClientService.Verify(wordId, answerMeaning);
        _isAnswered = true;
    }

    private async Task NextButtonOnClick()
    {
        _isAnswered = false;
        _question = QuestionClientService.GetQuestion(out _questionNumber);
        if (_question == null)
        {
            NavigationManager.NavigateTo($"result?level={_levelId}&part-of-speech={_partOfSpeechId}");
        }
        else
        {
            await SpeakService.Speak(_question.Word);
        }
    }

    private async Task SoundButtonOnClick()
    {
        if (_question == null)
        {
            StateContainer.DialogContent = new DialogContent("Unexpected error has occured.", DialogType.Error);
            return;
        }
        await SpeakService.Speak(_question.Word);
    }
}
