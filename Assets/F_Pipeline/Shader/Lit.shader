Shader "FPipeline/Lit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Src Blend", float) = 1
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="FForwardPass"}
            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling

            #include "Assets/F_Pipeline/Shader/Library/Common.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
	            UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
	            UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
	            UNITY_SETUP_INSTANCE_ID(v);
	            UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
	            UNITY_SETUP_INSTANCE_ID(i);
                float4 col = tex2D(_MainTex, i.uv);
                float3 normalWS = i.normalWS;
                return dot(i.normalWS, float3(0,1,0));
            }
            ENDHLSL
        }
    }
}
