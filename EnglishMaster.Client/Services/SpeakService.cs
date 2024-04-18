using Toolbelt.Blazor.SpeechSynthesis;

namespace EnglishMaster.Client.Services
{
    public sealed class SpeakService
    {
        private readonly ILogger<SpeakService> _logger;
        private readonly SpeechSynthesis _speechSynthesis;

        public SpeakService(ILogger<SpeakService> logger, SpeechSynthesis speechSynthesis)
        {
            _logger = logger;
            _speechSynthesis = speechSynthesis;
        }

        public async Task Speak(string text)
        {
            var utterancet = new SpeechSynthesisUtterance
            {
                Text = text,
                Lang = "en-US", // BCP 47 language tag
                Pitch = 1.0, // 0.0 ~ 2.0 (Default 1.0)
                Rate = 1.0, // 0.1 ~ 10.0 (Default 1.0)
                Volume = 1.0 // 0.0 ~ 1.0 (Default 1.0)
            };
            await _speechSynthesis.SpeakAsync(utterancet);
        }
    }
}
