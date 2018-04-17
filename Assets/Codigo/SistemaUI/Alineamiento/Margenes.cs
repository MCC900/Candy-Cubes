using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Margenes : MonoBehaviour, IObjetoRectAutoajustable {

	public ValorUI arriba;
	public ValorUI abajo;
	public ValorUI izquierda;
	public ValorUI derecha;

	//=======CORUTINAS=======
	bool ejecActualizar = false;

	//======COMPONENTES======
	RectTransform rectTransform;
	RectTransform rtPadre;

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
		actualizarAsociarRectTransform ();
		actualizarAsociarRectPadre ();
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
		ajustarseARectPadre ();
	}

	[ContextMenu("Actualizar")]
	public void actualizarObjetoRectEditor(){
		actualizarAsociarRectPadre ();
		actualizarAsociarRectTransform ();
		ajustarseARectPadre ();
	}

	public void actualizarAsociarRectTransform(){
		this.rectTransform = GetComponent<RectTransform> ();
	}

	public void actualizarAsociarRectPadre(){
		this.rtPadre = transform.parent.GetComponent<RectTransform> ();
	}
		
	void ajustarseARectPadre(){
		rectTransform.offsetMin = new Vector2 (izquierda.getValorPx (), abajo.getValorPx ());
		rectTransform.offsetMax = new Vector2 (-derecha.getValorPx (), -arriba.getValorPx ());
	}

	[ContextMenu("Inicializar Segun RectTransform")]
	public void initSegunRectTransform(){
		this.arriba = new ValorUI (ValorUI.TipoValorUI.FIJO_PX, -rectTransform.offsetMax.y, rtPadre);
		this.abajo = new ValorUI (ValorUI.TipoValorUI.FIJO_PX, rectTransform.offsetMin.y, rtPadre);
		this.izquierda = new ValorUI (ValorUI.TipoValorUI.FIJO_PX, rectTransform.offsetMin.x, rtPadre);
		this.derecha = new ValorUI (ValorUI.TipoValorUI.FIJO_PX, -rectTransform.offsetMax.x, rtPadre);
	}
	//---------------------------------------------------
}
