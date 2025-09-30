
using SGHR.Domain.Entities.Configuration;

namespace SGHR.Domain.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
