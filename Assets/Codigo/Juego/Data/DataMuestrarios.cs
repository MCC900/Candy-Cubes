using UnityEngine;

public class DataMuestrarios : MonoBehaviour {
    public static DataMuestrarios i;

	public GameObject terrenoPasto_0;
	public GameObject terrenoPasto_1;
	public GameObject terrenoPasto_2;

	public GameObject carameloRojo;
	public GameObject carameloVerde;
	public GameObject carameloNaranja;

	public Material materialTierra;

	[ContextMenu("Cargar")]
	public void cargarMuestrarios(){
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_ROJO, 0, carameloRojo);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_VERDE, 0, carameloVerde);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.TERRENO_PASTO, 0, terrenoPasto_0);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.TERRENO_PASTO, 1, terrenoPasto_1);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.TERRENO_PASTO, 2, terrenoPasto_2);
	}
}
