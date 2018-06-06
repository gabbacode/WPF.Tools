using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Kanban.Desktop.Issues.Model
{
    public class IssueModel : IIssueModel
    {
        public IssueModel(IRedmineRepository redmineRepository)
        {
            RedmineRepository = redmineRepository;
        }

        public async Task<Issue> LoadOrCreateAsync(int? issueId)
        {
            if (issueId.HasValue)
            {
                // Load
                var getIssueParameters = new NameValueCollection();
                getIssueParameters.Add(Keys.IssueId, issueId.ToString());

                return (await RedmineRepository
                    .GetIssuesAsync(getIssueParameters))
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
