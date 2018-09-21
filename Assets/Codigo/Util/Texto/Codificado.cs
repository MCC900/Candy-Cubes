using System;

public static class Codificado {

	public static string comprimirCodigo(string codigo, string charsInput, string charsOutput){
		int bitsPorCaracterInput = cantidadBitsMinima(charsInput.Length-1);
		int bitsPorCaracterOutput = cantidadBitsMinima (charsOutput.Length-1);
		string concatBits = "";
		foreach (char c in codigo) {
			concatBits += aBits(charsInput.IndexOf(c), bitsPorCaracterInput);
		}
		int bitsResultado = concatBits.Length;
		if (bitsResultado % bitsPorCaracterOutput != 0) {
			bitsResultado += bitsPorCaracterOutput - (bitsResultado % bitsPorCaracterOutput);
		}
		while (concatBits.Length < bitsResultado) {
			concatBits += "0";
		}

		string resultado = "";
		int i = 0;
		while (i < concatBits.Length) {
			string trozo = concatBits.Substring (i, bitsPorCaracterOutput);
			int numero = Convert.ToInt32 (trozo, 2);
			resultado += charsOutput [numero];
			i += bitsPorCaracterOutput;
		}
		return resultado;
	}

	public static string descomprimirCodigo(string codificado, string charsInput, string charsOutput){
		int bitsPorCaracterInput = cantidadBitsMinima(charsInput.Length-1);
		int bitsPorCaracterOutput = cantidadBitsMinima (charsOutput.Length-1);
		string concatBits = "";
		foreach (char c in codificado) {
			concatBits += aBits(charsOutput.IndexOf(c), bitsPorCaracterOutput);
		}
		string resultado = "";
		int i = 0;
		while (i < concatBits.Length) {
			string trozo = concatBits.Substring (i, bitsPorCaracterInput);
			int numero = Convert.ToInt32 (trozo, 2);
			resultado += numero;
			i += bitsPorCaracterInput;
		}
		return resultado;
	}

	private static string aBits(int numero, int cantBits){
		string resultado = Convert.ToString (numero, 2);
		while (resultado.Length < cantBits) {
			resultado = "0" + resultado;
		}
		return resultado;
	}

	private static int cantidadBitsMinima(int numero){
		int resultado = 1;
		while (numero > 1) {
			resultado++;
			numero >>= 1;
		}
		return resultado;
	}
}
