#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Common.hlsl"
#define kDielectricSpec half4(0.04, 0.04, 0.04, 1.0 - 0.04)

struct BRDFData
{
    half3 diffuse; // albedo * (1 - kDielectricSpec);
    half3 specular; // lerp(kDieletricSpec.rgb, albedo, metallic)
    half reflectivity; // 1 - (kDielectricSpec.a - metallic * kDielectricSpec)
    half perceptualRoughness; // perceptualRoughness
    half roughness; // perceptualRoughness * perceptualRoughness
    half roughness2; // roughness * roughness
    half grazingTerm; // saturate(1 - perceptualRoughness + reflectivity)

    half normalizationTerm;     // roughness * 4.0 + 2.0
    half roughness2MinusOne;    // roughness^2 - 1.0
};

inline void InitializeBRDFData(half3 albedo, half metallic, half perceptualRoughness, out BRDFData outBRDFData)
{

    half oneMinusReflectivity = kDielectricSpec.a - metallic * kDielectricSpec;
    half reflectivity = 1.0 - oneMinusReflectivity;
    half3 brdfDiffuse = albedo * oneMinusReflectivity;
    half3 brdfSpecular = lerp(kDielectricSpec.rgb, albedo, metallic);

    outBRDFData.diffuse = brdfDiffuse;
    outBRDFData.specular = brdfSpecular;
    outBRDFData.reflectivity = reflectivity;

    outBRDFData.perceptualRoughness = perceptualRoughness;
    outBRDFData.roughness           = max(perceptualRoughness * perceptualRoughness, HALF_MIN_SQRT);
    outBRDFData.roughness2          = max(outBRDFData.roughness * outBRDFData.roughness, HALF_MIN);
    outBRDFData.grazingTerm         = saturate(1 - perceptualRoughness + reflectivity);
    outBRDFData.normalizationTerm   = outBRDFData.roughness * 4.0h + 2.0h;
    outBRDFData.roughness2MinusOne  = outBRDFData.roughness2 - 1.0h;
}

half DirectBRDFSpecular(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
{
    float3 halfDir = SafeNormalize(float3(lightDirectionWS) + float3(viewDirectionWS));

    float NoH = saturate(dot(normalWS, halfDir));
    half LoH = saturate(dot(lightDirectionWS, halfDir));

    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;

    half LoH2 = LoH * LoH;
    half specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);

    return specularTerm;
}

half3 LightingPhysicallyBased(BRDFData brdfData, half3 lightColor, half3 lightDirectionWS, half3 normalWS, half3 viewDirectionWS)
{
    half NdotL = saturate(dot(normalWS, lightDirectionWS));
    half3 radiance = lightColor  * NdotL;

    half3 brdf = brdfData.diffuse;
    brdf += brdfData.specular * DirectBRDFSpecular(brdfData, normalWS, lightDirectionWS, viewDirectionWS);

    return brdf * radiance;
}

half3 LightingPass(InputData inputData)
{
    BRDFData brdfData;
    InitializeBRDFData(inputData.albedo, inputData.metallic, inputData.perceptualRoughness, brdfData);

    float3 color = LightingPhysicallyBased(brdfData, _MainLightColor, _MainLightDirection, inputData.normalWS, inputData.viewDirectionWS);

    return color;
}


