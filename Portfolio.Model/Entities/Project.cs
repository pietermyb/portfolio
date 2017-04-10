using Portfolio.Data;

namespace Portfolio.Model.Entities
{
    public class Project : IEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }
    }
}