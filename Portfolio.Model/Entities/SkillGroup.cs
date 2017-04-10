using Portfolio.Data;
using System.Collections.Generic;

namespace Portfolio.Model.Entities
{
    public class SkillGroup : IEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }
        
        public ICollection<Skill> Skills { get; set; }
    }
}