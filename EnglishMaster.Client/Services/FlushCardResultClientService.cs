using System.Net.Http.Json;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;

namespace EnglishMaster.Client.Services;

public sealed class FlushCardResultClientService
{
    private readonly ILogger<FlushCardResultClientService> _logger;
    private readonly HttpClient _httpClient;
    public FlushCardResultClientService(ILogger<FlushCardResultClientService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task SubmitResultAsync(long wordId, bool isCorrect)
    {
        FlushCardResultRequestDto flushCardResultRequestDto = new FlushCardResultRequestDto(wordId, isCorrect);
        await _httpClient.PostAsJsonAsync(ApiEndPoint.FLUSH_RESULT, flushCardResultRequestDto);
    }
}