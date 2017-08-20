using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoAdaptativo : LayoutActualizable {

	public float altoBaseTexto = 100;
	public float coeficienteTamano = 8;

	[ContextMenu("Actualizar")]
	override public void actualizar(){
		TextMesh tm = GetComponent<TextMesh> ();
		RectTransform rtPadre = transform.parent.GetComponent<RectTransform> ();
		tm.characterSize = (rtPadre.rect.height / altoBaseTexto) * coeficienteTamano;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
