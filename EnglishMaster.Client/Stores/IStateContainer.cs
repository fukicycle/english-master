namespace EnglishMaster.Client.Stores
{
    public interface IStateContainer
    {
        event Action OnLoadingStateChanged;
        public bool IsLoading { get; set; }

        event Action OnMessageChanged;
        public string Message { get; set; }

    }
}
