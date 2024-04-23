using System.Collections;
using System.Collections.Generic;

namespace Proyect_two.Pages.Clases_Utiles
{
    public class ListaEnlazadaSimple : IEnumerable<Nodo>
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

        // Implementación del método GetEnumerator para la interfaz IEnumerable
        public IEnumerator<Nodo> GetEnumerator()
        {
            Nodo actual = PrimerNodo;
            while (actual != null)
            {
                yield return actual;
                actual = actual.Referencia;
            }
        }

        // Implementación del método GetEnumerator para la interfaz IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Aquí puedes agregar más métodos para eliminar, buscar, o insertar nodos en posiciones específicas.
    }
}
