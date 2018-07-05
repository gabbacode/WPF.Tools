using System.Threading.Tasks;
using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;

namespace Kanban.Desktop.LocalBase.Issues.Model
{
    public class IssueModel : IIssueModel
    {
        public IssueModel(IRedmineRepository repository)
        {
            SqliteRepository = repository;
        }

        public async Task<Issue> LoadOrCreateAsync(int? issueId)
        {
            if (issueId.HasValue)
            {
                return await SqliteRepository.GetIssueAsync(issueId.Value);
            }
            else
            {
                return new Issue
                {

                };
            }
        }

        public IRedmineRepository SqliteRepository { get; }
    }
}
