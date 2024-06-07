using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class TreeFarmResponseDto
    {
        public TreeFarmResponseDto(DateTime dateTime)
        {
            DateTime = dateTime;
        }
        public DateTime DateTime { get; }
    }
}
