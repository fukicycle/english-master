namespace EnglishMaster.Client.Pages
{
    public partial class Info : PageBase
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
