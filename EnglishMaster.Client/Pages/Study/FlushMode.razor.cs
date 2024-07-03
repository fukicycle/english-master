using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages.Study;

public partial class FlushMode
{
    [Parameter]
    public long LevelId { get; set; } = 0;

    [Parameter]
    public long PartOfSpeechId { get; set; } = 0;

    [Parameter]
    public bool AutoStart { get; set; } = false;

    private long _partOfSpeechId = 0;
    private long _levelId = 0;
    private QuestionResponseDto? _question = new QuestionResponseDto(1, 1, "Word", 1, 1, Enumerable.Empty<AnswerResponseDto>());
    private List<PartOfSpeechResponseDto> _partOfSpeeches = new List<PartOfSpeechResponseDto>();
    private List<LevelResponseDto> _levles = new List<LevelResponseDto>();
    private bool _isAnswered = false;
    private bool _isCorrect = false;
    private int _questionNumber = 0;
    private bool _isMute = false;
}
