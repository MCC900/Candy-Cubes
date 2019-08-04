using System;
using UnityEngine;

public class SelectorOpcion : MonoBehaviour
{
    public BotonSelectorOpcion btnOpcionSeleccionada = null;

    [NonSerialized] public Action cambiaOpcion;
    [NonSerialized] public string opcionSeleccionada;

    public void Start()
    {
        this.opcionSeleccionada = btnOpcionSeleccionada.idOpcion;
    }

    public void seleccionarOpcion(BotonSelectorOpcion btnOpcion)
    {
        btnOpcionSeleccionada.meshRenderer.sharedMaterials = DataUI.i.matsOpcionDeseleccionada;
        btnOpcionSeleccionada.rendererTextoHijo.SetMaterial(DataUI.i.matTextoOpcionDeseleccionada, 0);

        this.opcionSeleccionada = btnOpcion.idOpcion;
        this.btnOpcionSeleccionada = btnOpcion;

        btnOpcion.meshRenderer.sharedMaterials = DataUI.i.matsOpcionSeleccionada;
        btnOpcion.rendererTextoHijo.SetMaterial(DataUI.i.matTextoOpcionSeleccionada, 0);
        if(cambiaOpcion != null)
            cambiaOpcion();
    }
}
