using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlinearRect : MonoBehaviour, IObjetoRectAutoajustable {
		
	public enum ALINEACION_HORIZ : byte {IZQUIERDA, CENTRO, DERECHA, PORCENTAJE, IGNORAR}
	public enum ALINEACION_VERT : byte {ARRIBA, MEDIO, ABAJO, PORCENTAJE, IGNORAR}
	public ALINEACION_HORIZ alineacionHorizontal = ALINEACION_HORIZ.CENTRO;
	public ALINEACION_VERT alineacionVertical = ALINEACION_VERT.MEDIO;

	[Range(0F, 100F)] public float porcentajeX;
	[Range(0F, 100F)] public float porcentajeY;

	//=======CORUTINAS=======
	bool ejecActualizar = false;

	//======COMPONENTES======
	RectTransform rtPadre;
	RectTransform rectTransform;

	bool inicializado = false;

	//----------------EVENTOS UNITY-----------------------

	void Awake(){
		inicializado = true;
		init ();
	}

	void Update(){
		this.ejecActualizar = false;
	}

	//UI.Graphic
	void OnRectTransformDimensionsChange(){
		this.actualizar ();
	}

	void OnValidate(){
		if(inicializado)
			actualizarObjetoRectEditor();
	}

	//---------------------------------------------------
	//-----------------INICIALIZACIÓN--------------------

	[ContextMenu("Init")]
	void init(){
		actualizarAsociarRectPadre ();
		actualizarAsociarRectTransform ();
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
		ajustarseARectPadre ();
	}

	[ContextMenu("Actualizar")]
	public void actualizarObjetoRectEditor(){
		actualizarAsociarRectPadre ();
		actualizarAsociarRectTransform ();
		ajustarseARectPadre ();
	}
		
	public void actualizarAsociarRectPadre(){
		this.rtPadre = transform.parent.GetComponent<RectTransform> ();
	}

	public void actualizarAsociarRectTransform(){
		this.rectTransform = GetComponent<RectTransform> ();
	}

	void ajustarseARectPadre(){

		Vector3[] esquinas = new Vector3[4];
		this.rectTransform.GetLocalCorners (esquinas);
		float ancho = Mathf.Abs(esquinas [2].x - esquinas [0].x) * this.rectTransform.transform.localScale.x;
		float alto = Mathf.Abs(esquinas [1].y - esquinas [0].y) * this.rectTransform.transform.localScale.x;

		Vector3[] esquinasPadre = new Vector3[4];
		this.rtPadre.GetLocalCorners (esquinasPadre);
		float anchoPadre = Mathf.Abs(esquinasPadre [2].x - esquinasPadre [0].x) * this.rtPadre.transform.localScale.x;
		float altoPadre = Mathf.Abs (esquinasPadre [1].y - esquinasPadre [0].y) * this.rtPadre.transform.localScale.y;

		this.rectTransform.anchorMin = new Vector2 (0F, 0F);
		this.rectTransform.anchorMax = new Vector2 (1F, 1F);

		float posX = 0;
		float posY = 0;

		switch (alineacionHorizontal) {

		case ALINEACION_HORIZ.CENTRO:
			posX = 0;
			break;
		case ALINEACION_HORIZ.DERECHA:
			posX = anchoPadre / 2 - ancho / 2;
			break;
		case ALINEACION_HORIZ.IZQUIERDA:
			posX = -anchoPadre / 2 + ancho / 2;
			break;
		case ALINEACION_HORIZ.PORCENTAJE:
			float izq = -anchoPadre / 2 + ancho / 2;
			float der = -izq;
			posX = (der - izq) * (this.porcentajeX / 100F) + izq; 
			break;
		default: //IGNORAR (EL SCRIPT NO AFECTA LA POSICIÓN HORIZONTAL)
			posX = this.rectTransform.anchoredPosition.x;
			break;
		}

		switch (alineacionVertical) {

		case ALINEACION_VERT.MEDIO:
			posY = 0;
			break;
		case ALINEACION_VERT.ARRIBA:
			posY = altoPadre / 2 - alto / 2;
			break;
		case ALINEACION_VERT.ABAJO:
			posY = -altoPadre / 2 + alto / 2;
			break;
		case ALINEACION_VERT.PORCENTAJE:
			float aba = -altoPadre / 2 + alto / 2;
			float arr = -aba;
			posY = (arr - aba) * (this.porcentajeX / 100F) + aba; 
			break;
		default: //IGNORAR (EL SCRIPT NO AFECTA LA POSICIÓN HORIZONTAL)
			posY = this.rectTransform.anchoredPosition.y;
			break;
		}

		this.rectTransform.anchoredPosition = new Vector2 (posX, posY);

	}
	//---------------------------------------------------
}
