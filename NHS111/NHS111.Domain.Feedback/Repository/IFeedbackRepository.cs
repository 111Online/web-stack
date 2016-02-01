using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;

namespace NHS111.Domain.Feedback.Repository
{
    public interface IFeedbackRepository
    {
        Task<int> Add(Models.Models.Domain.Feedback feedback);
        Task<IEnumerable<Models.Models.Domain.Feedback>> List();
    }
}
