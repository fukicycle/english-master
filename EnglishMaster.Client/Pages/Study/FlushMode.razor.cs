using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages.Study;

public partial class FlushMode
{

    [Parameter]
    public bool AutoStart { get; set; } = false;

    private FlushCardResponseDto? _question;
    private bool _isShowAnswered = false;
    private int _idx = 0;

    private List<FlushCardResponseDto> _questions = new List<FlushCardResponseDto>();

    private async Task StartButtonOnClick()
    {
        _questions = await ExecuteAsync(FlushCardQuestionClientService.GetFlushCardsAsync, true);
        _question = _questions[_idx];
    }

    private async Task OKButtonOnClick()
    {
        await SubmitAndNextAsync(true);
    }

    private async Task NGButtonOnClick()
    {
        await SubmitAndNextAsync(false);
    }

    private async Task SubmitAndNextAsync(bool isOk)
    {
        _idx++;
        if (_questions.Count == _idx)
        {
            _idx = 0;
            _question = null;
        }
        else
        {
            _question = _questions[_idx];
        }
        _isShowAnswered = false;
    }
}
