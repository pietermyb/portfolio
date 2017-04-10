using Portfolio.Data.Base;
using Portfolio.Data.Context;
using Portfolio.Model.Entities;

namespace Portfolio.Data.Repository
{
    public class ProjectRepository : EntityBaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(PortfolioContext context)
            : base(context)
        { }
    }
}