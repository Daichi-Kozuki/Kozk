// =============================================
// カラーを変更するシェーダー
// ------------------------
// ルール画像を元にカラーを変更していく
// =============================================

Shader "Custom/Super_Material"{
	Properties{
	[HDR]	_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
			
		// 色の付き方に関わるディゾルブ用ルールテクスチャ
		_ColorTex("ルール画像", 2D) = "white" {}
		_ColorTex_2("ルール画像", 2D) = "white" {}

		// 光カラー 最大値：１
	[HDR]	_Specified("光らせる色",Color) = (1,0,0,1)

		// 変更値 最大値：１
		_Threshold("Range", Range(0,1.1)) = 0.0
		_Dir("向き", Int) = 0.0

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _ColorTex;
			sampler2D _ColorTex_2;

			struct Input {
				float2 uv_MainTex;
			};

			float4 _EmissionColor;
			float4 _Specified;
			half _Threshold;
			int _Dir;
			fixed4 _Color;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {

				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				float2 rotatedUV;
				fixed4 m;
				switch (_Dir)
				{
				case 0: { 
					rotatedUV = float2(1.0 - IN.uv_MainTex.y, IN.uv_MainTex.x);
					m = tex2D(_ColorTex, rotatedUV);
					break; }

				case 1: {
					rotatedUV = float2(IN.uv_MainTex.y, IN.uv_MainTex.x);
					m = tex2D(_ColorTex, rotatedUV);
					break; }

				case 2: {
					rotatedUV = float2(IN.uv_MainTex.y,IN.uv_MainTex.x);
					m = tex2D(_ColorTex_2, rotatedUV);
					break; }

				case 3: {
					rotatedUV = float2(IN.uv_MainTex.y, 1.0 - IN.uv_MainTex.x);
					m = tex2D(_ColorTex_2, rotatedUV);
					break; }
				}

				// しきい値を越えた場合,色を変更する				
				half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
				if (g < _Threshold) {
					_EmissionColor = _Specified;
				}

				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Emission = _EmissionColor;
			}
			ENDCG
		}
		FallBack "Diffuse"
}