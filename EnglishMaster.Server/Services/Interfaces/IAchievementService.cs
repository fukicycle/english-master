﻿using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IAchievementService
    {
        IList<AchievementResponseDto> GetAchievementResponseDtosByEmail(string email);
    }
}