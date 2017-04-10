using Portfolio.Data;
using System;

namespace Portfolio.Model.Entities
{
    public class Employment : IEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CompanyURL { get; set; }

        public string ImageURL { get; set; }

        public DateTime? EmploymentStartDate { get; set; }

        public DateTime? EmploymentEndDate { get; set; }
    }
}