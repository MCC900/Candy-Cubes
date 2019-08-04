using UnityEngine;

public class DataMuestrarios : MonoBehaviour {
    public static DataMuestrarios i;

	public GameObject terrenoPasto_0;
	public GameObject terrenoPasto_1;

	public GameObject carameloRojo;
	public GameObject carameloVerde;
	public GameObject carameloNaranja;
    public GameObject carameloAzul;
    public GameObject carameloAmarillo;
    public GameObject carameloVioleta;

    public GameObject objetivoRojo;
    public GameObject objetivoVerde;
    public GameObject objetivoNaranja;
    public GameObject objetivoAzul;
    public GameObject objetivoAmarillo;
    public GameObject objetivoVioleta;

	[ContextMenu("Cargar")]
	public void cargarMuestrarios()
    {
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.TERRENO, 0, terrenoPasto_0);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.TERRENO, 1, terrenoPasto_1);
        MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_ROJO, 0, carameloRojo);
		MuestrarioPiezas.cargarMuestrario (Pieza.TipoPieza.CARAMELO_VERDE, 0, carameloVerde);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_NARANJA, 0, carameloNaranja);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_AZUL, 0, carameloAzul);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_AMARILLO, 0, carameloAmarillo);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.CARAMELO_VIOLETA, 0, carameloVioleta);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.OBJETIVO_ROJO, 0, objetivoRojo);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.OBJETIVO_VERDE, 0, objetivoVerde);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.OBJETIVO_NARANJA, 0, objetivoNaranja);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.OBJETIVO_AZUL, 0, objetivoAzul);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.OBJETIVO_AMARILLO, 0, objetivoAmarillo);
        MuestrarioPiezas.cargarMuestrario(Pieza.TipoPieza.OBJETIVO_VIOLETA, 0, objetivoVioleta);
    }
}
