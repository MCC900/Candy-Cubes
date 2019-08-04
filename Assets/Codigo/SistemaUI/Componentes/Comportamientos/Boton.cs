using System;
using UnityEngine;

public class Boton : MonoBehaviour {
    
    //=====PROPIEDADES AJUSTABLES=====
	public Action AlHacerClick;
    public Action AlEntrarEnFoco;
    public Action AlSalirDeFoco;
    public Action BotonMouseBaja;
    public Action BotonMouseSube;
    public Action BotonMouseSubeFuera;

    //=======VARIABLES PRIVADAS=======
    [NonSerialized] public RectTransform rectTransform;
    [NonSerialized] public BoxCollider2D box2DCollider;

	bool pulsandoMouse;
	bool mouseEncima;

	//----------------EVENTOS UNITY-----------------------

	protected virtual void Awake(){
		pulsandoMouse = false;
		mouseEncima = false;
        this.box2DCollider = GetComponent<BoxCollider2D>();
        this.rectTransform = GetComponent<RectTransform>();
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

    public void OnRectTransformDimensionsChange()
    {
        if (this.box2DCollider)
            this.box2DCollider.size = this.rectTransform.rect.size;
    }

    //---------------------------------------------------
}
