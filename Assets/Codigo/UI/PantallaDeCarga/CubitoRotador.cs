using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubitoRotador : MonoBehaviour
{
    public float velocidadRotacion = 1F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, velocidadRotacion, Space.Self);
    }
}
