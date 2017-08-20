using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RecrearCanvas : MonoBehaviour {
	
	[ContextMenu("Actualizar")]
	void actualizar(){
		Debug.Log ("Canvas actualizado");
		actualizarHijos (transform);
	}

	void actualizarHijos(Transform t){
		for (var i = 0; i < t.childCount; i++) {
			Transform hijo = t.GetChild (i);

			LayoutActualizable la = hijo.GetComponent<LayoutActualizable> ();
			if (la != null) {
				Canvas.ForceUpdateCanvases ();
				la.actualizar ();
			}

			if (hijo.childCount > 0) {
				actualizarHijos (hijo);
			}
		}
	}

	void Start(){
		actualizar ();
	}

	float anchoPrev = 0;
	float altoPrev = 0;
	void Update () {
		if (!UnityEditor.EditorApplication.isPlaying) {
			RectTransform rt = gameObject.GetComponent<RectTransform> ();
			if (rt.hasChanged || Screen.width != anchoPrev || Screen.height != altoPrev) {
				anchoPrev = Screen.width;
				altoPrev = Screen.height;
				rt.hasChanged = false;
				actualizar ();
			}
		}
	}
}
