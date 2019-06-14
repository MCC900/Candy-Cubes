// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mariano/CieloCambiante"
{
    Properties
    {
        [NoScaleOffset] _Cube ("Cielo 1", Cube) = "white" {}
        [NoScaleOffset] _Cube2 ("Cielo 2", Cube) = "white" {}
		_Tiempo("Factor t", Range (0, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" }

        Pass
        {
			ZWrite Off
			Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
			uniform samplerCUBE _Cube;
			uniform samplerCUBE _Cube2;
			uniform float _Tiempo;

            struct vertexInput
            {
                float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
            };

            struct vertexOutput
            {
                float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
            };


            vertexOutput vert (vertexInput v)
            {
                vertexOutput o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
            }

            fixed4 frag (vertexOutput vo) : COLOR
            {
				float3 tcoord = vo.texcoord;
				float4 c1;
				float4 c2;
				if(_Tiempo == 0){
					return texCUBE(_Cube, tcoord);
				} else if(_Tiempo == 1){
					return texCUBE(_Cube2, tcoord);
				} else {
					return texCUBE(_Cube, tcoord)*(1.0 -_Tiempo) + texCUBE(_Cube2, tcoord)*_Tiempo;
				}
            }
            ENDCG
        }
    }
}
