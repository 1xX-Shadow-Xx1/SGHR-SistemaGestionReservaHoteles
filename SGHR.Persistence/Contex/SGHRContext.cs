using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Entities.Configuration.Usuers;


namespace SGHR.Persistence.Contex
{
    public class SGHRContext : DbContext
    {
        public SGHRContext(DbContextOptions<SGHRContext> options) : base(options) { }

        //Habitaciones
        public DbSet<Amenity> Amenity { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Piso> Piso { get; set; }
        
        //Operaciones
        public DbSet<CheckInOut> CheckInOut { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<Auditory> Auditory { get; set; }
        public DbSet<Mantenimiento> Mantenimiento { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        //Reservas
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ServicioAdicional> ServicioAdicional { get; set; }
        public DbSet<Tarifa> Tarifa { get; set; }
        
        //Users
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        
    }
}
