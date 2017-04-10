using Portfolio.Data.Base;
using Portfolio.Data.Context;
using Portfolio.Model.Entities;

namespace Portfolio.Data.Repository
{
    public class EmploymentRepository : EntityBaseRepository<Employment>, IEmploymentRepository
    {
        public EmploymentRepository(PortfolioContext context)
            : base(context)
        { }
    }
}