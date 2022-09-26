Shader "2D Shader Pack/Sprite/LineFade" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color                     ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[Header(LineFade)][Space(5)]
		_GlowColor    ("Glow Color", Color) = (0, 1, 1, 1)
		_GlowStrength ("Glow Strength", Range(0, 2)) = 1
		_Direction    ("Direction", Vector) = (1, 0, 0, 0)
		_Progress     ("Progress", Range(-0.3, 1.2)) = 0
	}
	SubShader {
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull Off Lighting Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_local _ PIXELSNAP_ON
			#include "UnityCG.cginc"

			half4 _Color, _Direction, _GlowColor;
			sampler2D _MainTex;
			half _Progress, _GlowStrength;

			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed4 col : COLOR;
				float2 uv  : TEXCOORD0;
				float3 wldpos : TEXCOORD1;
			};
			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.col = v.color * _Color;
				o.wldpos = mul(unity_ObjectToWorld, v.vertex.xyz);
#ifdef PIXELSNAP_ON
				o.pos = UnityPixelSnap(o.pos);
#endif
				return o;
			}
			half4 frag (v2f input) : SV_Target
			{
				half f = (dot(input.wldpos.xy, normalize(_Direction.xy)) + 1.0) / 2.0;
				half threshold = f - _Progress;
				clip(threshold);

				half t = smoothstep(0.2, 0.0, threshold);

				half4 c = tex2D(_MainTex, input.uv) * input.col;
				c.rgb *= c.a;
				return lerp(c, _GlowStrength * _GlowColor * c.a, t);
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}