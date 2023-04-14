struct VertexInput
{
    float4 pos : POSITION;
    float3 normalOS : NORMAL;
};

struct VertexOutput
{
    float4 positionCS : SV_POSITION;
    float3 normalWS :TEXCOORD0;
};

VertexOutput LitPassVertex (VertexInput i) {
    VertexOutput o = (VertexOutput)0;
    o.normalWS = TransformObjectToWorldNormal(i.normalOS);
    return o;
}

float4 LitPassFragment (VertexOutput input) : SV_TARGET {
    return 1;
}