Shader "Mariano/ToonBordes"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_ColorBorde ("Color borde", Color) = (0,0,0,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DitherTex ("Dither Textura", 2D) = "" {}
		_AnchoDitherTex("Ancho texture dither", Float) = 4
		_CantColores("Bits colores", Float) = 3
		_Bias("Bias dither", Float) = 0.2
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_FarMenosNear ("Far menos Near", Float) = 1000
		_AnchoBorde ("Ancho borde", Float) = 2
		_DifProfMinima ("Diferencia Profundidad Min", Float) = 10
		_DifNormalMinima ("Diferencia Normal Min", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows finalcolor:mycolor addshadow
		
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		
        fixed4 _Color;
        fixed4 _ColorBorde;
        sampler2D _MainTex;
        sampler2D _DitherTex;
		float _AnchoDitherTex;
		float _CantColores;
		float _Bias;
        half _Glossiness;
        half _Metallic;
		float _FarMenosNear;
		float _AnchoBorde;
		float _DifProfMinima;
		float _DifNormalMinima;
		
        sampler2D _CameraDepthNormalsTexture;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DepthTex;
			float4 screenPos;
        };
		
		void mycolor (Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{

		  float unPixelX = IN.screenPos.w/_ScreenParams.x;
		  float unPixelY = IN.screenPos.w/_ScreenParams.y;
		  unPixelX = unPixelX * _AnchoBorde;
		  unPixelY = unPixelY * _AnchoBorde;
		  float4 dnTex = tex2D(_CameraDepthNormalsTexture, IN.screenPos.xy / IN.screenPos.w);
		  float4 dnTexXPos = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x+unPixelX, IN.screenPos.y         ) / IN.screenPos.w);
		  float4 dnTexYPos = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x         , IN.screenPos.y+unPixelY) / IN.screenPos.w);
		  float4 dnTexXNeg = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x-unPixelX, IN.screenPos.y         ) / IN.screenPos.w);
		  float4 dnTexYNeg = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x         , IN.screenPos.y-unPixelY) / IN.screenPos.w);
		  float4 dnTexXPosYPos = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x+unPixelX, IN.screenPos.y+unPixelY) / IN.screenPos.w);
		  float4 dnTexXPosYNeg = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x+unPixelX, IN.screenPos.y-unPixelY) / IN.screenPos.w);
		  float4 dnTexXNegYPos = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x-unPixelX, IN.screenPos.y+unPixelY) / IN.screenPos.w);
		  float4 dnTexXNegYNeg = tex2D(_CameraDepthNormalsTexture, float2(IN.screenPos.x-unPixelX, IN.screenPos.y-unPixelY) / IN.screenPos.w);
		  

		  float depth;
		  float depthXPos;
		  float depthYPos;
		  float depthXNeg;
		  float depthYNeg;
		  float depthXPosYPos;
		  float depthXPosYNeg;
		  float depthXNegYPos;
		  float depthXNegYNeg;

		  float3 normals;
		  float3 normalsXPos;
		  float3 normalsYPos;
		  float3 normalsXNeg;
		  float3 normalsYNeg;
		  float3 normalsXPosYPos;
		  float3 normalsXPosYNeg;
		  float3 normalsXNegYPos;
		  float3 normalsXNegYNeg;

		  DecodeDepthNormal(dnTex, depth, normals);
		  DecodeDepthNormal(dnTexXPos, depthXPos, normalsXPos);
		  DecodeDepthNormal(dnTexYPos, depthYPos, normalsYPos);
		  DecodeDepthNormal(dnTexXNeg, depthXNeg, normalsXNeg);
		  DecodeDepthNormal(dnTexYNeg, depthYNeg, normalsYNeg);
		  DecodeDepthNormal(dnTexXPosYPos, depthXPosYPos, normalsXPosYPos);
		  DecodeDepthNormal(dnTexXPosYNeg, depthXPosYNeg, normalsXPosYNeg);
		  DecodeDepthNormal(dnTexXNegYPos, depthXNegYPos, normalsXNegYPos);
		  DecodeDepthNormal(dnTexXNegYNeg, depthXNegYNeg, normalsXNegYNeg);

		  depth = depth * _FarMenosNear;
		  depthXPos = depthXPos * _FarMenosNear;
		  depthYPos = depthYPos * _FarMenosNear;
		  depthXNeg = depthXNeg * _FarMenosNear;
		  depthYNeg = depthYNeg * _FarMenosNear;
		  depthXPosYPos = depthXPosYPos * _FarMenosNear;
		  depthXPosYNeg = depthXPosYNeg * _FarMenosNear;
		  depthXNegYPos = depthXNegYPos * _FarMenosNear;
		  depthXNegYNeg = depthXNegYNeg * _FarMenosNear;
		  
		  bool borde = false;
		  bool xPosDepthMayor = depthXPos > depth;
		  bool xNegDepthMayor = depthXNeg > depth;
		  bool yPosDepthMayor = depthYPos > depth;
		  bool yNegDepthMayor = depthYNeg > depth;
		  bool xPosYPosDepthMayor = depthXPosYPos > depth;
		  bool xPosYNegDepthMayor = depthXPosYNeg > depth;
		  bool xNegYPosDepthMayor = depthXNegYPos > depth;
		  bool xNegYNegDepthMayor = depthXNegYNeg > depth;

		  borde = borde || (xPosDepthMayor ? depthXPos - depth > _DifProfMinima : false);
		  borde = borde || (xNegDepthMayor ? depthXNeg - depth > _DifProfMinima : false);
		  borde = borde || (yPosDepthMayor ? depthYPos - depth > _DifProfMinima : false);
		  borde = borde || (yNegDepthMayor ? depthYNeg - depth > _DifProfMinima : false);
		  borde = borde || (xPosYPosDepthMayor ? depthXPosYPos - depth > _DifProfMinima : false);
		  borde = borde || (xPosYNegDepthMayor ? depthXPosYNeg - depth > _DifProfMinima : false);
		  borde = borde || (xNegYPosDepthMayor ? depthXNegYPos - depth > _DifProfMinima : false);
		  borde = borde || (xNegYNegDepthMayor ? depthXNegYNeg - depth > _DifProfMinima : false);

		  float anguloXPos = acos(dot(normalize(normals), normalize(normalsXPos)));
		  float anguloXNeg = acos(dot(normalize(normals), normalize(normalsXNeg)));
		  float anguloYPos = acos(dot(normalize(normals), normalize(normalsYPos)));
		  float anguloYNeg = acos(dot(normalize(normals), normalize(normalsYNeg)));

		  bool bordeNormal = false;
		  bordeNormal = bordeNormal || anguloXPos > _DifNormalMinima;
		  bordeNormal = bordeNormal || anguloXNeg > _DifNormalMinima;
		  bordeNormal = bordeNormal || anguloYPos > _DifNormalMinima;
		  bordeNormal = bordeNormal || anguloYNeg > _DifNormalMinima;

		  float4 dither = tex2D(_DitherTex, float2((1/_AnchoDitherTex)*((IN.screenPos.x * _ScreenParams.x) % _AnchoDitherTex), (1/_AnchoDitherTex)*((IN.screenPos.y * _ScreenParams.y) % _AnchoDitherTex)));
		  float4 colorDither = color + ((dither-0.5) * _Bias);
		  if(borde || bordeNormal){
			color = _ColorBorde;
			color = round(color * _CantColores)/_CantColores;
		  } else {
			color = round(colorDither * _CantColores) / _CantColores;
		  }
		}

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
