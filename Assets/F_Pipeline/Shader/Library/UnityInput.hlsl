// #ifndef CUSTOM_UNITY_INPUT_INCLUDED
// #define CUSTOM_UNITY_INPUT_INCLUDED

CBUFFER_START(UnityPerDraw)
    // "Space" block feature
    float4x4 unity_ObjectToWorld;
    float4x4 unity_WorldToObject;
    float4 unity_LODFade;
    float4 unity_WorldTransformParams;

    // Render Layer block feature
    float4 unity_RenderingLayer;

    // Motion Vector block feature
    float4x4 unity_MatrixPreviousM;
    float4x4 unity_MatrixPreviousMI;
    // X : Use last frame positions (right now skinned meshes are the only objects that use this
    // Y : Force No Motion
    // Z : Z bias value
    // W : Camera only
    float4 unity_MotionVectorsParams;
CBUFFER_END

CBUFFER_START(MainLight)
    // DirectionalLight _MainLight; 
    float4 _MainLightDirection;
    float4 _MainLightColor;
CBUFFER_END

float4 _Time;
float4 _SinTime;
float4 _CosTime;
float4 unity_DeltaTime;
float4 _ProjectionParams;
float4 _FrameParams; // { FrameNum, TotalFrameNum, FrameNum % TotalFrameNum,  FrameNum % TotalFrameNum / TotalFrameNum }
float4 _JitterParams;
float4x4 unity_MatrixVP;
float4x4 unity_MatrixV;
float4x4 glstate_matrix_projection;
float4x4 unity_InvMatrixV;
float4x4 unity_InvMatrixVP;
float4x4 unity_MatrixPreviousVP;
float4x4 unity_InvMatrixPreviousVP;
float4x4 _NonJitteredMatrixVP;
float4x4 _InvNonJitteredMatrixVP;

// #endif