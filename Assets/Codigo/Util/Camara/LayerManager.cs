using UnityEngine;
public static class LayerManager
{
    public static int combinarCapas(string[] capas)
    {
        int combinadas = 0;
        for(int i=0; i<capas.Length; i++)
        {
            combinadas |= 1 << LayerMask.NameToLayer(capas[i]);
        }
        return combinadas;
    }

    public static int combinarCapas(int[] numerosCapa)
    {
        int combinadas = 0;
        for (int i = 0; i < numerosCapa.Length; i++)
        {
            combinadas |= 1 << numerosCapa[i];
        }
        return combinadas;
    }

    public static void ocultarTodasLasCapas(Camera camara)
    {
        camara.cullingMask = 0;
    }


    public static void seleccionarCapa(Camera camara, string capa)
    {
        camara.cullingMask = LayerMask.NameToLayer(capa);
    }

    public static void seleccionarCapa(Camera camara, int capa)
    {
        camara.cullingMask = capa;
    }

    public static void seleccionarCapas(Camera camara, string[] capas)
    {
        camara.cullingMask = combinarCapas(capas);
    }

    public static void seleccionarCapas(Camera camara, int[] numerosCapa)
    {
        camara.cullingMask = combinarCapas(numerosCapa);
    }

    public static void mostrarCapa(Camera camara, string capa)
    {
        camara.cullingMask |= 1 << LayerMask.NameToLayer(capa);
    }


    public static void mostrarCapa(Camera camara, int numeroCapa)
    {
        camara.cullingMask |= 1 << numeroCapa;
    }

    public static void ocultarCapa(Camera camara, string capa)
    {

        camara.cullingMask &= ~(1 << LayerMask.NameToLayer(capa));
    }
    
    public static void ocultarCapa(Camera camara, int numeroCapa)
    {
        camara.cullingMask &= ~(1 << numeroCapa);
    }

    public static void toggleCapa(Camera camara, string capa)
    {
        camara.cullingMask ^= 1 << LayerMask.NameToLayer(capa);
    }

    public static void toggleCapa(Camera camara, int numeroCapa)
    {
        camara.cullingMask ^= 1 << numeroCapa;
    }
}
