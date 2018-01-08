using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextoUI : MonoBehaviour, IObjetoRectAutoajustable {

	public float altoBaseTexto = 100;
	public float coeficienteTamano = 8;

	//=======CORUTINAS=======
	bool ejecActualizar = false;

	//======COMPONENTES======
	//RectTransform rectTransform;
	RectTransform rtPadre;
	TextMesh textMesh;

	//----------------EVENTOS UNITY-----------------------

	void Awake(){
		init ();
	}

	void Update(){
		this.ejecActualizar = false;
	}

	//UI.Graphic
	void OnRectTransformDimensionsChange(){
		actualizar ();
	}

	//---------------------------------------------------
	//-----------------INICIALIZACIÓN--------------------

	[ContextMenu("Init")]
	void init(){
		actualizarAsociarComponentes ();
		actualizar ();
	}

	//---------------------------------------------------
	//-----------------ACTUALIZACIÓN---------------------

	public void actualizar(){
		if (!ejecActualizar) {
			this.ejecActualizar = true;
			StartCoroutine(corActualizar());
		}
	} //+------+//
	IEnumerator corActualizar(){
		yield return new WaitForEndOfFrame ();
		this.actualizarAsociarComponentes ();
		textMesh.characterSize = (rtPadre.rect.height / altoBaseTexto) * coeficienteTamano;
	}

	[ContextMenu("Actualizar")]
	public void actualizarObjetoRectEditor(){
		this.actualizarAsociarComponentes ();
		textMesh.characterSize = (rtPadre.rect.height / altoBaseTexto) * coeficienteTamano;
	}

	void actualizarAsociarComponentes(){
		//this.rectTransform = GetComponent<RectTransform> ();
		this.rtPadre = transform.parent.GetComponent<RectTransform> ();
		this.textMesh = GetComponent<TextMesh> ();
	}
		
	//---------------------------------------------------
}

