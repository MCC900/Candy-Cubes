using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMaterial : LayoutActualizable {

	public bool mantenerProporcionEsquina = true;
	public Sprite baseSprite;
	public float tamanoEsquinaX = 50F;
	public float tamanoEsquinaY = 50F;

	MeshGen meshGen = new MeshGen();


	[ContextMenu("Actualizar")]
	override public void actualizar(){
		crearMesh ();
	}

	void crearMesh(){
		Vector3[] esquinas = new Vector3[4];
		RectTransform rectTransform = GetComponent<RectTransform> ();
		rectTransform.GetLocalCorners (esquinas);

		Vector3 esqAbaIzq = esquinas[0];
		Vector3 esqArrIzq = esquinas[1];
		Vector3 esqArrDer = esquinas[2];
		Vector3 esqAbaDer = esquinas[3];
		float posZ = esqAbaIzq.z;

		float anchoTotal = esqAbaDer.x - esqAbaIzq.x;
		float altoTotal = esqArrDer.y - esqAbaDer.y;

		float tamEsqX = tamanoEsquinaX;
		float tamEsqY = tamanoEsquinaY;

		if (2 * tamEsqX >= anchoTotal) {
			tamEsqX = anchoTotal / 2;
		}
		if (2 * tamEsqY >= altoTotal) {
			tamEsqY = altoTotal / 2;
		}

		if (mantenerProporcionEsquina) {
			float tamMin = Mathf.Min (tamEsqX, tamEsqY);
			tamEsqX = tamMin;
			tamEsqY = tamMin;
		}
		float anchoCentroTex = baseSprite.texture.width - baseSprite.border.x * 2;
		float anchoCentroReal = (anchoCentroTex * tamEsqX) / baseSprite.border.x;
		float altoCentroTex = baseSprite.texture.width - baseSprite.border.y * 2;
		float altoCentroReal = (altoCentroTex * tamEsqY) / baseSprite.border.y;

		int cantRepeticionesX = (int)Mathf.Ceil ((anchoTotal - 2 * tamEsqX) / anchoCentroReal) + 2;
		int cantRepeticionesY = (int)Mathf.Ceil ((altoTotal  - 2 * tamEsqY) / altoCentroReal ) + 2;
		anchoCentroReal = (anchoTotal - tamEsqX - tamEsqX) / (cantRepeticionesX - 2);
		altoCentroReal = (altoTotal - tamEsqY - tamEsqY) / (cantRepeticionesY - 2);

		float borderXNorm = baseSprite.border.x / baseSprite.texture.width;
		float borderX2Norm = 1 - (baseSprite.border.z / baseSprite.texture.width);
		float borderYNorm = baseSprite.border.y / baseSprite.texture.height;
		float borderY2Norm = 1 - (baseSprite.border.w / baseSprite.texture.height);

		meshGen.nuevaMeshVertsDobles (cantRepeticionesX * cantRepeticionesY * 2);

		//ESQUINAS
		rectangulo (new Vector3 (esqArrIzq.x, esqArrIzq.y - tamEsqY, posZ), new Vector3 (esqArrIzq.x + tamEsqX, esqArrIzq.y, posZ),
			new Vector2 (0, borderY2Norm), new Vector2 (borderXNorm, 1));

		rectangulo (new Vector3 (esqArrDer.x - tamEsqX, esqArrDer.y - tamEsqY, posZ), new Vector3 (esqArrDer.x, esqArrDer.y, posZ),
			new Vector2 (borderX2Norm, borderY2Norm), new Vector2 (1, 1));

		rectangulo (new Vector3 (esqAbaIzq.x, esqAbaIzq.y, posZ), new Vector3 (esqAbaIzq.x + tamEsqX, esqAbaIzq.y + tamEsqY, posZ),
			new Vector2 (0, 0), new Vector2 (borderXNorm, borderYNorm));

		rectangulo (new Vector3 (esqAbaDer.x - tamEsqX, esqAbaDer.y, posZ), new Vector3 (esqAbaDer.x, esqAbaDer.y + tamEsqY, posZ),
			new Vector2 (borderX2Norm, 0), new Vector2 (1, borderYNorm));

		//TIRAS HORIZONTALES
		float xx = esqArrIzq.x + tamEsqX;
		float borderXDer = borderX2Norm;
		for (int x = 0; x < cantRepeticionesX - 2; x++) {
			float xDer = xx + anchoCentroReal;
			rectangulo (new Vector3(xx, esqArrIzq.y - tamEsqY, posZ), new Vector3(xDer, esqArrIzq.y, posZ),
				new Vector2(borderXNorm, borderY2Norm), new Vector2(borderXDer, 1));
			rectangulo (new Vector3(xx, esqAbaIzq.y, posZ), new Vector3(xDer, esqAbaIzq.y + tamEsqY, posZ),
				new Vector2(borderXNorm, 0), new Vector2(borderXDer, borderYNorm));
			xx += anchoCentroReal;
		}


		//TIRAS VERTICALES
		float yy = esqAbaIzq.y + tamEsqY;
		float borderYArr = borderY2Norm;
		for (int y = 0; y < cantRepeticionesY - 2; y++) {
			float yArr = yy + altoCentroReal;
			rectangulo (new Vector3(esqAbaIzq.x, yy, posZ), new Vector3(esqAbaIzq.x + tamEsqX, yArr, posZ),
				new Vector2(0, borderYNorm), new Vector2(borderXNorm, borderYArr));
			rectangulo (new Vector3(esqAbaDer.x - tamEsqX, yy, posZ), new Vector3(esqAbaDer.x, yArr, posZ),
				new Vector2(borderX2Norm, borderYNorm), new Vector2(1, borderYArr));
			yy += altoCentroReal;
		}

		//CENTRO
		yy = esqAbaIzq.y + tamEsqY;
		borderYArr = borderY2Norm;
		for (int y = 0; y < cantRepeticionesY - 2; y++) {
			float yArr = yy + altoCentroReal;
			xx = esqArrIzq.x + tamEsqX;
			borderXDer = borderX2Norm;
			for (int x = 0; x < cantRepeticionesX - 2; x++) {
				float xDer = xx + anchoCentroReal;
				rectangulo (new Vector3(xx, yy, posZ), new Vector3(xDer, yArr, posZ),
					new Vector2(borderXNorm, borderYNorm), new Vector2(borderXDer, borderYArr));
				xx += anchoCentroReal;
			}
			yy += altoCentroReal;
		}

		GetComponent<MeshFilter> ().mesh = meshGen.getMesh (true);
	}

	public void rectangulo(Vector3 esqAbaIzq, Vector3 esqArrDer, Vector2 uvAbaIzq, Vector2 uvArrDer){
		Vector3 esqArrIzq = new Vector3 (esqAbaIzq.x, esqArrDer.y, esqArrDer.z);
		Vector3 esqAbaDer = new Vector3 (esqArrDer.x, esqAbaIzq.y, esqArrDer.z);
		Vector2 uvArrIzq = new Vector2 (uvAbaIzq.x, uvArrDer.y);
		Vector2 uvAbaDer = new Vector2 (uvArrDer.x, uvAbaIzq.y);
		meshGen.quadConvexoAutoVert (esqArrIzq, esqArrDer, esqAbaDer, esqAbaIzq, uvArrIzq, uvArrDer, uvAbaDer, uvAbaIzq);
	}

}