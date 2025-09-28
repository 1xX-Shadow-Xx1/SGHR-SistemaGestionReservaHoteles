
using SGHR.Domain.Entities.Configuration;

namespace SGHR.Domain.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string ModificacionRealizada { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioModificacion { get; set; }
        public bool IsDeleted {  get; set; }

        protected DateTime TimeNow()
        {
            return DateTime.Now;
        }
        public void Eliminar(int usuarioid)
        {
            IsDeleted = true;
            FechaModificacion = TimeNow();
        }
        public void RegistrarModificacion(int usuarioid,string modifigicacion)
        {
            FechaModificacion = TimeNow();
            UsuarioModificacion = usuarioid;
            ModificacionRealizada = modifigicacion;
        }
        public void RegistrarCreacion()
        {
            FechaCreacion = TimeNow();
        }
    }
}
