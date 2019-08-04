using System;
using System.Collections;
using UnityEngine;

public class DataUI : MonoBehaviour
{

    public static DataUI i;

    //--PROPIEDADES AJUSTABLES--
    public RecuadroInterfaz menuPrincipal;
    public RecuadroInterfaz seleccionNiveles;
    public RecuadroInterfaz pantallaCarga;
    public RecuadroInterfaz pantallaNivel;
    public RecuadroInterfaz pantallaEditor;

    public GameObject cuadroConfigCreador;
    public GameObject cuadroCreador;

    public Camera camaraUI;
    public Camera camaraMundo;
    public RenderTexture renderTexMundo;
    public QuadUI quadRenderMundo;
    public Material matRenderMundo;

    public Material cielo;
    public ReflectionProbe rpCielo;

    public Cubemap cieloPradera;
    public Cubemap cieloNieve;

    public float duracionCambioCielo = 1F;
    public float duracionEntradaMapaNivel = 0.25F;

    public Material[] matsOpcionDeseleccionada;
    public Material[] matsOpcionSeleccionada;

    public Material matTextoOpcionSeleccionada;
    public Material matTextoOpcionDeseleccionada;
    //--------------------------

    //--PROPIEDADES INTERNAS--
    [NonSerialized] public Cubemap cieloActual;
    [NonSerialized] public Cubemap cieloDestino;
    [NonSerialized] public float animCielo;
    [NonSerialized] public float animEntradaMapaNivel;
    [NonSerialized] public bool cieloCambiando;

    [NonSerialized] public bool nivelCargando;
    [NonSerialized] public RecuadroInterfaz recuadroActual;

    [NonSerialized] public bool generandoEnCreador;

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
        while (Time.fixedTime < tiempoInicio + duracionCambioCielo)
        {
            animCielo = (Time.fixedTime - tiempoInicio) / duracionCambioCielo;
            cielo.SetFloat("_Tiempo", animCielo);
            RenderSettings.customReflection = cieloDestino;
            yield return null;
        }
        RenderSettings.customReflection = cieloDestino;
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
        while (Time.fixedTime < tiempoInicio + duracionEntradaMapaNivel)
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
        if (recuadroActual == pantallaNivel)
        {
            quadRenderMundo.GetComponent<MeshRenderer>().enabled = true;
            camaraMundo.targetTexture = renderTexMundo;
            camaraMundo.clearFlags = CameraClearFlags.SolidColor;
        }
        if(recuadroActual == pantallaEditor)
        {
            ocultarPantallaNivel();
        }

        if (recuadroObjetivo == pantallaNivel)
        {
            camaraMundo.clearFlags = CameraClearFlags.Nothing;
            quadRenderMundo.GetComponent<MeshRenderer>().enabled = false;
            camaraMundo.targetTexture = null;
        }
        if (recuadroObjetivo == pantallaEditor)
        {
            abrirPantallaCreador();
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
        generandoEnCreador = false;
        nivelCargando = true;
        cambiarRecuadro(pantallaCarga);
        pantallaCarga.GetComponent<PantallaCarga>().iniciarCarga();
        DataJuego.i.mapaNivelPrincipal.empezarAGenerar(DataJuego.i.niveles[numeroNivel]);
    }

    public void nuevoNivelCreador(int ancho, int largo, int alto, bool es2D, MapaNivel.TipoPaisaje tipoPaisaje)
    {
        generandoEnCreador = true;
        nivelCargando = true;
        this.empezarCambiarCielo(tipoPaisaje);
        DataJuego.i.mapaNivelPrincipal.empezarAGenerarCreador(ancho, largo, alto,
            es2D ? MapaNivel.TipoMapaNivel.MAPA_2D : MapaNivel.TipoMapaNivel.MAPA_3D);
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

    public void pasarDeConfigMapaACreador()
    {
        cuadroConfigCreador.SetActive(false);
        cuadroCreador.SetActive(true);
    }

    public void abrirPantallaCreador()
    {
        cuadroConfigCreador.SetActive(true);
        cuadroCreador.SetActive(false);
    }

    public void ocultarPantallaNivel()
    {
        LayerManager.ocultarTodasLasCapas(camaraMundo);
    }

    public void irAPantallaNivel()
    {
        LayerManager.seleccionarCapas(camaraMundo, new string[] { "Default", "TransparentFX", "Ignore Raycast", "Water" });
        MapaNivel mn = DataJuego.i.mapaNivelPrincipal;
        camaraMundo.transform.position = mn.transform.parent.position + (Vector3.back * 100);
        camaraMundo.orthographicSize = Mathf.Max(new float[] { mn.dimensiones.x, mn.dimensiones.y, mn.dimensiones.z });
        camaraMundo.transform.localRotation = Quaternion.identity;
        camaraMundo.transform.RotateAround(mn.transform.parent.position, Vector3.right, 60);
        camaraMundo.transform.RotateAround(mn.transform.parent.position, Vector3.up, 180);
        StartCoroutine(corAnimarEntradaMapaNivel());

        if (!generandoEnCreador)
        {
            cambiarRecuadro(pantallaNivel);
        }
        else
        {
            pasarDeConfigMapaACreador();
        }
    }
    //------------
}
