namespace EnglishMaster.Shared.Models
{
    public partial class User
    {
        public long Id { get; set; }

        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Password { get; set; }

        public string? Nickname { get; set; }

        public string? IconUrl { get; set; }

        public virtual ICollection<AccessToken> AccessTokens { get; set; } = new List<AccessToken>();

        public virtual ICollection<MeaningOfWordLearningHistory> MeaningOfWordLearningHistories { get; set; } = new List<MeaningOfWordLearningHistory>();

        public virtual ICollection<RoomUser> RoomUsers { get; set; } = new List<RoomUser>();
    }
}