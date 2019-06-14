using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public class RecuadroInterfaz : MonoBehaviour {

	//=====PROPIEDADES AJUSTABLES=====
	float segundosDuracionAnim = 0.65F;
	EasingFunction.Ease easing = EasingFunction.Ease.EaseOutQuart; //Suavizado de animacion (easing)

	public enum EstadoRecuadro {QUIETO, ENTRANDO, SALIENDO, FUERA, DESACTIVADO};
	public EstadoRecuadro estado;


	//=======VARIABLES PRIVADAS=======
	EasingFunction.Function easingFunc;

	float anchoPrev = 0; //Ancho de la pantalla en el momento anterior
	float altoPrev = 0; //Alto de la pantalla en el momento anterior
	float momentoInicioAnim; //Momento en el que inicio la animacion
	bool interactivo = true; //Si los botones y menus hijos estan funcionando, i.e. el recuadro es interactivo

	Collider2D[] collidersHijos;

	//==========COMPONENTES===========
	RectTransform rectTransform;

	//----------------EVENTOS UNITY-----------------------

	void Start () {
		collidersHijos = UtilComponentes.getComponentesEnDescendencia<Collider2D>(gameObject).ToArray();
		easingFunc = EasingFunction.GetEasingFunction (easing);
		actualizarAsociarComponentes ();
		//actualizarTamanoRecuadro ();
	}
		
	[ExecuteInEditMode]
	void Update(){
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

	#if UNITY_EDITOR
	void updateEditor(){
		if (Screen.width != anchoPrev || Screen.height != altoPrev) {
			anchoPrev = Screen.width;
			altoPrev = Screen.height;
			actualizarTamanoRecuadro ();
		}
	}
	#endif

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

	[ContextMenu("Actualizar Tamaño Recuadro")]
	void actualizarTamanoRecuadro(){
		rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
	}
		
	void actualizarAsociarComponentes(){
		actualizarAsociarRectTransform ();
	}

	void actualizarAsociarRectTransform(){
		rectTransform = GetComponent<RectTransform> ();
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
