using System;
using UnityEngine;

public static class PintadorTrozos
{
	public static void pintarTrozo(TrozoPieza trozo, Pieza.TipoPieza tipoPieza, bool[,,] mapaVecindad, int metadata){
		switch (tipoPieza) {

		case Pieza.TipoPieza.TERRENO_PASTO:
			if (mapaVecindad [1, 2, 1]) { //Tiene vecino superior
				//Borrar pasto
				foreach (GameObject subTrozo in trozo.subTrozos) {
					if (subTrozo != null) {
						MeshRenderer mr = subTrozo.GetComponent<MeshRenderer> ();
						Material[] sharedMats = new Material[mr.sharedMaterials.Length];
						for (int i = 0; i < sharedMats.Length; i++) {
							sharedMats [i] = DataJuego.i.dataMuestrarios.materialTierra;
						}
						mr.sharedMaterials = sharedMats;
					}
				}
			}
			break;
		}
	}
}