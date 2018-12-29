using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class DataJuego : MonoBehaviour {
	public static DataJuego i;

	public static readonly string charsMetadata = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ#$";
	public static readonly string charsSeparadores = "/,;:";

	public DataMuestrarios dataMuestrarios;

	#if UNITY_EDITOR
	static DataJuego(){
		EditorApplication.update += actualizarEditor;
	}

	static void actualizarEditor(){
		if (!EditorApplication.isPlaying) {
			if (i == null) {
				GameObject[] objetosRaiz = SceneManager.GetActiveScene ().GetRootGameObjects ();
				foreach (GameObject go in objetosRaiz) {
					DataJuego dj = go.GetComponent<DataJuego> ();
					if (dj != null) {
						i = dj;
						Debug.Log ("Instancia de DataJuego adquirida");
					}
				}
			}
		}
	}
	#endif

	public void cargarDataMuestrarios(){
		dataMuestrarios.cargarMuestrarios ();
	}
		
	public static int cantidadEstadosTipoPieza(Pieza.TipoPieza tipo){
		switch (tipo) {
		case Pieza.TipoPieza.CARAMELO_ROJO:
		case Pieza.TipoPieza.CARAMELO_VERDE:
		case Pieza.TipoPieza.CARAMELO_NARANJA:
			return 1; //1 único estado implica que no tiene metadata
		case Pieza.TipoPieza.TERRENO_PASTO:
			return 1;
		default:
			return 0;
		}
	}

	public static int metadataAInt(char charMetadata){
		for (int i = 0; i < charsMetadata.Length; i++) {
			if (charsMetadata [i] == charMetadata) {
				return i;
			}
		}
		return -1;
	}

	void Awake(){
		if (i == null) {
			i = gameObject.GetComponent<DataJuego>();
			Debug.Log ("Instancia de DataJuego adquirida");
		}
	}
}
