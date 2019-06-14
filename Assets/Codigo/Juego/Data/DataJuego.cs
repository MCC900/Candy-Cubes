using UnityEngine;
using UnityEditor;
using System;

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
    public MapaNivel mapaNivelPrincipal;
    
    [NonSerialized][HideInInspector] public string[] niveles = {
        "1/6,5,2/1/0;6:5:2;111111:111111:111111:111111:111111:111111:100001:100001:100001:111111;000000:000000:000000:000000:000000:000000:000000:000000:000000:000000",
        "1/3,4,5/0/1;3:4:5;111:111:111:111:111:111:111:111:111:111:000:111:000:111:000:111:000:111:000:111",
        "1/3,4,5/1/1;3:4:5;111:111:111:111:111:111:111:111:111:111:000:111:000:111:000:111:000:111:000:111"
    };

    public DataMuestrarios dataMuestrarios;
	public DataUI dataUI;

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
                        DataUI.i = dj.dataUI;
                        DataMuestrarios.i = dj.dataMuestrarios;
						Debug.Log ("Instancia de DataJuego adquirida");
					}
				}
			}
		}
	}
	#endif

	[ContextMenu("Cargar data muestrarios")]
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
			return 3;
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

    //--EVENTOS--
	void Awake(){
		if (i == null) {
			i = gameObject.GetComponent<DataJuego>();
            DataUI.i = i.dataUI;
            DataMuestrarios.i = i.dataMuestrarios;
            Debug.Log ("Instancia de DataJuego adquirida");
		}
	}

    void Update()
    {
            
    }
}
