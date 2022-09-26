Shader "2D Shader Pack/Sprite/MotionBlurDirectional" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color                     ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[Header(MotionBlurDirectional)][Space(5)]
		_Dir ("Directional", Vector) = (1, 0, 0, 0)
		_Iterations ("Iterations", Int) = 16
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
			#pragma vertex vertMotionBlur
			#pragma fragment frag
			#pragma multi_compile_local _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			#include "Common.cginc"

			float2 _Dir;
			int _Iterations;

			v2f vertMotionBlur (appdata_full v)
			{
				float2 sz = abs(_Dir) * 2.0;
				float4 pos = v.vertex;
				pos.xy *= sz + 1.0;

				float2 uv = v.texcoord;
				uv = (uv - 0.5) * (sz + 1.0) + 0.5;

				v2f o;
				o.pos = UnityObjectToClipPos(pos);
				o.uv = uv;
				o.col = v.color * _Color;
#ifdef PIXELSNAP_ON
				o.pos = UnityPixelSnap(o.pos);
#endif
				return o;
			}
			float insideUnitSquare (float2 v)
			{
				float2 s = step(0.0, v) - step(1.0, v);
				return s.x * s.y;
			}
			float4 frag (v2f input) : SV_Target
			{
				float2 uv = input.uv;
				float inSquare = insideUnitSquare(uv);
				half4 c = SampleMainTex(uv, input.col) * inSquare;

				float2 s = _Dir / float(_Iterations);
				for (int i = 1; i <= _Iterations; i++)
				{
					float2 uv2 = uv + s * float(i);
					float inSquare = insideUnitSquare(uv2);
					c += (SampleMainTex(uv2, input.col) * inSquare);

					uv2 = uv - s * float(i);
					inSquare = insideUnitSquare(uv2);
					c += (SampleMainTex(uv2, input.col) * inSquare);
				}
				c /= (_Iterations * 2.0 + 1.0);
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}