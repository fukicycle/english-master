namespace EnglishMaster.Client.Pages
{
    public partial class Home : PageBase
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
