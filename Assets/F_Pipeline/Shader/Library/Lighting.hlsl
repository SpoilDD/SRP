#include "BRDF.hlsl"
#include "Common.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv :TEXCOORD0;
    float3 normalOS : NORMAL;
    float3 tangentOS : TANGENT;
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv :TEXCOORD0;
    float3 normalWS : TEXCOORD1;
    float3 tangentWS : TEXCOORD2;
    float3 positionWS : TEXCOORD3;
};



void InitInputData(Varyings i, out InputData inputData)
{
    float4 albedo = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, i.uv) * _BaseColor;
    inputData.albedo = albedo;
    inputData.normalWS = normalize(i.normalWS);
    inputData.viewDirectionWS = GetViewPositionWS(i.positionWS);
    inputData.perceptualRoughness = _PerceptualRoughness;
    inputData.metallic = _Metallic;
}

Varyings LitPassVertex (Attributes i) 
{
    Varyings o = (Varyings)0;
    o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
    o.normalWS = TransformObjectToWorldNormal(i.normalOS);
    o.tangentWS = TransformObjectToWorldDir(i.tangentOS);
    o.positionWS = TransformObjectToWorld(i.positionOS.xyz);
    o.uv = TRANSFORM_TEX(i.uv, _Albedo);
    return o;
}

float4 LitPassFragment (Varyings i) : SV_TARGET 
{
    InputData inputData;
    InitInputData(i, inputData);

    float4 color = float4(0,0,0,1);
    color.rgb = LightingPass(inputData);
    return color;
}