namespace Portfolio.API.Dto
{
    /// <summary>
    /// Skill Dto
    /// </summary>
    public class SkillDto
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
        /// Gets or sets the skill group identifier.
        /// </summary>
        /// <value>
        /// The skill group identifier.
        /// </value>
        public int SkillGroupId { get; set; }
    }
}
