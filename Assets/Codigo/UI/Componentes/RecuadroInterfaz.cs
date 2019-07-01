using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public class RecuadroInterfaz : MonoBehaviour, IObjetoRectAutoajustable {

	//--PROPIEDADES AJUSTABLES--
	float segundosDuracionAnim = 0.65F;
	EasingFunction.Ease easing = EasingFunction.Ease.EaseOutQuart; //Suavizado de animacion (easing)

	public enum EstadoRecuadro {QUIETO, ENTRANDO, SALIENDO, FUERA, DESACTIVADO};
	public EstadoRecuadro estado;

    //--PROPIEDADES INTERNAS--
    EasingFunction.Function easingFunc;

	float anchoPrev = 0; //Ancho de la pantalla en el momento anterior
	float altoPrev = 0; //Alto de la pantalla en el momento anterior
	float momentoInicioAnim; //Momento en el que inicio la animacion
	bool interactivo = true; //Si los botones y menus hijos estan funcionando, i.e. el recuadro es interactivo

    bool ejecActualizarRI = false;
    bool inicializado = false;

	Collider2D[] collidersHijos;
    RectTransform rtPadre;

    //==========COMPONENTES===========
    RectTransform rectTransform;

    //----------------EVENTOS UNITY-----------------------
    
    void init()
    {
        collidersHijos = UtilComponentes.getComponentesEnDescendencia<Collider2D>(gameObject).ToArray();
        easingFunc = EasingFunction.GetEasingFunction(easing);
        actualizarAsociarComponentes();
        actualizarTamanoRecuadro();
        inicializado = true;
    }

    void Awake () {
        init();
	}
	
    [ExecuteInEditMode]
	void Update(){
        ejecActualizarRI = false;
        #if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying) {
			//EDITOR
			updateEditor();
		} else {
			//JUEGO
			updateJuego();
		}
		#else
			updateJuego();
		#endif
	}

    void OnValidate()
    {
        if (inicializado)
            actualizarObjetoRectEditor();
    }

    //UI.Graphic
    void OnRectTransformDimensionsChange()
    {
        actualizarTamanoRecuadro();
    }

    #if UNITY_EDITOR

        void updateEditor()
        {
            if (Screen.width != anchoPrev || Screen.height != altoPrev) {
			    anchoPrev = Screen.width;
			    altoPrev = Screen.height;
                setTamanoRecuadro();
		    }
        
	    }

    #endif

    [ContextMenu("Actualizar Tamaño Recuadro")]
    public void actualizarObjetoRectEditor()
    {
        actualizarAsociarComponentes();
        setTamanoRecuadro();
    }

	void updateJuego(){
		switch (estado) {
		case EstadoRecuadro.DESACTIVADO:
			//No hacemos nada
			break;
		case EstadoRecuadro.QUIETO:
			//No hacemos nada
			break;
		case EstadoRecuadro.ENTRANDO:
		case EstadoRecuadro.SALIENDO:
			float tiempoAnim = Time.realtimeSinceStartup - momentoInicioAnim;
			if (tiempoAnim >= segundosDuracionAnim) {
				setPosicionAnim (segundosDuracionAnim);
				if (estado == EstadoRecuadro.ENTRANDO) {
					estado = EstadoRecuadro.QUIETO;
				} else {
					estado = EstadoRecuadro.FUERA;
				}
			} else {
				setPosicionAnim (tiempoAnim);
			}
			break;
		case EstadoRecuadro.FUERA:
			gameObject.SetActive (false);
			break;
		}
	}
    //----------------------------------------------------
    //------------------ACTUALIZACIÓN---------------------
    void actualizarTamanoRecuadro()
    {
        if (!ejecActualizarRI)
        {
            ejecActualizarRI = true;
            StartCoroutine(corActualizarTamanoRecuadro());
        }
    }
	IEnumerator corActualizarTamanoRecuadro()
    {
		yield return new WaitForEndOfFrame ();
        setTamanoRecuadro();
	}
	
    void setTamanoRecuadro()
    {
        rectTransform.sizeDelta = new Vector2(rtPadre.rect.width, rtPadre.rect.height);
    }

	void actualizarAsociarComponentes(){
		actualizarAsociarRectTransform ();
        actualizarAsociarRTPadre();
	}

	void actualizarAsociarRectTransform(){
		rectTransform = GetComponent<RectTransform> ();
	}

    void actualizarAsociarRTPadre()
    {
        rtPadre = transform.parent.GetComponent<RectTransform>();
    }
    //----------------------------------------------------
    //--------------------CONTROL-------------------------

    public void entraRecuadro(){
		gameObject.SetActive (true);
		activarInteraccion ();
		estado = EstadoRecuadro.ENTRANDO;
		momentoInicioAnim = Time.realtimeSinceStartup;
	}

	public void saleRecuadro(){
		desactivarInteraccion ();
		estado = EstadoRecuadro.SALIENDO;
		momentoInicioAnim = Time.realtimeSinceStartup;
	}

	void setPosicionAnim(float tiempoAnim){
		float progresoAnim = tiempoAnim / segundosDuracionAnim;
		if (estado == EstadoRecuadro.ENTRANDO) {
			//ENTRANDO
			rectTransform.anchoredPosition = new Vector2(easingFunc(Screen.width/2, 0, progresoAnim), 0);
			//rectTransform.localRotation = Quaternion.AngleAxis (easingFunc (90, 0, progresoAnim), Vector3.up);
			rectTransform.localScale = new Vector3(easingFunc(0,1,progresoAnim),1,1);
		} else {
			//SALIENDO
			rectTransform.anchoredPosition = new Vector2(easingFunc(0, -Screen.width/2, progresoAnim), 0);
			//rectTransform.localRotation = Quaternion.AngleAxis (easingFunc (0, -90, progresoAnim), Vector3.up);
			rectTransform.localScale = new Vector3(easingFunc(1,0,progresoAnim),1,1);
		}
	}

	void desactivarInteraccion(){
		if (interactivo) {
			foreach (Collider2D c in collidersHijos) {
				c.enabled = false;
			}
			interactivo = false;
		}
	}

	void activarInteraccion(){
		if (!interactivo) {
			foreach (Collider2D c in collidersHijos) {
				c.enabled = true;
			}
			interactivo = true;
		}
	}
	//----------------------------------------------------
}
