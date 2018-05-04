using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using System.Collections.Specialized;
using System.Linq;

namespace Kanban.Desktop.Issues.Model
{
    public class IssueModel : IIssueModel
    {
        public IssueModel(IRedmineRepository redmineRepository)
        {
            RedmineRepository = redmineRepository;
        }

        public Issue LoadOrCreate(int? issueId)
        {
            if (issueId.HasValue)
            {
                // Load
                var getIssueParameters = new NameValueCollection();
                getIssueParameters.Add(Keys.IssueId, issueId.ToString());

                return RedmineRepository
                    .GetIssues(getIssueParameters)
                    .First();
            }
            else
            {
                // create new 
                return new Issue
                {

                };
            }
        }

        public IRedmineRepository RedmineRepository { get; }

    }
}
