using System;

namespace Proyect_two.Pages.Clases_Utiles
{
    public class ListaEnlazadaSimple
    {
        public Nodo PrimerNodo { get; set; }
        public Nodo UltimoNodo { get; set; }

        public ListaEnlazadaSimple()
        {
            PrimerNodo = null;
            UltimoNodo = null;
        }

        public bool ListaVacia()
        {
            return PrimerNodo == null;
        }

        public void AgregarAlFinal(object informacion)
        {
            Nodo nuevoNodo = new Nodo(informacion);
            if (ListaVacia())
            {
                PrimerNodo = nuevoNodo;
                UltimoNodo = nuevoNodo;
            }
            else
            {
                UltimoNodo.Referencia = nuevoNodo;
                UltimoNodo = nuevoNodo;
            }
        }

        public void AgregarAlInicio(object informacion)
        {
            Nodo nuevoNodo = new Nodo(informacion);
            if (ListaVacia())
            {
                PrimerNodo = nuevoNodo;
                UltimoNodo = nuevoNodo;
            }
            else
            {
                nuevoNodo.Referencia = PrimerNodo;
                PrimerNodo = nuevoNodo;
            }
        }

        // Aquí puedes agregar más métodos para eliminar, buscar, o insertar nodos en posiciones específicas.
    }
}
