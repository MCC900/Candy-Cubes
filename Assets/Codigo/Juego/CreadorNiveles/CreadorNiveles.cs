using System;
using UnityEngine;

public class CreadorNiveles : MonoBehaviour
{

    public static CreadorNiveles i;
    
    public void empezarGenerarNuevoNivel(int ancho, int largo, int alto, bool es2D, MapaNivel.TipoPaisaje tipoPaisaje)
    {
        if (es2D)
            alto = 1;

        Debug.Log(ancho + " " + largo + " " + alto + " " + es2D.ToString());
        DataJuego.i.mapaNivelPrincipal.empezarAGenerarCreador(ancho, largo, alto,
            es2D ? MapaNivel.TipoMapaNivel.MAPA_2D : MapaNivel.TipoMapaNivel.MAPA_3D);

        DataUI.i.empezarCambiarCielo(tipoPaisaje);

    }

    public void terminaDeGenerar()
    {
    }
}