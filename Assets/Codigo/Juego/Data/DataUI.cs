using System;
using System.Collections;
using UnityEngine;

public class DataUI : MonoBehaviour
{

    public static DataUI i;

    //--PROPIEDADES ASIGNABLES--
    public RecuadroInterfaz menuPrincipal;
	public RecuadroInterfaz seleccionNiveles;
	public RecuadroInterfaz pantallaCarga;
	public RecuadroInterfaz pantallaNivel;

    public Camera camaraUI;
    public Camera camaraMundo;
    public RenderTexture renderTexMundo;
    public QuadUI quadRenderMundo;
    public Material matRenderMundo;

    public Material cielo;

    public Cubemap cieloPradera;
    public Cubemap cieloNieve;

    public float duracionCambioCielo = 1F;
    public float duracionEntradaMapaNivel = 0.25F;
    //--------------------------

    //--PROPIEDADES INTERNAS--
    [NonSerialized] public Cubemap cieloActual;
    [NonSerialized] public Cubemap cieloDestino;
    [NonSerialized] public float animCielo;
    [NonSerialized] public float animEntradaMapaNivel;
    [NonSerialized] public bool cieloCambiando;

    [NonSerialized] public bool nivelCargando;
    [NonSerialized] public RecuadroInterfaz recuadroActual;
    //------------------------

    //--EVENTOS DE UNITY--
    void Awake()
    {  
        recuadroActual = menuPrincipal;
        cieloActual = cieloPradera;
        cielo.SetTexture("_Cube", cieloActual);
        cielo.SetFloat("_Tiempo", 0F);
    }

    //--------------------
    
    //--CORRUTINAS--
    IEnumerator corAnimarCielo()
    {
        float tiempoInicio = Time.fixedTime;
        while(Time.fixedTime < tiempoInicio + duracionCambioCielo)
        {
            animCielo = (Time.fixedTime - tiempoInicio) / duracionCambioCielo;
            cielo.SetFloat("_Tiempo", animCielo);
            yield return null;
        }
        cieloCambiando = false;
        cielo.SetFloat("_Tiempo", 1F);
        animCielo = 1F;
        cieloActual = cieloDestino;
        terminaCargarCielo();
    }

    IEnumerator corAnimarEntradaMapaNivel()
    {
        animEntradaMapaNivel = 0F;
        Transform anclaMapaNivel = DataJuego.i.mapaNivelPrincipal.transform.parent;
        anclaMapaNivel.localScale = Vector3.zero;
        float tiempoInicio = Time.fixedTime;
        while(Time.fixedTime < tiempoInicio + duracionEntradaMapaNivel)
        {
            animEntradaMapaNivel = (Time.fixedTime - tiempoInicio) / duracionEntradaMapaNivel;
            animEntradaMapaNivel = EasingFunction.EaseInCubic(0F, 1F, animEntradaMapaNivel);
            anclaMapaNivel.localScale = new Vector3(animEntradaMapaNivel, animEntradaMapaNivel, animEntradaMapaNivel);
            anclaMapaNivel.localRotation = Quaternion.Euler(0F, animEntradaMapaNivel * 90F - 90F, 0F);
            yield return null;
        }
        anclaMapaNivel.localScale = Vector3.one;
        anclaMapaNivel.localRotation = Quaternion.Euler(0F, 0F, 0F);
        animEntradaMapaNivel = 1F;
    }

    //--ACCIONES--
    public void cambiarRecuadro(RecuadroInterfaz recuadroObjetivo)
    {
        if(recuadroActual == pantallaNivel && recuadroObjetivo == seleccionNiveles)
        {
            quadRenderMundo.GetComponent<MeshRenderer>().enabled = true;
            camaraMundo.targetTexture = renderTexMundo;
            camaraMundo.clearFlags = CameraClearFlags.SolidColor;
        }

        if(recuadroObjetivo == pantallaNivel)
        {
            camaraMundo.clearFlags = CameraClearFlags.Nothing;
            quadRenderMundo.GetComponent<MeshRenderer>().enabled = false;
            camaraMundo.targetTexture = null;
        }
        recuadroActual.saleRecuadro();
        recuadroObjetivo.entraRecuadro();
        recuadroActual = recuadroObjetivo;
    }

    public void empezarCambiarCielo(MapaNivel.TipoPaisaje tipoPaisaje)
    {
        switch (tipoPaisaje)
        {
            case MapaNivel.TipoPaisaje.PRADERA:
                cieloDestino = DataJuego.i.dataUI.cieloPradera;
                break;
            case MapaNivel.TipoPaisaje.NIEVE:
                cieloDestino = DataJuego.i.dataUI.cieloNieve;
                break;
        }

        if (cieloActual != cieloDestino)
        {
            cieloCambiando = true;
            cielo.SetTexture("_Cube", cieloActual);
            cielo.SetTexture("_Cube2", cieloDestino);
            animCielo = 0;
            cielo.SetFloat("_Tiempo", 0F);
            StartCoroutine(corAnimarCielo());
        }
    }

    public void terminaCargarCielo()
    {
        cieloCambiando = false;
        cieloActual = cieloDestino;
        if (!nivelCargando)
        {
            irAPantallaNivel();
        }
    }
    
    public void cargarNivel(int numeroNivel)
    {
        nivelCargando = true;
        cambiarRecuadro(pantallaCarga);
        pantallaCarga.GetComponent<PantallaCarga>().iniciarCarga();
        DataJuego.i.mapaNivelPrincipal.empezarAGenerar(DataJuego.i.niveles[numeroNivel]);
    }

    public void errorAlGenerarNivel()
    {
        Debug.Log("Error al generar nivel");
    }

    public void terminaDeGenerarNivel()
    {
        nivelCargando = false;
        if (!cieloCambiando)
        {
            irAPantallaNivel();
        }
    }

    public void irAPantallaNivel()
    {
        StartCoroutine(corAnimarEntradaMapaNivel());
        cambiarRecuadro(pantallaNivel);
        MapaNivel mn = DataJuego.i.mapaNivelPrincipal;
        camaraMundo.transform.position = mn.transform.parent.position + (Vector3.back * 100);
        camaraMundo.orthographicSize = Mathf.Max(new float[] { mn.dimensiones.x, mn.dimensiones.y, mn.dimensiones.z });
        camaraMundo.transform.localRotation = Quaternion.identity;
        camaraMundo.transform.RotateAround(mn.transform.parent.position, Vector3.right, 60);
        camaraMundo.transform.RotateAround(mn.transform.parent.position, Vector3.up, 180);
    }
    //------------
}
