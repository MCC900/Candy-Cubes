using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrillaSeleccion : MonoBehaviour {

	public GameObject botonSelNivel;
    public int paginaActual;

    GameObject[] botonesSelNivel;

	int maxBotones;
    int ultimoNivel;
    int cantPaginas;
    int cantBotones;

    // Use this for initialization
    void Start () {
		StartCoroutine(corLateStart());
	}
	IEnumerator corLateStart(){
		yield return new WaitForEndOfFrame ();
		maxBotones = getMaxCantBotones ();
        ultimoNivel = DataJuego.i.niveles.Length;
        cantPaginas = ultimoNivel / maxBotones;
        if(ultimoNivel % maxBotones != 0)
        {
            cantPaginas++;
        }
        paginaActual = 0;
        generarBotones ();
		numerarBotones ();
	}

	void generarBotones(){
        cantBotones = paginaActual + 1 < cantPaginas ? maxBotones : ultimoNivel % maxBotones;
		botonesSelNivel = new GameObject[cantBotones];

		botonesSelNivel [0] = Instantiate (botonSelNivel, this.transform);

		for (var i = 1; i < cantBotones; i++) {
            if(i > ultimoNivel){
                break;
            } else {
                botonesSelNivel[i] = Instantiate(botonesSelNivel[0], this.transform);
            }
		}
	}

	void numerarBotones(){
        int numPrimero = paginaActual * maxBotones + 1;

        for (var i = 0; i < cantBotones; i++) {
			TextMeshPro tmp = botonesSelNivel [i].GetComponentInChildren<TextMeshPro> ();
			tmp.text = (numPrimero + i).ToString ();

            botonesSelNivel[i].GetComponent<CuadritoSelNivel>().numeroNivel = i;
		}
	}

	int getMaxCantBotones(){
		
		RectTransform rt = GetComponent<RectTransform> ();
		GridLayoutGroup glg = GetComponent<GridLayoutGroup> ();
		//int cantx = Mathf.FloorToInt (rt.rect.width / glg.cellSize.x);
		//int canty = Mathf.FloorToInt (rt.rect.height / glg.cellSize.y);
		int cantx = 0;
		float cuentax = 0;
		while (true) {
			cuentax += glg.cellSize.x;
			if (cuentax > rt.rect.width) {
				break;
			} else {
				cantx++;
				cuentax += glg.spacing.x;
			}
		}
		int canty = 0;
		float cuentay = 0;
		while (true) {
			cuentay += glg.cellSize.y;
			if (cuentay > rt.rect.height) {
				break;
			} else {
				canty++;
				cuentay += glg.spacing.y;
			}
		}
		return cantx * canty;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
