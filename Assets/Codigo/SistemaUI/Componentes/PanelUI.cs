using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PanelUI : MonoBehaviour, IObjetoRectAutoajustable {
	public enum TipoRepeticionPanelUI{REPETIR, AJUSTAR};


	public Material material;
	public Sprite spriteReferencia;
	public Vector2 tamanoEsquinaTopLeft = new Vector2(20,20);
	public TipoRepeticionPanelUI tipoRepeticion = TipoRepeticionPanelUI.AJUSTAR;
	public bool pixelPerfect = true;

	//======COMPONENTES======
	RectTransform rectTransform;
	MeshFilter meshFilter;
	MeshRenderer meshRenderer;

	//===MATERIALES GENERADOS===
	Material matEsquinas;
	Material matHorizontal;
	Material matVertical;
	Material matCentro;

	//=======CORUTINAS=======
	bool ejecActualizarMesh = false;

	//----------------EVENTOS UNITY-----------------------
	void Awake(){
		init ();
	}

	void Update(){
		this.ejecActualizarMesh = false;
	}

	//UI.Graphic
	void OnRectTransformDimensionsChange(){
		actualizarMesh ();
	}
		
	//---------------------------------------------------
	//-----------------INICIALIZACIÓN--------------------

	[ContextMenu("Init")]
	void init(){
		actualizarAsociarComponentes ();

		this.meshFilter.mesh = new Mesh ();
		actualizarMesh ();
	}

	//---------------------------------------------------
	//-----------------ACTUALIZACIÓN---------------------

	void actualizarMesh(){
		if (!ejecActualizarMesh) {
			this.ejecActualizarMesh = true;
			StartCoroutine(corActualizarMesh());
		}
	} //+------+//
	IEnumerator corActualizarMesh(){
		yield return new WaitForEndOfFrame ();
		this.generarMesh ();
		MeshGen.actualizarMesh (this.meshFilter.mesh, true);
	}

	[ContextMenu("Actualizar Mesh")]
	public void actualizarObjetoRectEditor(){
		this.actualizarAsociarComponentes ();
		this.generarSubMateriales ();
		this.generarMesh ();
		this.meshFilter.sharedMesh = MeshGen.getMesh (true);

	}

	[ContextMenu("Actualizar Texturado")]
	public void actualizarTexturadoEditor(){
		PanelUIManager.getGrupoMateriales (this.material, this.spriteReferencia.border).forzarActualizarAssets();
		this.actualizarObjetoRectEditor ();
	}
	[ContextMenu("Actualizar Materiales")]
	public void actualizarMaterialesEditor(){
		PanelUIManager.getGrupoMateriales (this.material, this.spriteReferencia.border).forzarActualizarMateriales();
		this.actualizarObjetoRectEditor ();
	}

	void actualizarAsociarComponentes(){
		this.rectTransform = GetComponent<RectTransform> ();
		this.meshFilter = GetComponent<MeshFilter> ();
		this.meshRenderer = GetComponent<MeshRenderer> ();
	}
	//---------------------------------------------------
	//-----------------AUTOGENERADO----------------------

	void generarMesh(){
		Vector3[] esquinas = new Vector3[4];
		this.rectTransform.GetLocalCorners (esquinas);
		//=== Diagrama de los vértices ===
		//
		//        x0    x1      x2    x3
		//
		//   y0   0 --- 1 ----- 2 --- 3
		//        |  A  |   B   |  C  |
		//   y1   4 --- 5 ----- 6 --- 7
		//        |  D  |   E   |  F  |
		//   y2   8 --- 9 -----10 ---11
		//        |  G  |   H   |  I  |
		//   y3  12 ---13 -----14 ---15
		//
		// esquinas[0] = 12, esquinas[1] = 0, esquinas[2] = 3, esquinas[3] = 15

		//Vector2 esqTopLeft = this.tamanoEsquinaTopLeft;
		
		//bool hayEspacioHorizontal;
		float minAncho = (spriteReferencia.border.z * this.tamanoEsquinaTopLeft.x) / spriteReferencia.border.x + this.tamanoEsquinaTopLeft.x;
		float minAlto = (spriteReferencia.border.y * this.tamanoEsquinaTopLeft.y) / spriteReferencia.border.w + this.tamanoEsquinaTopLeft.y;

		bool hayEspacioX = this.rectTransform.rect.width > minAncho;
		bool hayEspacioY = this.rectTransform.rect.height > minAlto;
		float xTopLeft, yTopLeft;

		if (!hayEspacioX) {
			xTopLeft = (this.tamanoEsquinaTopLeft.x * this.rectTransform.rect.width) / minAncho;
		} else {
			xTopLeft = this.tamanoEsquinaTopLeft.x;
		}

		if (!hayEspacioY) {
			yTopLeft = (this.tamanoEsquinaTopLeft.y * this.rectTransform.rect.height) / minAlto;
		} else {
			yTopLeft = this.tamanoEsquinaTopLeft.y;
		}

		Vector2 esqTopLeft = new Vector2 (xTopLeft, yTopLeft);
		Vector2 esqBottomRight = new Vector2 ((spriteReferencia.border.z * esqTopLeft.x) / spriteReferencia.border.x,
			(spriteReferencia.border.y * esqTopLeft.y) / spriteReferencia.border.w);

		float anchoTotalEsquinas = spriteReferencia.border.x + spriteReferencia.border.z;
		float altoTotalEsquinas = spriteReferencia.border.y + spriteReferencia.border.w;
		Vector2 uvsTL = new Vector2 (spriteReferencia.border.x / anchoTotalEsquinas, spriteReferencia.border.y / altoTotalEsquinas);

		float posZ = esquinas [0].z;

		float x0 = esquinas [1].x;
		float x1 = x0 + esqTopLeft.x;
		float x3 = esquinas [2].x;
		float x2 = x3 - esqBottomRight.x;

		float y0 = esquinas [1].y;
		float y1 = y0 - esqTopLeft.y;
		float y3 = esquinas [0].y;
		float y2 = y3 + esqBottomRight.y;

		if (this.pixelPerfect) {
			float offX = rectTransform.rect.center.x % 1.0F;
			float offY = rectTransform.rect.center.y % 1.0F;

			x0 = Mathf.Round (x0) - offX;
			x1 = Mathf.Round (x1) - offX;
			x2 = Mathf.Round (x2) - offX;
			x3 = Mathf.Round (x3) - offX;
			y0 = Mathf.Round (y0) - offY;
			y1 = Mathf.Round (y1) - offY;
			y2 = Mathf.Round (y2) - offY;
			y3 = Mathf.Round (y3) - offY;

		}

		Vector3 v0 = new Vector3 (x0, y0, posZ);
		Vector3 v1 = new Vector3 (x1, y0, posZ);
		Vector3 v2 = new Vector3 (x2, y0, posZ);
		Vector3 v3 = new Vector3 (x3, y0, posZ);

		Vector3 v4 = new Vector3 (x0, y1, posZ);
		Vector3 v5 = new Vector3 (x1, y1, posZ);
		Vector3 v6 = new Vector3 (x2, y1, posZ);
		Vector3 v7 = new Vector3 (x3, y1, posZ);

		Vector3 v8 = new Vector3 (x0, y2, posZ);
		Vector3 v9 = new Vector3 (x1, y2, posZ);
		Vector3 v10 = new Vector3 (x2, y2, posZ);
		Vector3 v11 = new Vector3 (x3, y2, posZ);

		Vector3 v12 = new Vector3 (x0, y3, posZ);
		Vector3 v13 = new Vector3 (x1, y3, posZ);
		Vector3 v14 = new Vector3 (x2, y3, posZ);
		Vector3 v15 = new Vector3 (x3, y3, posZ);

		MeshGen.nuevaMesh (36, 8);


		//=====ESQUINAS=====
		//Rectángulos A, C, G, I
		MeshGen.quadConvexoAutoVertsCuatro(v0, v1, v5, v4,
			new Vector2(0F,1F), new Vector2(uvsTL.x, 1F), new Vector2(uvsTL.x, uvsTL.y), new Vector2(0F, uvsTL.y));
		MeshGen.quadConvexoAutoVertsCuatro(v2, v3, v7, v6,
			new Vector2(uvsTL.x,1F), new Vector2(1F, 1F), new Vector2(1F, uvsTL.y), new Vector2(uvsTL.x, uvsTL.y));
		MeshGen.quadConvexoAutoVertsCuatro(v8, v9, v13, v12,
			new Vector2(0F,uvsTL.y), new Vector2(uvsTL.x, uvsTL.y), new Vector2(uvsTL.x, 0F), new Vector2(0F, 0F));
		MeshGen.quadConvexoAutoVertsCuatro(v10, v11, v15, v14,
			new Vector2(uvsTL.x,uvsTL.y), new Vector2(1F, uvsTL.y), new Vector2(1F, 0F), new Vector2(uvsTL.x, 0F));

		float repeticionesX = 0F;
		float repeticionesY = 0F;


		//=====LADOS HORIZONTALES=====
		MeshGen.anadirSubmesh (4);
		if (hayEspacioX) {
			float centroX = spriteReferencia.texture.width - spriteReferencia.border.x - spriteReferencia.border.z;
			centroX = (centroX * esqTopLeft.x) / spriteReferencia.border.x;
			repeticionesX = (this.rectTransform.rect.width - esqTopLeft.x - esqBottomRight.x) / centroX;
			if (this.tipoRepeticion == TipoRepeticionPanelUI.AJUSTAR) {
				repeticionesX = Mathf.Round (repeticionesX);
			}

			//Rectángulos B, H
			MeshGen.quadConvexoAutoVertsCuatro (v1, v2, v6, v5,
				new Vector2 (0F, 1F), new Vector2 (repeticionesX, 1F), new Vector2 (repeticionesX, uvsTL.y), new Vector2 (0F, uvsTL.y));
			MeshGen.quadConvexoAutoVertsCuatro (v9, v10, v14, v13,
				new Vector2 (0F, uvsTL.y), new Vector2 (repeticionesX, uvsTL.y), new Vector2 (repeticionesX, 0F), new Vector2 (0F, 0F));
		}
		//=====LADOS VERTICALES=====
		MeshGen.anadirSubmesh (4);
		if (hayEspacioY) {
			float centroY = spriteReferencia.texture.height - spriteReferencia.border.y - spriteReferencia.border.w;
			centroY = (centroY * esqTopLeft.y) / spriteReferencia.border.w;
			repeticionesY = (this.rectTransform.rect.height - esqTopLeft.y - esqBottomRight.y) / centroY;
			if (this.tipoRepeticion == TipoRepeticionPanelUI.AJUSTAR) {
				repeticionesY = Mathf.Round (repeticionesY);
			}

			//Rectángulos D, F
			MeshGen.quadConvexoAutoVertsCuatro (v4, v5, v9, v8,
				new Vector2 (0F, 1F), new Vector2 (uvsTL.x, 1F), new Vector2 (uvsTL.x, 1-repeticionesY), new Vector2 (0F, 1-repeticionesY));
			MeshGen.quadConvexoAutoVertsCuatro (v6, v7, v11, v10,
				new Vector2 (uvsTL.x, 1F), new Vector2 (1F, 1F), new Vector2 (1F, 1-repeticionesY), new Vector2 (uvsTL.x, 1-repeticionesY));
		}

		//======CENTRO======
		//Rectángulo E
		MeshGen.anadirSubmesh (2);
		if(hayEspacioX && hayEspacioY){
			MeshGen.quadConvexoAutoVertsCuatro(v5, v6, v10, v9,
				new Vector2(0F,1F), new Vector2(repeticionesX,1F), new Vector2(repeticionesX,1-repeticionesY), new Vector2(0F, 1-repeticionesY));
		}
	}

	void generarSubMateriales(){
		PanelUIGrupoMats pgm = PanelUIManager.getGrupoMateriales (this.material, this.spriteReferencia.border);
		this.matEsquinas = pgm.matEsquinas;
		this.matHorizontal = pgm.matHorizontal;
		this.matVertical = pgm.matVertical;
		this.matCentro = pgm.matCentro;

		Material[] mats = new Material[4];
		mats [0] = this.matEsquinas;
		mats [1] = this.matHorizontal;
		mats [2] = this.matVertical;
		mats [3] = this.matCentro;
		this.meshRenderer.sharedMaterials = mats;
	}

	//---------------------------------------------------
}
