using UnityEngine;
using System;

[Serializable]
public class ValorUI {
	public enum TipoValorUI {FIJO_PX, FIJO_MM, PORCENTAJE_ANCHO, PORCENTAJE_ALTO}

	public TipoValorUI tipoValor = TipoValorUI.FIJO_PX;
	public float valor = 5F;
	public bool pixelPerfect = true;
	[HideInInspector]public RectTransform rectTransform = null;

	public static float resolucion = 1F;

	public ValorUI(TipoValorUI tipoValor, float valor){
		this.tipoValor = tipoValor;
		this.valor = valor;
	}

	public ValorUI(TipoValorUI tipoValor, float valor, RectTransform rectTransform){
		this.tipoValor = tipoValor;
		this.valor = valor;
		this.rectTransform = rectTransform;
	}

	public float getValorPx(){
		float valorPx;
		switch (tipoValor) {
		case TipoValorUI.FIJO_PX:
			valorPx = valor;
			break;
		case TipoValorUI.FIJO_MM:
			valorPx = valor * resolucion;
			break;
		case TipoValorUI.PORCENTAJE_ANCHO:
			valorPx = rectTransform.rect.width * (valor / 100F);
			break;
		//case TipoValorUI.PORCENTAJE_ALTO:
		default:
			valorPx = rectTransform.rect.height * (valor / 100F);
			break;
		}
		if (pixelPerfect) {
			return Mathf.Round (valorPx);
		} else {
			return valorPx;
		}
	}
}
