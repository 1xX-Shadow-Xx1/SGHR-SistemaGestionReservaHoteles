using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Base
{
    public abstract class BaseEntity : BaseAuditory
    {
        public int Id { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

    }
}
