using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RecuadroInterfaz : MonoBehaviour {

	// Use this for initialization
	void Start () {
		actualizar ();
	}

	[ContextMenu("Actualizar")]
	void actualizar(){
		RectTransform rt = GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (Screen.width, Screen.height);
	}

	float anchoPrev = 0;
	float altoPrev = 0;

	[ExecuteInEditMode]
	void Update(){
		if (!UnityEditor.EditorApplication.isPlaying) {
			if (Screen.width != anchoPrev || Screen.height != altoPrev) {
				anchoPrev = Screen.width;
				altoPrev = Screen.height;
				actualizar ();
			}
		}
	}
}
