using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PanelUIManager{
	public static List<PanelUIGrupoMats> gruposMaterial = new List<PanelUIGrupoMats>();

	public static PanelUIGrupoMats getGrupoMateriales(Material material, Vector4 bordes){
		foreach (PanelUIGrupoMats unPGM in gruposMaterial) {
			if (unPGM.matBase == null) {
				gruposMaterial.Remove (unPGM);
			} else {
				bool coincidenIDs = unPGM.matBase.GetInstanceID () == material.GetInstanceID ();
				if (coincidenIDs) {
					unPGM.setBordes (bordes);
					unPGM.checkearActualizarAssets ();
					return unPGM;
				}
			}
		}
		PanelUIGrupoMats pgm = new PanelUIGrupoMats (material);
		pgm.setBordes (bordes);
		gruposMaterial.Add (pgm);
		return pgm;
	}
}
