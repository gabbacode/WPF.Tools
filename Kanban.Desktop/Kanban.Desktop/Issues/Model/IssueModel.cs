using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Kanban.Desktop.Issues.Model
{
    public class IssueModel : IIssueModel
    {
        public IssueModel(IRepository redmineRepository)
        {
            RedmineRepository = redmineRepository;
        }

        public async Task<Issue> LoadOrCreateAsync(int? issueId)
        {
            if (issueId.HasValue)
            {
                // Load
                return await RedmineRepository.GetIssueAsync(issueId.Value);
            }
            else
            {
                // create new 
                return new Issue
                {

                };
            }
        }

        public IRepository RedmineRepository { get; }

    }
}
