using UnityEngine;
using System;

public class ConfigNuevoMapa : MonoBehaviour
{
    public SelectorOpcion selector2D3D;
    public InputNumero inputAncho;
    public InputNumero inputLargo;
    public InputNumero inputAlto;
    public BotonCrear botonCrear;

    private void Start()
    {
        this.selector2D3D.cambiaOpcion = new Action(this.cambiaOpcion2D3D);
        this.botonCrear.config = this;
    }

    private void cambiaOpcion2D3D()
    {
        if(selector2D3D.btnOpcionSeleccionada.idOpcion == "3D")
        {
            inputAlto.activar();
        } else
        {
            inputAlto.displayNumero.cambiarNumero(1);
            inputAlto.desactivar();
        }
    }
}