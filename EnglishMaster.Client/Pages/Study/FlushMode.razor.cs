using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages.Study;

public partial class FlushMode
{

    [Parameter]
    public bool AutoStart { get; set; } = false;

    private QuestionResponseDto? _question = new QuestionResponseDto(1, 1, "Word", 1, 1, Enumerable.Empty<AnswerResponseDto>());
    private bool _isShowAnswered = false;
}
