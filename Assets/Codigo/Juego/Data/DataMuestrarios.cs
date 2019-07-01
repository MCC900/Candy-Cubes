using UnityEngine;

public class DataMuestrarios : MonoBehaviour {
    public static DataMuestrarios i;

	public GameObject terrenoPasto_0;
	public GameObject terrenoPasto_1;
	public GameObject terrenoPasto_2;

	public GameObject carameloRojo;
	public GameObject carameloVerde;
	public GameObject carameloNaranja;
    public GameObject carameloAzul;
    public GameObject carameloAmarillo;
    public GameObject carameloVioleta;

    public GameObject objetivoRojo;

	public Material materialTierra;

	[ContextMenu("Cargar")]
	public void cargarMuestrarios(){
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_ROJO, 0, carameloRojo);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_VERDE, 0, carameloVerde);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_NARANJA, 0, carameloNaranja);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_AZUL, 0, carameloAzul);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_AMARILLO, 0, carameloAmarillo);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_VIOLETA, 0, carameloVioleta);
        MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.TERRENO_PASTO, 0, terrenoPasto_0);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.TERRENO_PASTO, 1, terrenoPasto_1);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.TERRENO_PASTO, 2, terrenoPasto_2);
        MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.OBJETIVO_ROJO, 0, objetivoRojo);
    }
}
