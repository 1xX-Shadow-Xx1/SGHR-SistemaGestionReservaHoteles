
using SGHR.Domain.Entities.Configuration;

namespace SGHR.Domain.Base
{
    public abstract class BaseEntity
    {
        public int ID{ get; set; }
        public bool is_deleted { get; set; }
    }
}
