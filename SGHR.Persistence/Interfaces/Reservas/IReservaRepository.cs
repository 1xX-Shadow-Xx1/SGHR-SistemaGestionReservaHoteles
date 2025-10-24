using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;


namespace SGHR.Domain.Repository
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task<OperationResult<List<Reserva>>> GetByClienteAsync(int idCliente);
        Task<OperationResult<List<Reserva>>> GetByHabitacionAsync(int idHabitacion);
        Task<OperationResult<List<Reserva>>> GetActiveReservationsAsync();
    }
}
