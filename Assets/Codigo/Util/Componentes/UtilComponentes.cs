using UnityEngine;
using System.Collections.Generic;

public class UtilComponentes {

    /// <summary>
    /// Obtiene todos los componentes del tipo indicado que sean descendencia (hijos, hijos de hijos, etc.) del GameObject pasado por parámetro.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject">GameObject padre</param>
    /// <returns>Lista de todos los componentes</returns>
	public static List<T> getComponentesEnDescendencia<T>(GameObject gameObject){
		List<T> componentes = new List<T> ();
		getComponentesRecursivo (gameObject.transform, componentes);
		return componentes;
	}
	
    /// <summary>
    /// Función privada utilizada por la función getComponentesEnDescendencia
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform">Transform actual</param>
    /// <param name="componentes">Lista a la que se van agregando todos los componentes</param>
	private static void getComponentesRecursivo<T>(Transform transform, List<T> componentes){
		T[] comps = transform.GetComponents<T> ();
		if (comps.Length > 0) {
			foreach (T comp in comps) {
				componentes.Add (comp);
			}
		}
		foreach (Transform thijo in transform) {
			getComponentesRecursivo (thijo, componentes);
		}
	}

    public static T getCrearComponenteRequerido<T>(GameObject gameObject) where T:Component
    {
        T comp = gameObject.GetComponent<T>();
        if(comp == null)
        {
            comp = gameObject.AddComponent<T>();
        }
        return comp;
    }

}