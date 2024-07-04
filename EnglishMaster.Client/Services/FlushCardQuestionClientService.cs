using System.Net.Http.Json;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Client.Services;

public sealed class FlushCardQuestionClientService
{
    private readonly ILogger<FlushCardQuestionClientService> _logger;
    private readonly HttpClient _httpClient;

    public FlushCardQuestionClientService(ILogger<FlushCardQuestionClientService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<FlushCardResponseDto>> GetFlushCardsAsync()
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(ApiEndPoint.FLUSH_QUESTION);
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
        }
        List<FlushCardResponseDto>? questions =
            await httpResponseMessage.Content.ReadFromJsonAsync<List<FlushCardResponseDto>>();
        if (questions == null)
        {
            throw new Exception($"Can not deserialized.{nameof(List<FlushCardResponseDto>)}");
        }
        if (questions.Count == 0)
        {
            throw new Exception("Ooops. Unexpected error. Question count is Zero.");
        }
        return questions;
    }
}
