using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class AchievementGraphResponseDto
    {
        public AchievementGraphResponseDto(string label, decimal? correctAnswerRate)
        {
            Label = label;
            CorrectAnswerRate = correctAnswerRate;
        }

        public string Label { get; }
        public decimal? CorrectAnswerRate { get; }
    }
}
