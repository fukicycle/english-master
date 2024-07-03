using System;
using System.Collections.Generic;

namespace EnglishMaster.Server.Models;

public partial class AccessToken
{
    public string Token { get; set; } = null!;

    public long UserId { get; set; }

    public DateTime Expires { get; set; }

    public virtual User User { get; set; } = null!;
}
