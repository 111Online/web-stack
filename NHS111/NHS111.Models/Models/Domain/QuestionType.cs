using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Domain
{
    public enum QuestionType
    {
        Boolean = 2,
        Date = 5,
        Integer = 4,
        String = 8,
        Text = 9,
        Choice = 11,
        Telephone = 17
    }

}
