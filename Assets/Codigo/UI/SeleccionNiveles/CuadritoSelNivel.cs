using System;
using UnityEngine;

public class CuadritoSelNivel : Boton
{
	BoxCollider2D box2DCollider;
	RectTransform rectTransform;
    public string codigoMapa;
    public int numeroNivel;

	public void Awake(){
		this.box2DCollider = GetComponent<BoxCollider2D> ();
		this.rectTransform = GetComponent<RectTransform> ();
        this.AlHacerClick = new Action(clickBotonNivel);
	}

    public void clickBotonNivel()
    {
        DataUI.i.cargarNivel(numeroNivel);
    }
		
	public void OnRectTransformDimensionsChange(){
		box2DCollider.size = rectTransform.rect.size;
	}
}
