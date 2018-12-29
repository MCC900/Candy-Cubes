#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class PanelUIGrupoMats {
	public Material matBase;
	public Texture2D[] texturasBase;

	Vector4 bordes;

	public string[] nombresPropiedadesMaterial;

	public Material matEsquinas;
	public Material matHorizontal;
	public Material matVertical;
	public Material matCentro;

	public Texture2D[] texsEsquinas;
	public Texture2D[] texsHorizontal;
	public Texture2D[] texsVertical;
	public Texture2D[] texsCentro;

	public PanelUIGrupoMats(Material matBase){
		this.matBase = matBase;
		this.renovarTexturas ();
		this.leerNombresPropiedadesMaterial ();
		this.checkearActualizarAssets ();
	}

	public void setBordes(Vector4 bordes){
		this.bordes = bordes;
	}

	public void renovarTexturas(){
		List<Texture2D> texturasMat = new List<Texture2D>();
		Shader s = this.matBase.shader;
		int cantPropiedades = ShaderUtil.GetPropertyCount(s);
		for (int i = 0; i < cantPropiedades; i++) {
			if (ShaderUtil.GetPropertyType (s, i) == ShaderUtil.ShaderPropertyType.TexEnv) {
				Texture2D tex = (Texture2D) this.matBase.GetTexture (ShaderUtil.GetPropertyName (s, i));
				if (tex != null) {
					texturasMat.Add (tex);
				}
			}
		}

		this.texturasBase = texturasMat.ToArray ();

		this.texsCentro = new Texture2D[texturasBase.Length];
		this.texsEsquinas = new Texture2D[texturasBase.Length];
		this.texsHorizontal = new Texture2D[texturasBase.Length];
		this.texsVertical = new Texture2D[texturasBase.Length];
	}

	void leerNombresPropiedadesMaterial(){
		Shader s = this.matBase.shader;
		int cuenta = 0;
		this.nombresPropiedadesMaterial = new string[ShaderUtil.GetPropertyCount (s)];

		for (int i = 0; i < nombresPropiedadesMaterial.Length; i++) {
			if (ShaderUtil.GetPropertyType (s, i) == ShaderUtil.ShaderPropertyType.TexEnv) {
				this.nombresPropiedadesMaterial [cuenta] = ShaderUtil.GetPropertyName (s, i);
				cuenta++;
			}
		}
		string[] trimArray = new string[cuenta];
		for (int i = 0; i < cuenta; i++) {
			trimArray [i] = this.nombresPropiedadesMaterial [i];
		}
		this.nombresPropiedadesMaterial = trimArray;
	}

	void generarTexturas(int indice){
		
		Texture2D t2D = texturasBase [indice];
		var ruta = AssetDatabase.GetAssetPath (t2D);
		TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath (ruta);
		TextureImporterSettings tis = new TextureImporterSettings ();
		TextureImporterSettings tis2 = new TextureImporterSettings ();
		ti.ReadTextureSettings (tis);
		ti.ReadTextureSettings (tis2);
		tis2.ApplyTextureType (TextureImporterType.Default);
		ti.SetTextureSettings (tis2);
		ti.SaveAndReimport ();

		int intBX = (int)bordes.x;
		int intBY = (int)bordes.y;
		int intBZ = (int)bordes.z;
		int intBW = (int)bordes.w;

		//Textura Esquinas

		this.texsEsquinas[indice] = new Texture2D (intBX + intBZ, intBY + intBW);

		this.texsEsquinas[indice].SetPixels(0, 0, intBX, intBY,
			texturasBase[indice].GetPixels(0, 0, intBX, intBY));
		this.texsEsquinas[indice].SetPixels(intBX, 0, intBZ, intBY,
			texturasBase[indice].GetPixels(this.texturasBase[indice].width - intBZ, 0, intBZ, intBY));
		this.texsEsquinas[indice].SetPixels (0, intBY, intBX, intBW,
			texturasBase[indice].GetPixels (0, this.texturasBase[indice].height - intBW, intBX, intBW));
		this.texsEsquinas[indice].SetPixels (intBX, intBY, intBZ, intBW,
			texturasBase[indice].GetPixels (this.texturasBase[indice].width - intBZ, this.texturasBase[indice].height - intBW, intBZ, intBW));

		this.texsEsquinas[indice].wrapMode = TextureWrapMode.Clamp;
		//Textura Horizontal

		int ancho = this.texturasBase[indice].width - intBX - intBZ;
		this.texsHorizontal[indice] = new Texture2D (ancho, intBY + intBW);

		this.texsHorizontal[indice].SetPixels (0, 0, ancho, intBY,
			this.texturasBase[indice].GetPixels (intBX, 0, ancho, intBY));
		this.texsHorizontal[indice].SetPixels (0, intBY, ancho, intBW,
			this.texturasBase[indice].GetPixels (intBX, this.texturasBase[indice].height - intBW, ancho, intBW));
		
		this.texsHorizontal[indice].wrapMode = TextureWrapMode.Repeat;

		//Textura Vertical

		int alto = this.texturasBase[indice].height - intBY - intBW;
		this.texsVertical[indice] = new Texture2D (intBX + intBZ, alto);

		this.texsVertical[indice].SetPixels (0, 0, intBX, alto,
			this.texturasBase[indice].GetPixels (0, intBY, intBX, alto));
		this.texsVertical[indice].SetPixels (intBX, 0, intBZ, alto,
			this.texturasBase[indice].GetPixels (this.texturasBase[indice].width - intBZ, intBY, intBZ, alto));
		
		this.texsVertical[indice].wrapMode = TextureWrapMode.Repeat;

		//Textura Centro
		this.texsCentro[indice] = new Texture2D (ancho, alto);

		this.texsCentro[indice].SetPixels (0, 0, ancho, alto,
			this.texturasBase[indice].GetPixels (intBX, intBY, ancho, alto));
		
		this.texsCentro[indice].wrapMode = TextureWrapMode.Repeat;

		if (tis.convertToNormalMap) {
			this.texsCentro [indice] = NormalMap (this.texsCentro [indice], tis.heightmapScale);
			this.texsEsquinas [indice] = NormalMap (this.texsEsquinas [indice], tis.heightmapScale);
			this.texsHorizontal [indice] = NormalMap (this.texsHorizontal [indice], tis.heightmapScale);
			this.texsVertical [indice] = NormalMap (this.texsVertical [indice], tis.heightmapScale);
		}

		ti.SetTextureSettings (tis);
		ti.SaveAndReimport ();
		crearAssetsTextura (indice);
	}

	void generarMateriales(){
		this.matEsquinas = new Material (this.matBase);
		this.matHorizontal = new Material (this.matBase);
		this.matVertical = new Material (this.matBase);
		this.matCentro = new Material (this.matBase);

		this.crearAssetsMaterial ();
		this.asignarTexturasAMaterial ();
	}

	string getCrearRutaAutoGenSpriteBase(int indice){
		string ruta = AssetDatabase.GetAssetPath (this.texturasBase[indice]);
		ruta = ruta.Substring (0, ruta.LastIndexOf ('/')) + "/PanelUIAutoGen";
		Directory.CreateDirectory (ruta);
		ruta += "/" + this.texturasBase[indice].name + "_";
		return ruta;
	}

	string getCrearRutaAutoGenMatBase(){
		string ruta = AssetDatabase.GetAssetPath (this.matBase);
		ruta = ruta.Substring (0, ruta.LastIndexOf ('/')) + "/PanelUIAutoGen";
		Directory.CreateDirectory (ruta);
		ruta += "/" + this.matBase.name + "_";
		return ruta;
	}

	void crearAssetsTextura(int indice){
		string ruta = this.getCrearRutaAutoGenSpriteBase (indice);

		guardarTextura (ref this.texsEsquinas[indice], ref this.texturasBase[indice], ruta + "Esquinas.png");
		guardarTextura (ref this.texsHorizontal[indice], ref this.texturasBase[indice], ruta + "Horizontal.png");
		guardarTextura (ref this.texsVertical[indice], ref this.texturasBase[indice], ruta + "Vertical.png");
		guardarTextura (ref this.texsCentro[indice], ref this.texturasBase[indice], ruta + "Centro.png");
	}

	void crearAssetsMaterial(){
		string ruta = this.getCrearRutaAutoGenMatBase ();

		AssetDatabase.CreateAsset (matEsquinas, ruta + "Esquinas.mat");
		AssetDatabase.CreateAsset (matHorizontal, ruta + "Horizontal.mat");
		AssetDatabase.CreateAsset (matVertical, ruta + "Vertical.mat");
		AssetDatabase.CreateAsset (matCentro, ruta + "Centro.mat");
	}

	void asignarTexturasAMaterial(){
		if (this.matEsquinas != null) {
			int cuenta = 0;
			for (int i = 0; i < this.texturasBase.Length; i++) {
				while (cuenta < nombresPropiedadesMaterial.Length) {
					if (matBase.GetTexture (nombresPropiedadesMaterial [cuenta]) != null) {
						this.matEsquinas.SetTexture (nombresPropiedadesMaterial [cuenta], this.texsEsquinas [i]);
						this.matHorizontal.SetTexture (nombresPropiedadesMaterial [cuenta], this.texsHorizontal [i]);
						this.matVertical.SetTexture (nombresPropiedadesMaterial [cuenta], this.texsVertical [i]);
						this.matCentro.SetTexture (nombresPropiedadesMaterial [cuenta], this.texsCentro [i]);
						cuenta++;
						break;
					} else {
						cuenta++;
					}
				}
			}
		}
	}

	void guardarTextura(ref Texture2D t2D, ref Texture2D tBase, string ruta){

		byte[] bytes = t2D.EncodeToPNG ();

		FileStream fs = new FileStream (ruta, FileMode.OpenOrCreate, FileAccess.Write);
		BinaryWriter bw = new BinaryWriter (fs);
		for (int i = 0; i < bytes.Length; i++) {
			bw.Write(bytes[i]);
		}
		bw.Close();
		fs.Close();

		TextureWrapMode twm = t2D.wrapMode;

		AssetDatabase.ImportAsset (ruta);
		t2D = AssetDatabase.LoadAssetAtPath<Texture2D>(ruta);

		string rutabase = AssetDatabase.GetAssetPath (tBase);
		TextureImporter TIbase = (TextureImporter)TextureImporter.GetAtPath(rutabase);
		TextureImporter TInueva = (TextureImporter)TextureImporter.GetAtPath (ruta);

		TextureImporterSettings tis = new TextureImporterSettings();
		TIbase.ReadTextureSettings (tis);
		tis.wrapMode = twm;
		tis.convertToNormalMap = false;
		TInueva.SetTextureSettings (tis);
		TInueva.SaveAndReimport ();
	}

	public void checkearActualizarAssets(){
		string ruta;
		bool reasignarTexturas = false;
		for (int i = 0; i < this.texturasBase.Length; i++) {
			ruta = this.getCrearRutaAutoGenSpriteBase (i);

			bool existenTexturas = true;
			existenTexturas &= File.Exists (ruta + "Esquinas.png");
			existenTexturas &= File.Exists (ruta + "Horizontal.png");
			existenTexturas &= File.Exists (ruta + "Vertical.png");
			existenTexturas &= File.Exists (ruta + "Centro.png");

			if (!existenTexturas) {
				this.generarTexturas (i);
				reasignarTexturas = true;
			} else {
				if (this.texsCentro[i] == null || this.texsEsquinas[i] == null || this.texsHorizontal[i] == null || this.texsVertical[i] == null) {
					reasignarTexturas = true;
					this.texsEsquinas[i] = AssetDatabase.LoadAssetAtPath<Texture2D> (ruta + "Esquinas.png");
					this.texsHorizontal[i] = AssetDatabase.LoadAssetAtPath<Texture2D> (ruta + "Horizontal.png"); 
					this.texsVertical[i] = AssetDatabase.LoadAssetAtPath<Texture2D> (ruta + "Vertical.png"); 
					this.texsCentro[i] = AssetDatabase.LoadAssetAtPath<Texture2D> (ruta + "Centro.png"); 
				}
			}
		}
		if (reasignarTexturas) {
			this.asignarTexturasAMaterial ();
		}

		ruta = this.getCrearRutaAutoGenMatBase ();

		bool existenMats = true;
		existenMats &= File.Exists (ruta + "Esquinas.mat");
		existenMats &= File.Exists (ruta + "Horizontal.mat");
		existenMats &= File.Exists (ruta + "Vertical.mat");
		existenMats &= File.Exists (ruta + "Centro.mat");

		if (!existenMats) {
			this.generarMateriales ();
		} else {
			if (this.matCentro == null || this.matEsquinas == null || this.matHorizontal == null || this.matVertical == null) {
				this.matEsquinas = AssetDatabase.LoadAssetAtPath<Material> (ruta + "Esquinas.mat");
				this.matHorizontal = AssetDatabase.LoadAssetAtPath<Material> (ruta + "Horizontal.mat");
				this.matVertical = AssetDatabase.LoadAssetAtPath<Material> (ruta + "Vertical.mat");
				this.matCentro = AssetDatabase.LoadAssetAtPath<Material> (ruta + "Centro.mat");
			}
		}

	}

	public void forzarActualizarAssets(){
		this.renovarTexturas ();
		for (var i = 0; i < this.texturasBase.Length; i++) {
			this.generarTexturas (i);
		}
		this.generarMateriales ();
	}

	public void forzarActualizarMateriales(){
		this.generarMateriales ();
	}

	/*
	La siguiente función para convertir heightmaps en normalmaps fue obtenida de una de las respuestas en:
	https://gamedev.stackexchange.com/questions/106703/create-a-normal-map-using-a-script-unity
	
	Solución provista por el usuario "Hash Buoy" del foro.
	Como única modificación, la línea de código que exporta el mapa normal
	automáticamente fue //comentada ya que no lo necesito en este caso.
	*/
	/*
	private Texture2D NormalMap(Texture2D source,float strength) 
	{
		strength=Mathf.Clamp(strength,0.0F,1.0F);

		Texture2D normalTexture;
		float xLeft;
		float xRight;
		float yUp;
		float yDown;
		float yDelta;
		float xDelta;

		normalTexture = new Texture2D (source.width, source.height, TextureFormat.ARGB32, true);

		for (int y=0; y<normalTexture.height; y++) 
		{
			for (int x=0; x<normalTexture.width; x++) 
			{
				xLeft = source.GetPixel(x-1,y).grayscale*strength;
				xRight = source.GetPixel(x+1,y).grayscale*strength;
				yUp = source.GetPixel(x,y-1).grayscale*strength;
				yDown = source.GetPixel(x,y+1).grayscale*strength;
				xDelta = ((xLeft-xRight)+1)*0.5f;
				yDelta = ((yUp-yDown)+1)*0.5f;
				normalTexture.SetPixel(x,y,new Color(xDelta,yDelta,1.0f,yDelta));
			}
		}
		normalTexture.Apply();

		//Code for exporting the image to assets folder
		System.IO.File.WriteAllBytes( "Assets/Source"+UnityEngine.Random.value+".png", normalTexture.EncodeToPNG());

		return normalTexture;
	}
	*/

	public Texture2D NormalMap(Texture2D aTexture, float strength) {
		Texture2D normalTexture = new Texture2D(aTexture.width, aTexture.height, TextureFormat.RGB24, aTexture.mipmapCount > 1);
		Color[] pixels = aTexture.GetPixels(0);
		Color[] nPixels = new Color[pixels.Length];
		for (int y=0; y<aTexture.height; y++) {
			for (int x=0; x<aTexture.width; x++) {
				int x_1 = x-1;
				if(x_1 < 0) x_1 = aTexture.width - 1; // repeat the texture so use the opposit side
				int x1 = x+1;
				if(x1 >= aTexture.width) x1 = 0; // repeat the texture so use the opposit side
				int y_1 = y-1;
				if(y_1 < 0) y_1 = aTexture.height - 1; // repeat the texture so use the opposit side
				int y1 = y+1;
				if(y1 >= aTexture.height) y1 = 0; // repeat the texture so use the opposit side
				float grayX_1 = pixels[(y * aTexture.width) + x_1].grayscale;
				float grayX1 = pixels[(y * aTexture.width) + x1].grayscale;
				float grayY_1 = pixels[(y_1 * aTexture.width) + x].grayscale;
				float grayY1 = pixels[(y1 * aTexture.width) + x].grayscale;
				Vector3 vx = new Vector3(1, 0, (grayX1 - grayX_1) * strength * 50F);
				Vector3 vy = new Vector3(0, 1, (grayY1 - grayY_1) * strength * 50F);
				Vector3 n = Vector3.Cross(vx, vy).normalized;
				nPixels[(y * aTexture.width) + x] = (Vector4)((n + Vector3.one) * 0.5f);
			}
		}
		normalTexture.SetPixels(nPixels, 0);
		normalTexture.Apply(true);
		return normalTexture;
	}

}
#endif