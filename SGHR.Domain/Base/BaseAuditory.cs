namespace SGHR.Domain.Base
{
    public abstract class BaseAuditory
    {
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
        public int? SesionCreacionId { get; set; }
        public int? SesionActualizacionId { get; set; }
    }
}
