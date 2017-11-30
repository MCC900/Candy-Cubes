using UnityEngine;
using System;

public class ObjetoRectUpdateChecker {
	IObjetoRectAutoajustable objetoRectMesh;
	RectTransform rectTransform;
	float anchoPrev;
	float altoPrev;

	public ObjetoRectUpdateChecker(IObjetoRectAutoajustable objetoRectMesh){
		this.objetoRectMesh = objetoRectMesh;
		this.rectTransform = (objetoRectMesh as MonoBehaviour).GetComponent<RectTransform> ();
		this.anchoPrev = this.rectTransform.rect.width;
		this.altoPrev = this.rectTransform.rect.height;
	}

	public void tryUpdate(){
		if (this.rectTransform.rect.width != this.anchoPrev || this.rectTransform.rect.height != this.altoPrev) {
			this.anchoPrev = this.rectTransform.rect.width;
			this.altoPrev = this.rectTransform.rect.height;
			this.objetoRectMesh.actualizarObjetoRectEditor();
		}
	}

	public void forceUpdate(){
		this.objetoRectMesh.actualizarObjetoRectEditor ();
	}

	public bool existeObjeto(){
		return this.objetoRectMesh != null && this.rectTransform != null;
	}
}
