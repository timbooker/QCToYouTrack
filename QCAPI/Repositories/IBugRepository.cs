using System.Collections.Generic;
using QCAPI.Model;

namespace QCAPI.Repositories
{
    public interface IBugRepository
    {
        IEnumerable<Bug> GetBugs();
    }
}