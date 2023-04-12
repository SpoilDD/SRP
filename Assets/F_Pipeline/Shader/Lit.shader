Shader "FPipeline/Lit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="FForwardPass"}
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Assets/F_Pipeline/Shader/Library/Common.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normalOS : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.normalOS = v.normal;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float3 normalWS = TransformObjectToWorldNormal(i.normalOS);
                return col;
            }
            ENDHLSL
        }
    }
}
