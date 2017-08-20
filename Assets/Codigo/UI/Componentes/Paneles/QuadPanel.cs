using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuadPanel : LayoutActualizable {

	MeshGen meshGen = new MeshGen();

	[ContextMenu("Actualizar")]
	override public void actualizar(){
		crearMesh ();
	}

	public void crearMesh(){
		Vector3[] esquinas = new Vector3[4];
		RectTransform rectTransform = GetComponent<RectTransform> ();
		rectTransform.GetLocalCorners (esquinas);

		MeshFilter mf = GetComponent<MeshFilter> ();

		meshGen.nuevaMesh (4, 2);
		meshGen.vertice (esquinas [0]);
		meshGen.vertice (esquinas [1]);
		meshGen.vertice (esquinas [2]);
		meshGen.vertice (esquinas [3]);
		meshGen.triangulo (0, 1, 2, new Vector2(0,0), new Vector2(0,1), new Vector2(1, 1));
		meshGen.triangulo (0, 2, 3, new Vector2(0,0), new Vector2(1,1), new Vector2(1, 0));
		mf.mesh = meshGen.getMesh (true);

	}


}
