namespace EnglishMaster.Shared.Models
{
    public partial class RoomUser
    {
        public long Id { get; set; }

        public long RoomId { get; set; }

        public long UserId { get; set; }

        public virtual Room Room { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}