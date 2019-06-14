using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeJuego : MonoBehaviour
{
    RectTransform rectTransform;
    
    // Start is called before the first frame update
    void Awake()
    {
        this.rectTransform = GetComponent<RectTransform>();
        MapaNivel mapa = DataJuego.i.mapaNivelPrincipal;
        Transform anclaMapaNivel = mapa.transform.parent;

        Camera.main.transform.position = anclaMapaNivel.position - Vector3.forward * 100F;
        Camera.main.orthographicSize = Mathf.Max(new float[] { mapa.dimensiones.x, mapa.dimensiones.y, mapa.dimensiones.z });
    }
}
