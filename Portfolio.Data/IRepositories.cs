using Portfolio.Data.Repository;
using Portfolio.Model.Entities;

namespace Portfolio.Data
{
    public interface IEmploymentRepository : IEntityBaseRepository<Employment> { }

    public interface IProjectRepository : IEntityBaseRepository<Project> { }

    public interface ISkillGroupRepository : IEntityBaseRepository<SkillGroup> { }

    public interface ISkillRepository : IEntityBaseRepository<Skill> { }

    public interface IHistoryRepository : IEntityBaseRepository<History> { }
}