using UnityEngine;
using System.Collections.Generic;
using System;

public static class MuestrarioPiezas
{
	static MuestrarioSubTrozos[][] muestrarios = new MuestrarioSubTrozos[Enum.GetValues (typeof(Pieza.TipoPieza)).Length][];

	public static GameObject[] getSubTrozos(bool[,,] mapaVecindad, Pieza.TipoPieza tipoPieza, int metadata){
		return muestrarios [(int)tipoPieza][metadata].armarTrozo(mapaVecindad);
	}

	public static void cargarMuestrario(Pieza.TipoPieza tipoPieza, int metadata, GameObject prefabMuestrario){
		switch (tipoPieza) {
		case Pieza.TipoPieza.CARAMELO:
			muestrarios[(int)tipoPieza] = new MuestrarioSubTrozos[4];
			muestrarios [(int)tipoPieza] [metadata] = muestrarioDesdePrefab (prefabMuestrario);
			break;
		default:
			Debug.LogError ("El TipoPieza " + Enum.GetName (typeof(Pieza.TipoPieza), tipoPieza) + " no se considera en la función cargarMuestrario()"); 
			break;
		}
	}

	static string[] nombresSubTrozos = {"PPP","PPN","PNP","PNN","NPP","NPN","NNP","NNN"};

	static MuestrarioSubTrozos muestrarioDesdePrefab(GameObject prefabMuestrario){
		Dictionary<int, GameObject[]> subTrozos = new Dictionary<int, GameObject[]> ();

		for (int i = 0; i < prefabMuestrario.transform.childCount; i++) {
			Transform hijo = prefabMuestrario.transform.GetChild (i);
			GameObject[] subSubTrozos = new GameObject[hijo.childCount];
			for (int j = 0; j < hijo.childCount; j++) {
				//subSubTrozos [j] = hijo.GetChild (j).gameObject;
				GameObject subSubTrozo = hijo.GetChild(j).gameObject;
				string siglasPosicion = subSubTrozo.name.Substring (subSubTrozo.name.Length - 3);
				for (int k = 0; k < nombresSubTrozos.Length; k++) {
					if (siglasPosicion == nombresSubTrozos [k]) {
						subSubTrozos [k] = subSubTrozo;
					}
				}
			}

			switch (hijo.name) {
			case "Esquinas":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.ESQUINAS, subSubTrozos);
				break;
			case "EjeX":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.EJE_X, subSubTrozos);
				break;
			case "EjeY":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.EJE_Y, subSubTrozos);
				break;
			case "EjeZ":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.EJE_Z, subSubTrozos);
				break;
			case "PlanoX":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.PLANO_X, subSubTrozos);
				break;
			case "PlanoY":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.PLANO_Y, subSubTrozos);
				break;
			case "PlanoZ":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.PLANO_Z, subSubTrozos);
				break;
			case "DoblezX":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.DOBLEZ_X, subSubTrozos);
				break;
			case "DoblezY":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.DOBLEZ_Y, subSubTrozos);
				break;
			case "DoblezZ":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.DOBLEZ_Z, subSubTrozos);
				break;
			case "Union":
				subTrozos.Add ((int)MuestrarioSubTrozos.TipoSubTrozo.UNION, subSubTrozos);
				break;
			}
		}

		return new MuestrarioSubTrozos(subTrozos);
	}

}