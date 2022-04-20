Shader "2D Shader Pack/Sprite/Switch" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color                     ("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[Header(Switch)]
		_TargetTex ("Target", 2D) = "white" {}
		_Lerp ("Lerp", Range(0, 1)) = 0
	}
	SubShader {
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull Off Lighting Off ZWrite Off
		Blend One OneMinusSrcAlpha
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			#include "Common.cginc"

			sampler2D _TargetTex;
			half _Lerp;

			half4 frag (v2f input) : SV_Target
			{
				half4 c = tex2D(_MainTex, input.uv) * input.col;
				c.rgb *= c.a;
				half4 c2 = tex2D(_TargetTex, input.uv) * input.col;
				c2.rgb *= c2.a;
				c = lerp(c, c2, _Lerp);
				return c;
			}
			ENDCG
		}
	}
	Fallback "Sprites/Default"
}