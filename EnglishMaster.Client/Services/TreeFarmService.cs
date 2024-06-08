using Blazored.LocalStorage;
using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class TreeFarmService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TreeFarmService> _logger;
        private const string TREE_FARM_STORAGE_KEY = "TREE_FARM_START_DATE";

        public TreeFarmService(
            ILocalStorageService localStorageService,
            HttpClient httpClient,
            ILogger<TreeFarmService> logger)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> IsEnabledTreeFarmAsync()
        {
            bool exsits = await _localStorageService.ContainKeyAsync(TREE_FARM_STORAGE_KEY);
            return exsits;
        }

        public async Task SetStartDateAsync()
        {
            await _localStorageService.SetItemAsync(TREE_FARM_STORAGE_KEY, DateTime.Today);
        }

        public async Task<int> GetTreeLevelAsync()
        {
            //Sample data
            DateTime startDate = await _localStorageService.GetItemAsync<DateTime>(TREE_FARM_STORAGE_KEY);
            List<TreeFarmResponseDto> treeFarmData = await GetTreeFarmDataAsync(startDate);
            return await Task.Run(() => CalcLevel(treeFarmData.Select(a => a.DateTime).ToList()));
        }

        public string GenerateTreeImagePath(int level)
        {
            return $"process/tree_{level:00}.png";
        }

        public async Task<List<TreeFarmResponseDto>> GetTreeFarmDataAsync(DateTime startDate)
        {
            HttpResponseMessage httpResponseMessage =
                await _httpClient.GetAsync(ApiEndPoint.ACHIEVEMENT + "/tree/farm/data?startDate=" + startDate);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<TreeFarmResponseDto>? treeFarmResponses =
                await httpResponseMessage.Content.ReadFromJsonAsync<List<TreeFarmResponseDto>>();
            if (treeFarmResponses == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<TreeFarmResponseDto>)}");
            }
            return treeFarmResponses;
        }

        private int CalcLevel(List<DateTime> dateTimes)
        {
            int level = 1;
            int numberOfContinue = 0;
            int numberOfDisContinue = 0;
            if (!dateTimes.Any())
            {
                return level;
            }
            DateTime prev = dateTimes.Min();
            foreach (DateTime dt in dateTimes.OrderBy(a => a))
            {
                if (dt == prev.AddDays(1))
                {
                    numberOfContinue++;
                    prev = dt;
                    int prevLevel = level;
                    level = GetLevel(numberOfContinue, level, numberOfDisContinue);
                    if (prevLevel != level)
                    {
                        numberOfContinue = 0;
                        numberOfDisContinue = 0;
                    }
                }
                else
                {
                    numberOfDisContinue = 0;
                    while (dt != prev)
                    {
                        prev = prev.AddDays(1);
                        numberOfDisContinue++;
                    }
                }
            }
            return level;
        }

        private int GetLevel(int numberOfContinue, int currentLevel, int numberOfDisContinue)
        {
            if (numberOfDisContinue == 30)
            {
                return 0;
            }
            //2
            if (numberOfContinue == 2 + numberOfDisContinue)
            {
                if (currentLevel == 0 || currentLevel == 1)
                {
                    return currentLevel + 1;
                }
            }

            //3
            if (numberOfContinue == 3 + numberOfDisContinue)
            {
                if (currentLevel == 2)
                {
                    return currentLevel + 1;
                }
            }

            //4
            if (numberOfContinue == 4 + numberOfDisContinue)
            {
                if (currentLevel == 3 || currentLevel == 4)
                {
                    return currentLevel + 1;
                }
            }

            //5
            if (numberOfContinue == 5 + numberOfDisContinue)
            {
                if (currentLevel == 5)
                {
                    return currentLevel + 1;
                }
            }

            //6
            if (numberOfContinue == 6 + numberOfDisContinue)
            {
                if (currentLevel == 6)
                {
                    return currentLevel + 1;
                }
            }

            //7
            if (numberOfContinue == 7 + numberOfDisContinue)
            {
                if (currentLevel == 7)
                {
                    return currentLevel + 1;
                }
            }

            //8
            if (numberOfContinue == 8 + numberOfDisContinue)
            {
                if (currentLevel == 8)
                {
                    return currentLevel + 1;
                }
            }
            return currentLevel;
        }
    }
}