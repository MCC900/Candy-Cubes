using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour {

	float proporcionBase;
	float ancho = 300;
	// Use this for initialization
	void Start () {
		inicializar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setAncho(float ancho) {
		this.ancho = ancho;
	}

	void inicializar() {
		RectTransform rt = this.GetComponent<RectTransform>();
		proporcionBase = rt.sizeDelta.x / rt.sizeDelta.y;
		rt.sizeDelta = new Vector2 (ancho, ancho / proporcionBase);
		//QuadPanel qp = GetComponent<QuadPanel> ();
		//qp.crearMesh ();
	}
}
