using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuadUI : MonoBehaviour, IObjetoRectAutoajustable {
	//======COMPONENTES======
	RectTransform rectTransform;
	MeshFilter meshFilter;

	//=======CORUTINAS=======
	bool ejecActualizarMesh = false;

	//======PROPIEDADES======
	static Vector2[] uvs = { new Vector2 (0, 0), new Vector2 (0, 1), new Vector2 (1, 1), new Vector2 (1, 0) };

	bool inicializado = false;

	//----------------EVENTOS UNITY-----------------------
	void Awake(){
		init ();
	}

	void Update(){
		this.ejecActualizarMesh = false;
	}

	//UI.Graphic
	void OnRectTransformDimensionsChange(){
		actualizarMesh ();
	}

	void OnValidate(){
		if(inicializado)
			actualizarObjetoRectEditor();
	}

	//---------------------------------------------------
	//-----------------INICIALIZACIÓN--------------------

	[ContextMenu("Init")]
	void init(){
		inicializado = true;
		actualizarAsociarComponentes ();

		this.meshFilter.mesh = new Mesh ();
		actualizarMesh ();
	}

	//---------------------------------------------------
	//-----------------ACTUALIZACIÓN---------------------

	void actualizarMesh(){
		if (!ejecActualizarMesh) {
			this.ejecActualizarMesh = true;
			StartCoroutine(corActualizarMesh());
		}
	} //+------+//
	IEnumerator corActualizarMesh(){
		//!!! -- NO VERIFICA DESAPARICIÓN O CAMBIO EN LOS COMPONENTES ASOCIADOS POR TEMAS DE EFICIENCIA.
		//TODO ACTUALIZACIÓN EXTERNA AL CAMBIAR O ELIMINARSE RectTransform o MeshFilter

		yield return new WaitForEndOfFrame ();
		this.generarMesh ();
		MeshGen.actualizarMesh (this.meshFilter.mesh, true);
	}
	[ContextMenu("Actualizar Mesh")]
	public void actualizarObjetoRectEditor(){
		this.actualizarAsociarComponentes (); //Estamos en el editor, no nos preocupa eficiencia y se cambian/borran componentes constantemente
		this.generarMesh ();
		this.meshFilter.mesh = MeshGen.getMesh (true);
	}

	public void actualizarAsociarComponentes(){
		actualizarAsociarRectTransform ();
		actualizarAsociarMeshFilter ();
	}

	public void actualizarAsociarRectTransform(){
		this.rectTransform = GetComponent<RectTransform> ();
	}

	public void actualizarAsociarMeshFilter(){
		this.meshFilter = GetComponent<MeshFilter> ();
	}
	//---------------------------------------------------
	//-----------------AUTOGENERADO----------------------
	void generarMesh(){
		MeshGen.nuevaMeshVertsDobles (2);
		Vector3[] esquinas = new Vector3[4];
		this.rectTransform.GetLocalCorners (esquinas);
		MeshGen.quadConvexoAutoVert (esquinas [0], esquinas [1], esquinas [2], esquinas [3],
			uvs[0], uvs[1], uvs[2], uvs[3]);
	}
}
