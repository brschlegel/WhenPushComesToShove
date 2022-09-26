Shader "2D Shader Pack/Sprite/Pinch" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color                     ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[Header(Pinch)][Space(5)]
		_Size ("Size", Range(0, 0.5)) = 0.25
	}
	SubShader {
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull Off Lighting Off ZWrite Off Blend One OneMinusSrcAlpha
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_local _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			#include "Common.cginc"

			half _Size;

			float2 pinchUv (float2 uv, float size)
			{
				float2 m = float2(0.5, 0.5);
				float2 d = uv - m;
				float r = sqrt(dot(d, d));
				float power = (2.0 * 3.141592 / (2.0 * sqrt(dot(m, m)))) * (-size+0.001);
				float bind = 0.5;
				uv = m + normalize(d) * atan(r * -power * 10.0) * bind / atan(-power * bind * 10.0);
				return uv;
			}
			half4 frag (v2f input) : SV_Target
			{
				float2 uv = pinchUv(input.uv, _Size);
				half4 c = tex2D(_MainTex, uv) * input.col;
				c.rgb *= c.a;
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}