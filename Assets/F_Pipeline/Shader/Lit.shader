Shader "FPipeline/Lit"
{
    Properties
    {
        _Albedo ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)

        _PerceptualRoughness ("Roughness",  Range(0,1)) = 1
        _Metallic ("Metallic",  Range(0,1)) = 1
        
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", float) = 6
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
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "Assets/F_Pipeline/Shader/Library/LightInput.hlsl"
            #include "Assets/F_Pipeline/Shader/Library/Lighting.hlsl"

            ENDHLSL
        }
    }
}
