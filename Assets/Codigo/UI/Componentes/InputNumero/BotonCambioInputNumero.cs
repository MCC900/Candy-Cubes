using System;
using UnityEngine;

public class BotonCambioInputNumero : Boton
{
    public int incremento = 1;
    public bool desactivado = false;

    [NonSerialized] public MeshRenderer meshRenderer;
    [NonSerialized] public CanvasRenderer rendererTextoHijo;


    InputNumero inputNumero;

    protected override void Awake()
    {
        base.Awake();
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.rendererTextoHijo = this.transform.GetChild(0).GetComponentInChildren<CanvasRenderer>();
        this.inputNumero = this.transform.parent.GetComponentInParent<InputNumero>();
        this.AlHacerClick = new Action(this.clickBoton);
    }

    /*
    public void OnRectTransformDimensionsChange()
    {
        if (this.box2DCollider)
            this.box2DCollider.size = this.rectTransform.rect.size;
    }
    */

    void clickBoton()
    {
        this.inputNumero.incrementar(this.incremento);
    }
}
