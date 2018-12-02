using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ExportNormalMap {
	[MenuItem ("Mariano/Exportar mapa normal")]
	static void exportarMapaNormal(){
		Texture2D texSeleccionada = Selection.activeObject as Texture2D;
		if (texSeleccionada != null) {			
			string ruta = EditorUtility.SaveFilePanel (
				"Exportar mapa normal...",
				"",
				texSeleccionada.name + "_export",
				"png"
			);

			if (ruta != "") {
				string pathTex = AssetDatabase.GetAssetPath (texSeleccionada);
				TextureImporter texImp = (TextureImporter) AssetImporter.GetAtPath (pathTex);
				if (!texImp.isReadable) {
					texImp.isReadable = true;
					AssetDatabase.ImportAsset (pathTex, ImportAssetOptions.ForceUpdate);
				}

				byte[] texEncoded = texSeleccionada.EncodeToPNG ();
				System.IO.File.WriteAllBytes (ruta, texEncoded);
				AssetDatabase.Refresh ();
			}

		}
	}
}
