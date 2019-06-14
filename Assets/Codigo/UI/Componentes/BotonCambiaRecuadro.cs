using UnityEngine;
using System;

public class BotonCambiaRecuadro : Boton
{
    public RecuadroInterfaz recuadroDestino;
    
    void Start()
    {
        this.AlHacerClick = new Action(cambiarRecuadro);
    }

    void cambiarRecuadro()
    {
        DataUI.i.cambiarRecuadro(recuadroDestino);
    }
}
