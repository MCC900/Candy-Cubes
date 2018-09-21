using UnityEngine;
using System;

public class MapaNivel : MonoBehaviour {

	//=======VARIABLES PRIVADAS=======

	static char[] charsSeparadores = new char[]{'/',',','.'};

	enum TipoMapaNivel {MAPA_2D, MAPA_3D};
	TipoMapaNivel tipoMapaNivel;

	//-------------------GENERACION----------------------

	string mapaPrueba = "1/6,4,0//";

	/// <summary>
	/// Genera el MapaNivel a partir de su código explícito.
	/// </summary>
	/// <returns><c>true</c>, si el código es correcto y se generó el MapaNivel correctamente, <c>false</c> en caso contrario.</returns>
	/// <param name="codigo">Código explícito del MapaNivel.</param>
	bool generarDesdeCodigo(string codigo){
		//descomprimirCodigo (codigo);
		string[] split = codigo.Split (charsSeparadores[0]);
		if (split.Length <= 5) {
			logErrorConstruccion ("No hay suficientes datos separados, sólo hay " + split.Length, codigo);
			return false;
		}

		//------TIPO DE MAPA NIVEL (0 o 1)------
		string str_tipoMapaNivel = split [0];
		int int_tipoMapaNivel_leido;

		if (!int.TryParse (str_tipoMapaNivel, out int_tipoMapaNivel_leido)) {
			logErrorConstruccion ("Tipo de MapaNivel sólo puede ser 0 para Mapa_2D o 1 para Mapa_3D", codigo);
			return false;
		}

		TipoMapaNivel tipoMapaNivel_leido = (TipoMapaNivel)int_tipoMapaNivel_leido;

		//------DIMENSIONES DEL MAPA (x,y,z)------
		string str_dimensiones = split[1];
		string[] splitDimensiones = str_dimensiones.Split (charsSeparadores[1]);

		if ((tipoMapaNivel_leido == TipoMapaNivel.MAPA_2D && split.Length != 2) ||
		   (tipoMapaNivel_leido == TipoMapaNivel.MAPA_3D && split.Length != 3)) {
			logErrorConstruccion ("El tipo de MapaNivel especificado (" + tipoMapaNivel_leido + ") no corresponde con la cantidad de dimensiones especificada (" + split.Length + ")", codigo);
			return false;
		}

		string str_dimX = splitDimensiones [0];
		string str_dimY = splitDimensiones [1];

		int dimX_leido;
		int dimY_leido;
		int dimZ_leido;

		if (!int.TryParse (str_dimX, out dimX_leido)) {
			logErrorConstruccion ("Dimensión X del mapa no se traduce como un número entero", codigo);
			return false;
		}
		if (!int.TryParse (str_dimY, out dimY_leido)) {
			logErrorConstruccion ("Dimensión Y del mapa no se traduce como un número entero", codigo);
			return false;
		}
		if (tipoMapaNivel_leido == TipoMapaNivel.MAPA_3D) {
			string str_dimZ = splitDimensiones [2];
			if (!int.TryParse (str_dimZ, out dimZ_leido)) {
				logErrorConstruccion ("Dimensión Z del mapa no se traduce como un número entero", codigo);
				return false;
			}
		}

		//-----PIEZAS (...)-----

		string str_piezas = split[2];
		string[] splitPiezas = str_piezas.Split (charsSeparadores [1]);
		foreach (string pieza in splitPiezas) {
			string[] datos = pieza.Split (charsSeparadores [2]);
			if (!decodificarPieza (datos, tipoMapaNivel_leido)) {
				logErrorConstruccion ("Error al decodificar la pieza a partir del código '" + pieza + "'", codigo);
				return false;
			}
		}

		return false;	
	}

	bool decodificarPieza(string[] datosPieza, TipoMapaNivel tipoMapa){


		return true;
	}


	/// <summary>
	/// Comprime el código literal del MapaNivel y devuelve el resultado.
	/// </summary>
	/// <returns>El código comprimido.</returns>
	/// <param name="codigo">Código explícito del mapaNivel.</param>
	string comprimirCodigo(string codigo){
		
		return null;	
	}

	/// <summary>
	/// Recupera el código literal a partir del código comprimido del MapaNivel, a partir del cual luego se podrán leer los datos.
	/// </summary>
	/// <returns>Código explícito del MapaNivel.</returns>
	/// <param name="codigo">Codigo comprimido del MapaNivel.</param>
	string descomprimirCodigo(string codigo){
		return null;	

	}

	void logErrorConstruccion(string mensaje, string codigo){
		Debug.LogError ("Error al construír MapaNivel a partir del código " + codigo + ":"+mensaje);
	}

	[ContextMenu("Prueba")]
	void prueba(){
		string charsInput = "01";
		string charsOutput = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ#$";
		string test = Codificado.comprimirCodigo("0101101101110", charsInput, charsOutput);
		Debug.Log ("CODIFICADO: " + test);
		Debug.Log("DECODIFICADO: " + Codificado.descomprimirCodigo(test, charsInput, charsOutput));
	}
	

	//---------------------------------------------------
}
