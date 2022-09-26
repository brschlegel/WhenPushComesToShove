#ifndef COMMON_INCLUDED
#define COMMON_INCLUDED

half4 _Color;
sampler2D _MainTex;
half4 _MainTex_TexelSize;

struct v2f
{
	float4 pos : SV_POSITION;
	half4 col  : COLOR;
	float2 uv  : TEXCOORD0;
};
v2f vert (appdata_full v)
{
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv = v.texcoord;
	o.col = v.color * _Color;
#ifdef PIXELSNAP_ON
	o.pos = UnityPixelSnap(o.pos);
#endif
	return o;
}

half2 DisplacementUV (half2 uv, half x, half y, half v)
{
	return lerp(uv, uv + half2(x, y), v);
}

half4 SampleMainTex (half2 uv, half4 color)
{
	float4 c = tex2D(_MainTex, uv) * color;
	c.rgb *= c.a;
	return c;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////
// rebuild the normalized texcoord

float4 _UvRect;

float2 CalcNormalizedUv (float2 uv)
{
	float width = _UvRect.z - _UvRect.x;
	float height = _UvRect.w - _UvRect.y;
	return float2((uv.x - _UvRect.x) / width, (uv.y - _UvRect.y) / height);
}

#endif