using Microsoft.EntityFrameworkCore;
using Portfolio.Data.Base;
using Portfolio.Data.Context;
using Portfolio.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Portfolio.Data.Repository
{
    public class SkillGroupRepository : EntityBaseRepository<SkillGroup>, ISkillGroupRepository
    {
        private PortfolioContext _context;

        public SkillGroupRepository(PortfolioContext context)
            : base(context)
        { _context = context; }

        public SkillGroup GetSkillGroupWithSkills(int id)
        {
            return _context.SkillGroups
                .Include(s => s.Skills)
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<SkillGroup> GetAllSkillGroupsWithSkills()
        {
            return _context.SkillGroups
                .Include(s => s.Skills);
        }
    }
}