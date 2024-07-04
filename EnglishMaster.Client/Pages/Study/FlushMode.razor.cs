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
    private bool _isAuthenticated = false;

    private List<FlushCardResponseDto> _questions = new List<FlushCardResponseDto>();

    protected override void OnInitialized()
    {
        _isAuthenticated = AuthenticationStateProvider.LoginUser != null;
    }

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
            if (_question != null && _isAuthenticated)
            {
                await FlushCardResultClientService.SubmitResultAsync(_question.WordId, isOk);
            }
            _question = _questions[_idx];
        }
        _isShowAnswered = false;
    }
}
