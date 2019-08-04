using UnityEngine;
using System;

public class BotonCrear : Boton
{
    [NonSerialized] public ConfigNuevoMapa config;

    // Use this for initialization
    void Start()
    {
        this.AlHacerClick = new Action(this.clickCrear);
    }

    void clickCrear()
    {
        bool es2D = config.selector2D3D.btnOpcionSeleccionada.idOpcion == "2D";
        int ancho = config.inputAncho.numero;
        int largo = config.inputLargo.numero;
        int alto = config.inputAlto.numero;

        DataUI.i.nuevoNivelCreador(ancho, largo, alto, es2D, MapaNivel.TipoPaisaje.NIEVE);
    }
}
