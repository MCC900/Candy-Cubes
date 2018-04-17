using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TextoUI : MonoBehaviour, IObjetoRectAutoajustable {

	public float coeficienteTamano = 1;

	//=======CORUTINAS=======
	bool ejecActualizar = false;

	//======COMPONENTES======
	RectTransform rectTransform;
	//RectTransform rtPadre;
	TextMesh textMesh;

	bool inicializado = false;

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
		//!!! -- NO VERIFICA DESAPARICIÓN O CAMBIO EN LOS COMPONENTES ASOCIADOS POR TEMAS DE EFICIENCIA.
		//TODO ACTUALIZACIÓN EXTERNA AL CAMBIAR O ELIMINARSE RectTransform o MeshFilter
		yield return new WaitForEndOfFrame ();
		actualizarTexto ();
	}

	[ContextMenu("Actualizar")]
	public void actualizarObjetoRectEditor(){
		this.actualizarAsociarComponentes (); //Estamos en el editor, no nos preocupa eficiencia y se cambian/borran componentes constantemente
		actualizarTexto ();
	}

	public void actualizarAsociarComponentes(){
		this.rectTransform = GetComponent<RectTransform> ();
		//this.rtPadre = transform.parent.GetComponent<RectTransform> ();
		this.textMesh = GetComponent<TextMesh> ();
	}

	public void actualizarTexto(){
		int anchoTexto = 0;
		char[] caracteres = textMesh.text.ToCharArray ();
		foreach (char c in caracteres) {
			CharacterInfo ci;
			textMesh.font.GetCharacterInfo (c, out ci, 100);
			anchoTexto += ci.advance;
		}
		int tamFuenteSegunAlto = (int)(rectTransform.rect.height * coeficienteTamano * ((float)textMesh.font.lineHeight / textMesh.font.ascent));
		int tamFuenteSegunAncho = (int)(((float)rectTransform.rect.width * 100) / anchoTexto);
		textMesh.fontSize = Math.Min (tamFuenteSegunAlto, tamFuenteSegunAncho);
		//TODO Ofrecer opciones apropiadas para decidir como se ajusta el texto

	}
	//---------------------------------------------------
}

