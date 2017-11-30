using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto3DUI : MonoBehaviour, IObjetoRectAutoajustable {

	public Vector3 escalaRelativa = new Vector3(1,1,1);
	public bool mantenerProporcion = true;

	//======COMPONENTES======
	RectTransform rectTransform;
	MeshFilter meshFilter;

	//=======CORUTINAS=======
	bool ejecActualizarEscala = false;

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

	//---------------------------------------------------
	//-----------------INICIALIZACIÓN--------------------

	[ContextMenu("Init")]
	void init(){
		this.actualizarAsociarComponentes ();
	}

	//---------------------------------------------------
	//-----------------ACTUALIZACIÓN---------------------
	void setCalcularEscala(){
		float ancho = this.rectTransform.rect.width;
		float alto = this.rectTransform.rect.height;

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
		yield return new WaitForEndOfFrame ();
		this.setCalcularEscala ();
	}
	[ContextMenu("Actualizar Mesh")]
	public void actualizarObjetoRectEditor(){
		this.actualizarAsociarComponentes ();
		this.setCalcularEscala ();
	}

	//---------------------------------------------------
	void actualizarAsociarComponentes(){
		this.rectTransform = GetComponent<RectTransform> ();
		this.meshFilter = GetComponent<MeshFilter> ();
	}
}
