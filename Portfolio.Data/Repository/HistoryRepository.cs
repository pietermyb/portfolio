using Portfolio.Data.Base;
using Portfolio.Data.Context;
using Portfolio.Model.Entities;

namespace Portfolio.Data.Repository
{
    public class HistoryRepository : EntityBaseRepository<History>, IHistoryRepository
    {
        public HistoryRepository(PortfolioContext context)
            : base(context)
        { }
    }
}