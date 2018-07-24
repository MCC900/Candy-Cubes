using UnityEngine;
using System.Collections.Generic;

public class UtilComponentes {

	public static List<T> getComponentesEnDescendencia<T>(GameObject gameObject){
		List<T> componentes = new List<T> ();
		getComponentesRecursivo (gameObject.transform, componentes);
		return componentes;
	}
		
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


}