Shader "CrossSection/Others/StencilledTexture"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_StencilMask("Stencil Mask", Range(0, 255)) = 255
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

		LOD 100
		
		Stencil{
			Ref [_StencilMask]
			Comp Equal
			Pass Zero
		}

		CGPROGRAM
		#pragma surface surf Lambert
		#include "UnityCG.cginc"


		sampler2D _MainTex;
		fixed4 _Color;
	      
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
	          
			
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb * _Color.rgb;

			o.Alpha = tex.a * _Color.a;
		}
		ENDCG
	}
	Fallback "Legacy Shaders/Transparent/VertexLit"
}
