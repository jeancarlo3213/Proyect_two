namespace Proyect_two.Pages.Clases_Utiles
{
    public class SolicitudService
    {
        private ListaEnlazadaSimple listaSolicitudes;

        public SolicitudService()
        {
            listaSolicitudes = new ListaEnlazadaSimple();
        }

        public void AgregarSolicitud(Solicitud nuevaSolicitud)
        {
            listaSolicitudes.AgregarAlFinal(nuevaSolicitud);
        }

        public Solicitud BuscarSolicitudPorId(int id)
        {
            Nodo nodoActual = listaSolicitudes.PrimerNodo;
            while (nodoActual != null)
            {
                if (((Solicitud)nodoActual.Informacion).Id == id)
                {
                    return (Solicitud)nodoActual.Informacion;
                }
                nodoActual = nodoActual.Referencia;
            }
            return null;
        }
        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorCliente(int idCliente)
        {
            ListaEnlazadaSimple solicitudesCliente = new ListaEnlazadaSimple();
            Nodo nodoActual = listaSolicitudes.PrimerNodo;

            while (nodoActual != null)
            {
                if (((Solicitud)nodoActual.Informacion).IdCliente == idCliente)
                {
                    solicitudesCliente.AgregarAlFinal(nodoActual.Informacion);
                }
                nodoActual = nodoActual.Referencia;
            }

            return solicitudesCliente;
        }

        public void ActualizarEstadoSolicitud(int id, string nuevoEstado)
        {
            Nodo nodoActual = listaSolicitudes.PrimerNodo;
            while (nodoActual != null)
            {
                if (((Solicitud)nodoActual.Informacion).Id == id)
                {
                    ((Solicitud)nodoActual.Informacion).Estado = nuevoEstado;
                    return;
                }
                nodoActual = nodoActual.Referencia;
            }
        }

        public void EliminarSolicitud(int id)
        {
            Nodo nodoActual = listaSolicitudes.PrimerNodo;
            Nodo nodoAnterior = null;

            while (nodoActual != null)
            {
                if (((Solicitud)nodoActual.Informacion).Id == id)
                {
                    if (nodoAnterior != null)
                    {
                        nodoAnterior.Referencia = nodoActual.Referencia;
                        if (nodoActual.Referencia == null)
                        {
                            listaSolicitudes.UltimoNodo = nodoAnterior;
                        }
                    }
                    else
                    {
                        listaSolicitudes.PrimerNodo = nodoActual.Referencia;
                        if (listaSolicitudes.PrimerNodo == null)
                        {
                            listaSolicitudes.UltimoNodo = null;
                        }
                    }
                    return;
                }
                nodoAnterior = nodoActual;
                nodoActual = nodoActual.Referencia;
            }
        }

       

    }
}
