
namespace EnglishMaster.Client.Pages
{
    public partial class Study : PageBase
    {
        protected override async Task OnInitializedAsync()
        {
            await RunActionWithLoading(async () =>
            {
                await Task.Delay(2000);
            });
        }
    }
}
