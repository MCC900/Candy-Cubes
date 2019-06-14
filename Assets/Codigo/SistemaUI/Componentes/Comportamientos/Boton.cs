using System;
using UnityEngine;

public class Boton : MonoBehaviour {
    
    //=====PROPIEDADES AJUSTABLES=====
	protected Action AlHacerClick;
    protected Action AlEntrarEnFoco;
    protected Action AlSalirDeFoco;
    protected Action BotonMouseBaja;
    protected Action BotonMouseSube;
    protected Action BotonMouseSubeFuera;

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
        if (AlEntrarEnFoco != null)
            AlEntrarEnFoco();
	}

	void OnMouseExit(){
        mouseEncima = false;
        if (AlSalirDeFoco != null)
            AlSalirDeFoco();
    }

	void OnMouseDown(){
		pulsandoMouse = true;
        if (BotonMouseBaja != null)
            BotonMouseBaja();
    }

	void OnMouseUp(){
        if (BotonMouseSube != null)
            BotonMouseSube();
        if (pulsandoMouse && mouseEncima) {
            if(AlHacerClick != null)
                AlHacerClick();
        } else {
            if(BotonMouseSubeFuera != null)
			    BotonMouseSubeFuera();
        }
        pulsandoMouse = false;
    }

	//---------------------------------------------------
}
