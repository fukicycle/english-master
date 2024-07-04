using Toolbelt.Blazor.SpeechSynthesis;

namespace EnglishMaster.Client.Services
{
    public sealed class SpeakService
    {
        private readonly ILogger<SpeakService> _logger;
        private readonly SpeechSynthesis _speechSynthesis;
        private SpeechSynthesisVoice? _voice;

        public SpeakService(ILogger<SpeakService> logger, SpeechSynthesis speechSynthesis)
        {
            _logger = logger;
            _speechSynthesis = speechSynthesis;
        }

        public async Task Speak(string text)
        {
            if (_voice == null) return;// if silent mode is enabled, this is null value.
            var utterancet = new SpeechSynthesisUtterance
            {
                Text = text,
                Lang = "en-US", // BCP 47 language tag
                Pitch = 1.0, // 0.0 ~ 2.0 (Default 1.0)
                Rate = 1.0, // 0.1 ~ 10.0 (Default 1.0)
                Volume = 1.0, // 0.0 ~ 1.0 (Default 1.0)
                Voice = _voice
            };
            await _speechSynthesis.SpeakAsync(utterancet);
        }

        public async Task<IEnumerable<SpeechSynthesisVoice>> GetVoicesAsync()
        {
            IEnumerable<SpeechSynthesisVoice> voices = await _speechSynthesis.GetVoicesAsync();
            return voices.Where(a => a.Lang == "en-US");
        }

        public async Task SetVoiceAsync(string? voice)
        {
            if (string.IsNullOrEmpty(voice))
            {
                _voice = null;
            }
            else
            {
                var voices = await GetVoicesAsync();
                _voice = voices.First(a => a.VoiceIdentity == voice);
            }
        }
    }
}
