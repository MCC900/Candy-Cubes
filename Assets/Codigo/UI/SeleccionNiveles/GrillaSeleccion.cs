using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrillaSeleccion : MonoBehaviour {

	public GameObject botonSelNivel;
	GameObject[] botonesSelNivel;

	int maxBotones;

	// Use this for initialization
	void Start () {
		StartCoroutine(corLateStart());
	}
	IEnumerator corLateStart(){
		yield return new WaitForEndOfFrame ();
		maxBotones = getMaxCantBotones ();
		generarBotones ();
		numerarBotones (1);
	}

	void generarBotones(){
		botonesSelNivel = new GameObject[maxBotones];

		botonesSelNivel [0] = Instantiate (botonSelNivel, this.transform);

		for (var i = 1; i < maxBotones; i++) {
			botonesSelNivel[i] = Instantiate (botonesSelNivel[0], this.transform);
		}
	}

	void numerarBotones(int numPrimero){
		for (var i = 0; i < maxBotones; i++) {
			TextMeshPro tmp = botonesSelNivel [i].GetComponentInChildren<TextMeshPro> ();
			tmp.text = (numPrimero + i).ToString ();
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
