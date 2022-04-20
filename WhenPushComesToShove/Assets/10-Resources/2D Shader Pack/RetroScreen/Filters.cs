using UnityEngine;

namespace ShaderPack2D
{
	//[ExecuteInEditMode]
	public class Filters : MonoBehaviour
	{
		public bool m_Enable = true;
		public Material m_Mat;
		[Header("Color Quantize")]
		public bool m_EnableQuantize = false;
		[Range(1, 8)] public int m_RBits = 8;
		[Range(1, 8)] public int m_GBits = 8;
		[Range(1, 8)] public int m_BBits = 8;
		[Header("Tv Curvature")]
		public bool m_EnableTvCurvature = false;
		[Range(0f, 1f)] public float m_TvCurvature = 0f;
		[Header("Pixel Mask")]
		public bool m_EnablePixelMask = false;
		public Texture2D m_PixelMask;
		public Vector4 m_PixelMaskParams = new Vector4 (1f, 1f, 1.3f, 0f);
		[Header("Rolling Flicker")]
		public bool m_EnableRollingFlicker = false;
		[Range(0f, 1f)] public float m_RollingFlickerAmount = 0.1f;
		private float m_RollingFlickerOffset = 0f;
		public float m_RollingFlickerVsynctime = 1f;
		[Header("Gameboy")]
		public bool m_EnableGameboy = false;
		[Header("Pixelate")]
		public bool m_EnablePixelate = false;
		[Range(0.002f, 0.03f)] public float m_PixelateSize = 0.01f;
		[Header("Vignette")]
		public float m_VignetteRadius = 0.75f;
		[Range(0f, 2f)] public float m_VignetteSoftness = 0.45f;
		[Range(0f, 1f)] public float m_VignetteIntensity = 1f;

//		void OnEnable()  {}
//		void OnDisable() {}
//		void Start()     {}
		void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			if (m_Enable)
			{
				ToggleKeyword(m_EnableQuantize, "USE_Quantize");
				ToggleKeyword(m_EnableTvCurvature, "USE_TvCurvature");
				ToggleKeyword(m_EnablePixelMask, "USE_PixelMask");
				ToggleKeyword(m_EnableRollingFlicker, "USE_RollingFlicker");
				ToggleKeyword(m_EnableGameboy, "USE_Gameboy");
				ToggleKeyword(m_EnablePixelate, "USE_Pixelate");

				Vector4 quantize = new Vector4(Mathf.Pow(2f, m_RBits), Mathf.Pow(2f, m_GBits), Mathf.Pow(2f, m_BBits), 1f);
				m_Mat.SetVector("_QuantizeRGB", quantize);

				float oneOverBaseSize = 80.0f / 512.0f;
				float ar = (src.width * 1.0f) / (src.height * 1.0f);
				m_Mat.SetFloat("_TvCurvature", m_TvCurvature * ar * oneOverBaseSize);

				m_Mat.SetTexture("_PixelMaskTex", m_PixelMask);
				m_Mat.SetVector("_PixelMaskParams", m_PixelMaskParams);

				m_RollingFlickerOffset += m_RollingFlickerVsynctime;
				m_Mat.SetVector("_RollingFlickerParams", new Vector4 (m_RollingFlickerOffset, m_RollingFlickerOffset + m_RollingFlickerVsynctime, m_RollingFlickerAmount, 0f));
				m_Mat.SetFloat("_PixelateSize", m_PixelateSize);
				m_Mat.SetFloat("_VignetteRadius", m_VignetteRadius);
				m_Mat.SetFloat("_VignetteSoftness", m_VignetteSoftness);
				m_Mat.SetFloat("_VignetteIntensity", m_VignetteIntensity);
				Graphics.Blit(src, dst, m_Mat);
			}
			else
			{
				Graphics.Blit(src, dst);
			}
		}
		void ToggleKeyword(bool toggle, string keyword)
		{
			if (toggle)
				m_Mat.EnableKeyword(keyword);
			else
				m_Mat.DisableKeyword(keyword);
		}
	}
}