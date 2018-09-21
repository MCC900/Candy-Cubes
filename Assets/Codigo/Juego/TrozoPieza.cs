using UnityEngine;

public class TrozoPieza : MonoBehaviour {

	public GameObject prefabMuestrario;

	public void generarTrozoPieza(bool[,,] mapaVecindad, int metadata, Pieza.TipoPieza tipoPieza){
		GameObject[] subTrozos = MuestrarioPiezas.getSubTrozos (mapaVecindad, tipoPieza, metadata);
		foreach (GameObject go in subTrozos) {
			if (go != null) {
				Quaternion rotGlobal = go.transform.rotation;
				GameObject inst = Instantiate (go, transform);
				inst.transform.rotation = rotGlobal;
			}
		}
	}

	[ContextMenu("Limpiar")]
	void limpiar(){
		while (transform.childCount > 0) {
			DestroyImmediate (transform.GetChild (0).gameObject);
		}
	}

	[ContextMenu("PRUEBA")]
	void prueba(){
		limpiar ();
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO, 0, prefabMuestrario);

		bool[,,] mapaVecindad = new bool[3, 3, 3];
		mapaVecindad [1, 2, 1] = true;
		mapaVecindad [1, 0, 1] = true;
		mapaVecindad [1, 1, 2] = true;

		generarTrozoPieza (mapaVecindad, 0, Pieza.TipoPieza.CARAMELO);
	}
}
