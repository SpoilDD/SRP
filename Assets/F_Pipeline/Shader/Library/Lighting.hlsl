VertexOutput LitPassVertex (VertexInput i) {
    VertexOutput o = (VertexOutput)0;
    o.normalWS = TransformObjectToWorldNormal(i.normalOS);
    return o;
}

float4 LitPassFragment (VertexOutput input) : SV_TARGET {
    return 1;
}