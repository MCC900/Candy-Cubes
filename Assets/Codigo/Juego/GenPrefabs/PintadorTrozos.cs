using System;
using UnityEngine;

public static class PintadorTrozos
{
	public static void pintarTrozo(TrozoPieza trozo, Pieza.TipoPieza tipoPieza, bool[,,] mapaVecindad, int metadata){
		switch (tipoPieza) {

		case Pieza.TipoPieza.TERRENO:
			if (mapaVecindad [1, 2, 1]) { //Tiene vecino superior
				
			} else {
				
			}
			break;
		}
	}

	static bool checker(int x, int y, int z){
		return ((x % 2) + (y % 2) + (z % 2)) % 2 == 0;
	}
}