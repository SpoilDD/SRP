#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

struct InputData
{
    float3 normalWS;
    float3 viewDirectionWS;
    float4 albedo;
    float4 metallic;
    float perceptualRoughness;
};

CBUFFER_START(UnityPerMaterial)
    float4 _Albedo_ST;
    float4 _BaseColor;
    float _Metallic;
    float _PerceptualRoughness;
CBUFFER_END

TEXTURE2D(_Albedo);            SAMPLER(sampler_Albedo);