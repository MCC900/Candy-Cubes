using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectPorcentMinimo : LayoutActualizable {

	public float porcentajeAncho = 50;
	public float porcentajeAlto = 50;
	public float anchoMinimo = 300;
	public float altoMinimo = 300;

	[ContextMenu("Actualizar")]
	override public void actualizar(){
		RectTransform rt = transform.GetComponent<RectTransform> ();
		RectTransform rtPadre = transform.parent.GetComponent<RectTransform> ();
		float ancho;
		float alto;
		float anchoPadre = rtPadre.rect.width;
		float altoPadre = rtPadre.rect.height;

		if (anchoPadre <= anchoMinimo) {
			ancho = anchoPadre;
		} else {
			float porcentAncho = anchoPadre * (porcentajeAncho / 100F);
			if (porcentAncho <= anchoMinimo) {
				ancho = anchoMinimo;
			} else {
				ancho = porcentAncho;
			}
		}

		if (altoPadre <= altoMinimo) {
			alto = altoPadre;
		} else {
			float porcentAlto = altoPadre * (porcentajeAlto / 100F);
			if (porcentAlto <= altoMinimo) {
				alto = altoMinimo;
			} else {
				alto = porcentAlto;
			}
		}

		rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, ancho);
		rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, alto);

	}

	// Update is called once per frame
	void Update () {
		
	}
}
