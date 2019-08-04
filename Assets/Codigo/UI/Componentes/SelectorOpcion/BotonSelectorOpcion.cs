
using System;
using TMPro;
using UnityEngine;

public class BotonSelectorOpcion : Boton
{
    SelectorOpcion selectorOpcion;
    [NonSerialized] public MeshRenderer meshRenderer;
    [NonSerialized] public CanvasRenderer rendererTextoHijo;

    public string idOpcion;

    public void Awake()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.rendererTextoHijo = this.transform.GetChild(0).GetComponentInChildren<CanvasRenderer>();
        this.selectorOpcion = this.transform.parent.GetComponentInParent<SelectorOpcion>();
        this.AlHacerClick = new Action(this.opcionSeleccionada);
    }

    public void opcionSeleccionada()
    {
        selectorOpcion.seleccionarOpcion(this);
    }
}
