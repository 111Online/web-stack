using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Extensions;

namespace NHS111.Domain.Repository
{
    public class VersionRepository
    {
        public VersionRepository(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public async Task<VersionInfo> GetInfo()
        {
            return await _graphRepository.Client.Cypher
                .Return(v => v.As<VersionInfo>())
                .ResultsAsync
                .FirstOrDefault();
        }

        private readonly IGraphRepository _graphRepository;
    }

    public interface IVersionRepository
    {
        Task<VersionInfo> GetInfo();
    }
}
