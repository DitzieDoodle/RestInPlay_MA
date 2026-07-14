void SpriteFlipUV_float(float2 UV, float2 Flip, float4 ST, out float2 Out)
{
    // ST.xy = scale (rect size), ST.zw = offset (rect min)
    float2 local = (UV - ST.zw) / ST.xy;

    local.x = (Flip.x > 0.5) ? local.x : (1.0 - local.x);
    local.y = (Flip.y > 0.5) ? local.y : (1.0 - local.y);

    Out = ST.zw + local * ST.xy;
}

void SpriteFlipUV_half(half2 UV, half2 Flip, half4 ST, out half2 Out)
{
    half2 local = (UV - ST.zw) / ST.xy;

    local.x = (Flip.x > 0.5h) ? local.x : (1.0h - local.x);
    local.y = (Flip.y > 0.5h) ? local.y : (1.0h - local.y);
    
    Out = ST.zw + local * ST.xy;
}