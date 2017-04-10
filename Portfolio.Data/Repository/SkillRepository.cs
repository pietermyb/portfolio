using Portfolio.Data.Base;
using Portfolio.Data.Context;
using Portfolio.Model.Entities;

namespace Portfolio.Data.Repository
{
    public class SkillRepository : EntityBaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(PortfolioContext context)
            : base(context)
        { }
    }
}