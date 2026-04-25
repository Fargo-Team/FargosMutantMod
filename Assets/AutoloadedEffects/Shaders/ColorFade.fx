float2 screenSize;
float2 screenPosition;
float rotation;
float2 anchorPoint;
float fadePercent;
float4 fadeColor;
float opacity;
float fadeSize;

sampler image0 : register(s0);

struct VSOutput
{
    float4 position : SV_POSITION;
    float4 color : COLOR0;
    float2 uv : TEXCOORD0;
};

float4 PixelShaderFunction(VSOutput input) : COLOR0
{
    float4 coords = input.position;
    float2 uv = input.uv;
    float4 sampleColor = input.color;
    
    float4 color = tex2D(image0, uv) * sampleColor;
    if (color.a < 0.2)
        return color;
    //aa
    float2 pos = coords.xy;

    float2 p = pos - anchorPoint;
    float2 dir = float2(-cos(rotation), sin(rotation));
    float signedDist = dot(p, dir);
    signedDist /= fadeSize;
    
    float t = saturate(signedDist + 0.5);
    float3 tinted = lerp(color.rgb, fadeColor.rgb, t);
    return float4(tinted, color.a);
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
