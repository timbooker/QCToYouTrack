using System;
using System.Linq;
using PandocWrapper;
using QCAPI.Repositories;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;

namespace QCToYouTrack
{
    public class Class1
    {
        public void Go()
        {
            var rep = new BugRepository();
            foreach (var issue in rep.GetBugs().Select(bug => new Issue()
                                                                {
                                                                    Summary = bug.Summary,
                                                                    Description = PandocHelper.Convert(bug.Description).Replace("<br />", string.Empty),
                                                                    Assignee = "Unassigned",
                                                                    State = MapBugStatus(bug.Status),
                                                                    Type = "Bug",
                                                                    ProjectShortName = "EJ"
                                                                }))
            {
                var conn = new Connection("localhost", 2669);
                conn.Authenticate("root", "10Pounds");

                var issueManagement = new IssueManagement(conn);

                issueManagement.CreateIssue(issue);
            }
        }

        public void AddComments()
        {
            var rep = new BugRepository();
            foreach (var issue in rep.GetBugs().Select(bug => new Issue()
            {
                Summary = bug.Summary,
                Description = PandocHelper.Convert(bug.Description).Replace("<br />", string.Empty),
                Assignee = "Unassigned",
                State = MapBugStatus(bug.Status),
                Type = "Bug",
                ProjectShortName = "EJ"
            }))
            {
                var conn = new Connection("localhost", 2669);
                conn.Authenticate("root", "10Pounds");

                var issueManagement = new IssueManagement(conn);

                issueManagement.CreateIssue(issue);
            }
        }

        public void DeleteAll()
        {
            var conn = new Connection("localhost", 2669);
            conn.Authenticate("root", "10Pounds");

            var issueManagement = new IssueManagement(conn);

            foreach (var issueId in issueManagement.GetAllIssuesForProject("EJ").Select(x => x.Id))
            {
                var conn1 = new Connection("localhost", 2669);
                conn1.Authenticate("root", "10Pounds");
                var issueManagement1 = new IssueManagement(conn1);

                issueManagement1.ApplyCommand(issueId, "remove", string.Empty);
            }
        }

        public string MapBugStatus(string bugState)
        {
            return bugState == "Closed" ? "Verified" : "Submitted";
        }
    }
}