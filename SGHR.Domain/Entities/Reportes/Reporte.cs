namespace SGHR.Domain.Entities.Reporte
{
    public sealed class Reporte : Base.BaseEntity
    {
        //Constructor de Reporte
        public Reporte(string tipoReporte, int generadoPor,string rutaArchivo)
        {
            this.tipoReporte = tipoReporte;
            this.generadoPor = generadoPor;
            this.rutaArchivo = rutaArchivo;
        }

        //Logica de Reporte


        //Metodos para Modificar los atributos de Reporte
        public void CambiarTipoReporte(string nuevoTipo,int usuarioID)
        {
            tipoReporte = nuevoTipo;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarRutaReporte(string nuevaRuta, int usuarioID)
        {
            rutaArchivo = nuevaRuta;
            RegistrarModificacion(usuarioID);
        }

        //Atributos de Reporte
        public string tipoReporte { get; private set; }
        public int generadoPor { get; }
        public string rutaArchivo { get; private set; }
    }
}
