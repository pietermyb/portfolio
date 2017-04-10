using Portfolio.Model.Entities;
using System.Collections.Generic;

namespace Portfolio.API.Dto
{
    /// <summary>
    /// Skill Group Dto
    /// </summary>
    public class SkillGroupDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public IList<Skill> Skills { get; set; }
    }
}
