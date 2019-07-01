using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraProfunidad : MonoBehaviour
{
    Camera camara;
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        camara = GetComponent<Camera>();
        camara.depthTextureMode = DepthTextureMode.DepthNormals;
    }
}
