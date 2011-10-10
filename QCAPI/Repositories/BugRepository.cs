using System;
using System.Collections.Generic;
using System.Linq;
using QCAPI.Model;
using QCAPI.QCModel;
using Simple.Data;

namespace QCAPI.Repositories
{
    public class BugRepository : IBugRepository
    {
        public IEnumerable<Bug> GetBugs()
        {
            var db = Database.Default;
            IEnumerable<QCBug> bugs = db.td.BUG.FindAll(db.td.BUG.bg_Bug_Id == 5002).Cast<QCBug>();

            return bugs.Select(MapToBug());
        }

        public IEnumerable<Bug> GetBugsBy(Func<Bug, bool> pred)
        {
            // this'll do for now.
            return GetBugs().Where(pred);
        }

        public Func<QCBug, Bug> MapToBug()
        {
            return x => new Bug
                            {
                                Description = x.bg_Description,
                                Status = x.bg_Status,
                                Comments = BugHelper.ParseComments(x.bg_Dev_Comments),
                                Summary = x.bg_Summary
                            };
        }
    }
}
