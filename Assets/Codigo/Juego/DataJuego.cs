using UnityEngine;

public class DataJuego : MonoBehaviour {
	public static DataJuego i;
	public static readonly string charsMetadata = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ#$";
	public static readonly string charsSeparadores = "/,;:";

	public DataMuestrarios dataMuestrarios;

	public void cargarDataMuestrarios(){
		dataMuestrarios.cargarMuestrarios ();
	}

	[ContextMenu("Despertar")]
	void Awake(){
		i = gameObject.GetComponent<DataJuego>();
	}
		
	public static int cantidadEstadosTipoPieza(Pieza.TipoPieza tipo){
		switch (tipo) {
		case Pieza.TipoPieza.CARAMELO_ROJO:
		case Pieza.TipoPieza.CARAMELO_VERDE:
		case Pieza.TipoPieza.CARAMELO_NARANJA:
			return 1; //1 único estado implica que tiene metadata
		case Pieza.TipoPieza.TERRENO_PASTO:
			return 2;
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
}
