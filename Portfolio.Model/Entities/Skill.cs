using Portfolio.Data;

namespace Portfolio.Model.Entities
{
    public class Skill : IEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int SkillGroupId { get; set; }

        public SkillGroup SkillGroup { get; set; }
    }
}