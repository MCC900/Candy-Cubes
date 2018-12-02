using UnityEngine;

public class DataMuestrarios : MonoBehaviour {

	public GameObject terrenoPasto; 
	public GameObject carameloRojo;
	public GameObject carameloVerde;
	public GameObject carameloNaranja;

	public Material materialTierra;

	public void cargarMuestrarios(){
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_ROJO, 0, carameloRojo);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_VERDE, 0, carameloVerde);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.TERRENO_PASTO, 0, terrenoPasto);
	}
}
