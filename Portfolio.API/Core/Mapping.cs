using Portfolio.API.Dto;
using Portfolio.Model.Entities;
using System.Linq;

namespace Portfolio.API.Core
{
    /// <summary>
    /// Mapping Extensions
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        /// Maps the specified skill group.
        /// </summary>
        /// <param name="skillGroup">The skill group.</param>
        /// <returns></returns>
        public static SkillGroupDto Map(this SkillGroup skillGroup)
        {
            return (skillGroup == null) ? null : new SkillGroupDto
            {
                Id = skillGroup.Id,
                Name = skillGroup.Name,
                Description = skillGroup.Description,
                Icon = skillGroup.Icon,
                Skills = skillGroup.Skills?.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    SkillGroupId = s.SkillGroupId
                }).ToList()
            };
        }
    }
}
