
namespace EnglishMaster.Client.Pages
{
    public partial class Dictionary : PageBase
    {
        protected override async Task OnInitializedAsync()
        {
            await RunActionWithLoading(async () =>
            {
                await Task.Delay(1000);
            });
        }
    }
}
