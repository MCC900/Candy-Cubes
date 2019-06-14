using UnityEngine;
using System.Collections;

public class PantallaCarga : MonoBehaviour
{
    public GameObject cubitoRotador;
    public float velocidadRotacion;

    bool cargando;

    IEnumerator corIniciarCarga()
    {
        cargando = true;
        cubitoRotador.transform.rotation = Quaternion.identity;
        while (cargando)
        {
            cubitoRotador.transform.Rotate(Vector3.up, velocidadRotacion, Space.Self);
            yield return null;
        }
    }

    public void iniciarCarga()
    {
        StartCoroutine(corIniciarCarga());
    }

    void terminarCarga()
    {
        cargando = false;
    }
    
}
