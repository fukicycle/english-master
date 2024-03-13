using System;
using System.Collections.Generic;

namespace EnglishMaster.Shared.Models;

public partial class Room
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string Code { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool IsOpen { get; set; }

    public bool IsPrivate { get; set; }

    public virtual ICollection<RoomUser> RoomUsers { get; set; } = new List<RoomUser>();
}
