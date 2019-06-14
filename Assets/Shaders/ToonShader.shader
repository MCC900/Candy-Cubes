// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Mariano/ToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
            	float3 posMundo:TEXCOORD0;
            	float2 uv: TEXCOORD1;
            	float2 uv2: TEXCOORD5;
            	half3 tEspacio0:TEXCOORD2;
            	half3 tEspacio1:TEXCOORD3;
            	half3 tEspacio2:TEXCOORD4;
            	half3 worldViewDir:TEXCOORD6;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _BumpMap;
            float4 _BumpMap_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posMundo = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _BumpMap);

                half3 wNormal = UnityObjectToWorldNormal(v.normal);
                half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
                half signoTangente = v.tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * signoTangente;
                o.tEspacio0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tEspacio1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tEspacio2 = half3(wTangent.z, wBitangent.z, wNormal.z);

                o.worldViewDir = normalize(UnityWorldSpaceViewDir(o.posMundo));
                //o.reflejo = reflect(-worldViewDir, wNormal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

            	half3 tNormal = UnpackNormal(tex2D(_BumpMap, i.uv2));
            	half3 worldNormal;
            	worldNormal.x = dot(i.tEspacio0, tNormal);
            	worldNormal.y = dot(i.tEspacio1, tNormal);
            	worldNormal.z = dot(i.tEspacio2, tNormal);
            	half3 reflejo = reflect(-i.worldViewDir, worldNormal);

            	half4 dataCielo = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflejo);
            	half3 colorCielo = DecodeHDR(dataCielo, unity_SpecCube0_HDR);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb =  colorCielo * col.rgb;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
