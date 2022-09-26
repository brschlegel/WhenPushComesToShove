Shader "2D Shader Pack/Sprite/Buggy" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color                     ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[Header(Buggy)][Space(5)]
		_Power      ("Power", Range(1, 6)) = 3
		_Size       ("Size", Vector) = (10, 10, 10, 10)
		_Distortion ("Distortion", Range(0, 1)) = 0
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
			#include "Common.cginc"

			float _Power, _Distortion;
			float4 _Size;

			float random2 (float2 seed)
			{
				return frac(sin(dot(seed * floor(50 + (_Time.y + 0.1) * 12.0), float2(127.1, 311.7))) * 43758.5453123);
			}
			float random (float seed)  { return random2(float2(seed, 1.0)); }
			half4 frag (v2f input) : SV_Target
			{
				float2 s = floor(input.uv * _Size.xy);
				float2 l = floor(input.uv * _Size.zw);
				float nis = pow(random2(s), _Power) *_Distortion * pow(random2(l), _Power);
				float4 c = tex2D(_MainTex, input.uv + float2(nis * 0.2 * random(2.0), 0)) * input.col;
				c.rgb *= c.a;
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}