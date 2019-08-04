using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelUI : MonoBehaviour
{
    [NonSerialized] public TextMeshProUGUI tmproTexto;
    [NonSerialized] public CanvasRenderer canvasRenderer;
    public bool desactivado = false;

    void Awake()
    {
        this.tmproTexto = this.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        this.canvasRenderer = this.transform.GetChild(0).GetComponentInChildren<CanvasRenderer>();
    }

    public void cambiarTexto(string texto)
    {
        this.tmproTexto.text = texto;
    }

    public void desactivar()
    {
        if (!this.desactivado)
        {
            canvasRenderer.SetMaterial(DataUI.i.matTextoOpcionDeseleccionada, 0);
            this.desactivado = true;
        }
    }

    public void activar()
    {
        if (this.desactivado)
        {
            canvasRenderer.SetMaterial(DataUI.i.matTextoOpcionSeleccionada, 0);
            this.desactivado = false;
        }
    }
}
