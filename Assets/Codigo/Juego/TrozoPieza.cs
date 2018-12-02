using UnityEngine;

public class TrozoPieza : MonoBehaviour {

	public GameObject prefabMuestrario;

	public GameObject[] subTrozos; //En orden: PPP, PPN, PNP, PNN, NPP, NPN, NNP, NNN

	public void generarTrozoPieza(bool[,,] mapaVecindad, int metadata, Pieza.TipoPieza tipoPieza){
		GameObject[] subTrozosAClonar = MuestrarioPiezas.getSubTrozos (mapaVecindad, tipoPieza, metadata);
		subTrozos = new GameObject[subTrozosAClonar.Length];
		for(int i = 0; i < subTrozosAClonar.Length; i++) {
			GameObject go = subTrozosAClonar [i];
			if (go != null) {
				Quaternion rotGlobal = go.transform.rotation;
				GameObject inst = Instantiate (go, transform);
				inst.transform.rotation = rotGlobal;
				subTrozos [i] = inst;
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
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_ROJO, 0, prefabMuestrario);

		bool[,,] mapaVecindad = new bool[3, 3, 3];
		mapaVecindad [1, 2, 1] = true;
		mapaVecindad [1, 0, 1] = true;
		mapaVecindad [1, 1, 2] = true;

		generarTrozoPieza (mapaVecindad, 0, Pieza.TipoPieza.CARAMELO_ROJO);
	}
}
