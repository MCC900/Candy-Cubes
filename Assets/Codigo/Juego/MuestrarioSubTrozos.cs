using UnityEngine;
using System.Collections.Generic;

public class MuestrarioSubTrozos
{
	public enum TipoSubTrozo{EJE_X, EJE_Y, EJE_Z, ESQUINAS, PLANO_X, PLANO_Y, PLANO_Z, DOBLEZ_X, DOBLEZ_Y, DOBLEZ_Z, UNION}

	Dictionary<int, GameObject[]> subTrozos;

	public MuestrarioSubTrozos (Dictionary<int, GameObject[]> subTrozos)
	{
		this.subTrozos = subTrozos;
	}

	public GameObject[] armarTrozo(bool[,,] m){
		GameObject PPP, PPN, PNP, PNN, NPP, NPN, NNP, NNN;
		PPP = PPN = PNP = PNN = NPP = NPN = NNP = NNN = null;

		bool[,,] vecindad = new bool[2, 2, 2];
		//vecindad[0,0,0] = m[1,1,1];

		//============ PPP ============
		vecindad[0,0,1] = m[1,1,2];
		vecindad[0,1,0] = m[1,2,1];
		vecindad[0,1,1] = m[1,2,2];
		vecindad[1,0,0] = m[2,1,1];
		vecindad[1,0,1] = m[2,1,2];
		vecindad[1,1,0] = m[2,2,1];
		vecindad[1,1,1] = m[2,2,2];
		PPP = getSubTrozoSegunVecindad (vecindad, 0);

		//============ PPN ============
		vecindad[0,0,1] = m[1,1,0];
		vecindad[0,1,0] = m[1,2,1];
		vecindad[0,1,1] = m[1,2,0];
		vecindad[1,0,0] = m[2,1,1];
		vecindad[1,0,1] = m[2,1,0];
		vecindad[1,1,0] = m[2,2,1];
		vecindad[1,1,1] = m[2,2,0];
		PPN = getSubTrozoSegunVecindad(vecindad, 1);
		
		//============ PNP ============
		vecindad[0,0,1] = m[1,1,2];
		vecindad[0,1,0] = m[1,0,1];
		vecindad[0,1,1] = m[1,0,2];
		vecindad[1,0,0] = m[2,1,1];
		vecindad[1,0,1] = m[2,1,2];
		vecindad[1,1,0] = m[2,0,1];
		vecindad[1,1,1] = m[2,0,2];
		PNP = getSubTrozoSegunVecindad(vecindad, 2);

		//============ PNN ============
		vecindad[0,0,1] = m[1,1,0];
		vecindad[0,1,0] = m[1,0,1];
		vecindad[0,1,1] = m[1,0,0];
		vecindad[1,0,0] = m[2,1,1];
		vecindad[1,0,1] = m[2,1,0];
		vecindad[1,1,0] = m[2,0,1];
		vecindad[1,1,1] = m[2,0,0];
		PNN = getSubTrozoSegunVecindad(vecindad, 3);

		//============ NPP ============
		vecindad[0,0,1] = m[1,1,2];
		vecindad[0,1,0] = m[1,2,1];
		vecindad[0,1,1] = m[1,2,2];
		vecindad[1,0,0] = m[0,1,1];
		vecindad[1,0,1] = m[0,1,2];
		vecindad[1,1,0] = m[0,2,1];
		vecindad[1,1,1] = m[0,2,2];
		NPP = getSubTrozoSegunVecindad (vecindad, 4);

		//============ NPN ============
		vecindad[0,0,1] = m[1,1,0];
		vecindad[0,1,0] = m[1,2,1];
		vecindad[0,1,1] = m[1,2,0];
		vecindad[1,0,0] = m[0,1,1];
		vecindad[1,0,1] = m[0,1,0];
		vecindad[1,1,0] = m[0,2,1];
		vecindad[1,1,1] = m[0,2,0];
		NPN = getSubTrozoSegunVecindad(vecindad, 5);

		//============ NNP ============
		vecindad[0,0,1] = m[1,1,2];
		vecindad[0,1,0] = m[1,0,1];
		vecindad[0,1,1] = m[1,0,2];
		vecindad[1,0,0] = m[0,1,1];
		vecindad[1,0,1] = m[0,1,2];
		vecindad[1,1,0] = m[0,0,1];
		vecindad[1,1,1] = m[0,0,2];
		NNP = getSubTrozoSegunVecindad(vecindad, 6);

		//============ NNN ============
		vecindad[0,0,1] = m[1,1,0];
		vecindad[0,1,0] = m[1,0,1];
		vecindad[0,1,1] = m[1,0,0];
		vecindad[1,0,0] = m[0,1,1];
		vecindad[1,0,1] = m[0,1,0];
		vecindad[1,1,0] = m[0,0,1];
		vecindad[1,1,1] = m[0,0,0];
		NNN = getSubTrozoSegunVecindad(vecindad, 7);
	
		return new GameObject[]{PPP, PPN, PNP, PNN, NPP, NPN, NNP, NNN};
	}
		
	GameObject getSubTrozoSegunVecindad(bool[,,] vecindadEsquina, int indicePosicion){
		GameObject[] posibilidades = null;

		//0 vecinos
		if (!vecindadEsquina [1, 0, 0] && !vecindadEsquina [0, 1, 0] && !vecindadEsquina [0, 0, 1]) {
			subTrozos.TryGetValue ((int)TipoSubTrozo.ESQUINAS, out posibilidades);

		//1 vecino
		} else if (vecindadEsquina [1, 0, 0] && !vecindadEsquina [0, 1, 0] && !vecindadEsquina [0, 0, 1]) {
			subTrozos.TryGetValue ((int)TipoSubTrozo.EJE_X, out posibilidades);

		} else if (!vecindadEsquina [1, 0, 0] && vecindadEsquina [0, 1, 0] && !vecindadEsquina [0, 0, 1]) {
			subTrozos.TryGetValue ((int)TipoSubTrozo.EJE_Y, out posibilidades);
		} else if (!vecindadEsquina [1, 0, 0] && !vecindadEsquina [0, 1, 0] && vecindadEsquina [0, 0, 1]) {
			subTrozos.TryGetValue ((int)TipoSubTrozo.EJE_Z, out posibilidades);
		
			//2 vecinos
		} else if (!vecindadEsquina [1, 0, 0] && vecindadEsquina [0, 1, 0] && vecindadEsquina [0, 0, 1]) {
			if (vecindadEsquina [0, 1, 1]) {
				subTrozos.TryGetValue ((int)TipoSubTrozo.PLANO_X, out posibilidades);
			} else {
				subTrozos.TryGetValue ((int)TipoSubTrozo.DOBLEZ_X, out posibilidades);
			}
		} else if (vecindadEsquina [1, 0, 0] && !vecindadEsquina [0, 1, 0] && vecindadEsquina [0, 0, 1]) {
			if (vecindadEsquina [1, 0, 1]) {
				subTrozos.TryGetValue ((int)TipoSubTrozo.PLANO_Y, out posibilidades);
			} else {
				subTrozos.TryGetValue ((int)TipoSubTrozo.DOBLEZ_Y, out posibilidades);
			}
		} else if (vecindadEsquina [1, 0, 0] && vecindadEsquina [0, 1, 0] && !vecindadEsquina [0, 0, 1]) {
			if (vecindadEsquina [1, 1, 0]) {
				subTrozos.TryGetValue ((int)TipoSubTrozo.PLANO_Z, out posibilidades);
			} else {
				subTrozos.TryGetValue ((int)TipoSubTrozo.DOBLEZ_Z, out posibilidades);
			}

			//3 vecinos
		} else {
				//0 vecinos-vecinos
			if (!(vecindadEsquina[1,1,0] || vecindadEsquina[1,0,1] || vecindadEsquina[0,1,1])) {
				subTrozos.TryGetValue ((int)TipoSubTrozo.UNION, out posibilidades);

				//1 vecinos-vecinos
			} else if(vecindadEsquina[0,1,1] && !vecindadEsquina[1,0,1] && !vecindadEsquina[1,1,0]) {
				subTrozos.TryGetValue((int)TipoSubTrozo.PLANO_X, out posibilidades);
			} else if(!vecindadEsquina[0,1,1] && vecindadEsquina[1,0,1] && !vecindadEsquina[1,1,0]){
				subTrozos.TryGetValue((int)TipoSubTrozo.PLANO_Y, out posibilidades);
			} else if (!vecindadEsquina[0,1,1] && !vecindadEsquina[1,0,1] && vecindadEsquina[1,1,0]){
				subTrozos.TryGetValue((int)TipoSubTrozo.PLANO_Z, out posibilidades);
			} //else { 3 vecinos-vecinos -> 
			//El subtrozo forma parte del interior de la pieza, por lo que es invisible, se devuelve null.
		}


		if (posibilidades != null) {
			return posibilidades [indicePosicion];
		} else {
			return null;
		}

	}



}

