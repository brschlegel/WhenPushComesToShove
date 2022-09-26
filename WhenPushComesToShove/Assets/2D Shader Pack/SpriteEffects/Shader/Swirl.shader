Shader "2D Shader Pack/Sprite/Swirl" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color                     ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[Header(Swirl)][Space(5)]
		_Ratio    ("Ratio", Range(0.3, 1)) = 0.1
		_Power    ("Power", Float) = 3
		_MinSpeed ("Min Speed", Float) = 10
		_MaxSpeed ("Max Speed", Float) = 90
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

			float _Ratio, _Power, _MinSpeed, _MaxSpeed;

			float4 frag (v2f input) : SV_Target
			{
				float2 uv = input.uv * 2.0 - 1.0;
				float len = length(uv);
				float rspeed = lerp(_MaxSpeed, _MinSpeed, len);

				float sinx = sin((1.0 - _Ratio) * rspeed);
				float cosx = cos((1.0 - _Ratio) * rspeed);
				float2 trs = mul(uv, float2x2(cosx, sinx, -sinx, cosx));
				trs /= pow(_Ratio, _Power);

				trs += 1.0;
				trs /= 2.0;

				float4 c = 0.0;
				if (trs.x > 1.0 || trs.x < 0.0 || trs.y > 1.0 || trs.y < 0.0)
					c = 0.0;
				else
					c = SampleMainTex(trs, input.col);
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}