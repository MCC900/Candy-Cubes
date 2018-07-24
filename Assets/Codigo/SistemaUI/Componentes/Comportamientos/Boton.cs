using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Boton : MonoBehaviour {

	//=====PROPIEDADES AJUSTABLES=====
	public UnityEvent alHacerClick;
	public UnityEvent alEntrarEnFoco;
	public UnityEvent alSalirDeFoco;
	public UnityEvent botonMouseBaja;
	public UnityEvent botonMouseSube;
	public UnityEvent botonMouseSubeFuera;

	//=======VARIABLES PRIVADAS=======
	bool pulsandoMouse;
	bool mouseEncima;

	//----------------EVENTOS UNITY-----------------------

	void Awake(){
		pulsandoMouse = false;
		mouseEncima = false;
	}

	void OnMouseEnter(){
		mouseEncima = true;
		alEntrarEnFoco.Invoke ();
	}

	void OnMouseExit(){
		alSalirDeFoco.Invoke ();
		mouseEncima = false;
		pulsandoMouse = false;
	}

	void OnMouseDown(){
		pulsandoMouse = true;
		botonMouseBaja.Invoke ();
	}

	void OnMouseUp(){
		botonMouseSube.Invoke ();
		if (pulsandoMouse && mouseEncima) {
			alHacerClick.Invoke ();
		} else {
			botonMouseSubeFuera.Invoke ();
		}
	}

	//---------------------------------------------------
}
