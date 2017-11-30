using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;

[InitializeOnLoad]
class SistemaObjetoRect {

	[NonSerialized]static List<ObjetoRectUpdateChecker> objetoRectCheckers = new List<ObjetoRectUpdateChecker> ();

	[NonSerialized]static bool actualizacionesAutomaticas = true;
	[NonSerialized]static bool actualizarLista = false;

	static SistemaObjetoRect(){
		actualizarLista = true;
		EditorApplication.update += actualizar;
	}

	static void actualizar(){
		if (!EditorApplication.isPlaying) {
			if (actualizarLista) {
				actualizarListaRectMeshes ();
				actualizarLista = false;
			}
			List<ObjetoRectUpdateChecker> aEliminar = new List<ObjetoRectUpdateChecker> ();
			if (actualizacionesAutomaticas) {
				foreach (ObjetoRectUpdateChecker rmuc in objetoRectCheckers) {
					if (rmuc.existeObjeto ()) {
						rmuc.tryUpdate ();
					} else {
						aEliminar.Add (rmuc);
					}
				}
			}
			foreach (ObjetoRectUpdateChecker oruc in aEliminar) {
				objetoRectCheckers.Remove (oruc);
			}
			aEliminar.Clear ();
		}
	}
	static void actualizarListaRectMeshes(){
		objetoRectCheckers = new List<ObjetoRectUpdateChecker> ();
		GameObject[] objetosRaiz = SceneManager.GetActiveScene ().GetRootGameObjects ();
		List<QuadUI> quadUIs = encontrarComponentesHijos<QuadUI> (objetosRaiz);
		foreach (QuadUI qui in quadUIs) {
			objetoRectCheckers.Add(new ObjetoRectUpdateChecker(qui));
		}
		List<PanelUI> panelUIs = encontrarComponentesHijos<PanelUI> (objetosRaiz);
		foreach (PanelUI pui in panelUIs) {
			objetoRectCheckers.Add(new ObjetoRectUpdateChecker(pui));
		}
		List<Objeto3DUI> obj3DUIs = encontrarComponentesHijos<Objeto3DUI> (objetosRaiz);
		foreach (Objeto3DUI obj3d in obj3DUIs) {
			objetoRectCheckers.Add(new ObjetoRectUpdateChecker(obj3d));
		}
	}
		
	static List<T> encontrarComponentesHijos<T>(GameObject[] objetosPadre){
		List<T> componentes = new List<T>();
		for (int i = 0; i < objetosPadre.Length; i++) {
			componentes.AddRange(objetosPadre[i].GetComponentsInChildren<T>());
		}
		return componentes;
	}

	[MenuItem("SistemaUI/Actualizaciones Automáticas/Activar", false)]
	static void activarActualizacionesAutomaticas(){
		actualizacionesAutomaticas = true;
	}

	[MenuItem("SistemaUI/Actualizaciones Automáticas/Desactivar", false)]
	static void desactivarActualizacionesAutomaticas(){
		actualizacionesAutomaticas = false;
	}

	[MenuItem("SistemaUI/Actualizaciones Automáticas/Activar", true)]
	static bool checkMenu1(){
		return !actualizacionesAutomaticas;
	}

	[MenuItem("SistemaUI/Actualizaciones Automáticas/Desactivar", true)]
	static bool checkMenu2(){
		return actualizacionesAutomaticas;
	}

	[MenuItem("SistemaUI/Refrescar Lista ObjetosRect")]
	static void actualizarListaObjetosRect(){
		actualizarLista = true;
		foreach (ObjetoRectUpdateChecker oruc in objetoRectCheckers) {
			oruc.forceUpdate ();
		}
	}
}
