namespace SGHR.Domain.Base
{
    public abstract class BaseEntity : BaseAuditory
    {
        public int Id { get; set; }
        public bool Eliminado { get; set; } = false;

    }
}
