using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto3DUI : MonoBehaviour, IObjetoRectAutoajustable {

	public Vector3 escalaRelativa = new Vector3(1,1,1);
	public bool mantenerProporcion = true;

	//======COMPONENTES======
	RectTransform rtPadre;
	RectTransform rectTransform;
	MeshFilter meshFilter;

	//=======CORUTINAS=======
	bool ejecActualizarEscala = false;

	bool inicializado = false;

	//----------------EVENTOS UNITY-----------------------

	void Awake(){
		init ();
	}

	void Update(){
		this.ejecActualizarEscala = false;
	}
		
	//UI.Graphic
	void OnRectTransformDimensionsChange(){
		actualizarEscala ();
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
		this.actualizarAsociarComponentes ();
	}

	//---------------------------------------------------
	//-----------------ACTUALIZACIÓN---------------------
	void setCalcularEscala(){
		float ancho = this.rtPadre.rect.width;
		float alto = this.rtPadre.rect.height;

		float meshTamanoX = this.meshFilter.sharedMesh.bounds.extents.x * 2;
		float meshTamanoY = this.meshFilter.sharedMesh.bounds.extents.y * 2;
		float meshTamanoZ = this.meshFilter.sharedMesh.bounds.extents.z * 2;

		Vector3 escala;
		if (!this.mantenerProporcion) {
			escala = new Vector3 ((ancho / meshTamanoX) * this.escalaRelativa.x, (alto / meshTamanoY) * this.escalaRelativa.y, meshTamanoZ * this.escalaRelativa.z);
		} else {
			float coefx = ancho / meshTamanoX;
			float coefy = alto / meshTamanoY;
			float coef = Mathf.Min (coefx, coefy);
			escala = new Vector3 (coef * this.escalaRelativa.x, coef * this.escalaRelativa.y, coef * this.escalaRelativa.z);
			//this.rectTransform.localPosition = Vector3.zero;
			this.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, meshTamanoX);
			this.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, meshTamanoY);

		}

		this.transform.localScale = escala;
	}

	void actualizarEscala(){
		if (!ejecActualizarEscala) {
			this.ejecActualizarEscala = true;
			StartCoroutine (corActualizarEscala());
		}
	} //+------+//
	IEnumerator corActualizarEscala(){
		//!!! -- NO VERIFICA DESAPARICIÓN O CAMBIO EN LOS COMPONENTES ASOCIADOS POR TEMAS DE EFICIENCIA.
		//TODO ACTUALIZACIÓN EXTERNA AL CAMBIAR O ELIMINARSE RectTransform o MeshFilter

		yield return new WaitForEndOfFrame ();
		this.setCalcularEscala ();
	}
	[ContextMenu("Actualizar Mesh")]
	public void actualizarObjetoRectEditor(){
		this.actualizarAsociarComponentes (); //Estamos en el editor, no nos preocupa eficiencia y se cambian/borran componentes constantemente
		this.setCalcularEscala ();
	}

	//---------------------------------------------------
	public void actualizarAsociarComponentes(){
		actualizarAsociarRectPadre ();
		actualizarAsociarRectTransform ();
		actualizarAsociarMeshFilter ();
	}

	public void actualizarAsociarRectPadre(){
		if(transform.parent)
			this.rtPadre = transform.parent.GetComponent<RectTransform> ();
	}

	public void actualizarAsociarRectTransform(){
		this.rectTransform = GetComponent<RectTransform> ();
	}

	public void actualizarAsociarMeshFilter(){
		this.meshFilter = GetComponent<MeshFilter> ();
	}
}
