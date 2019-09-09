Shader "Clipping/Box/SurfaceShader/Standard" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

	//	_SectionCentre("SectionCentre",Vector) = (0,0,0,0)
	//	_SectionDirX("SectionNormal",Vector) = (1,0,0,0)
	//	_SectionDirY("SectionNormal",Vector) = (0,1,0,0)
	//	_SectionDirZ("SectionNormal",Vector) = (0,0,1,0)
	//	_SectionScale("SectionNormal",Vector) = (1,1,1,0)  

	_SectionColor ("Section Color", Color) = (0,0,0,1)
	_StencilMask("Stencil Mask", Range(0, 255)) = 255
	[Toggle(RETRACT_BACKFACES)] _retractBackfaces("retractBackfaces", Float) = 0 
	
	}
	SubShader {
		Tags {"Queue" = "Transparent-1" "RenderType"="Transparent" }
		LOD 200

		Stencil
		{
			Ref [_StencilMask]
			CompBack Always
			PassBack Replace

			CompFront Always
			PassFront Zero
		}

		
	Cull Off
		
	CGPROGRAM
	// Physically based Standard lighting model, and enable shadows on all light types
	#pragma surface surf Standard fullforwardshadows vertex:vert
	#pragma multi_compile __ CLIP_BOX CLIP_CORNER
	#include "CGIncludes/section_clipping_CS.cginc"
	#pragma shader_feature RETRACT_BACKFACES
	// Use shader model 3.0 target, to get nicer looking lighting
	#pragma target 3.0

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
		float3 worldPos;
		float myface : VFACE;
	};

	half _BackfaceExtrusion;

	void vert (inout appdata_full v) {
		#if RETRACT_BACKFACES
		float3 viewDir = ObjSpaceViewDir(v.vertex);
		float dotProduct = dot(v.normal, viewDir);
		if(dotProduct<0) {
			float3 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
			float3 worldNorm = UnityObjectToWorldNormal(v.normal);
			worldPos -= worldNorm * _BackfaceExtrusion;
			v.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;
		}
		#endif
	}

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	fixed4 _SectionColor;

	void surf(Input IN, inout SurfaceOutputStandard o) {
		PLANE_CLIP(IN.worldPos);
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		// Metallic and smoothness come from slider variables
		if(IN.myface>0) 
		{
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		else
		{
			o.Emission = _SectionColor.rgb;
		}
	}
	ENDCG


	Cull Off
	ColorMask 0
	CGPROGRAM
	#pragma surface surf NoLighting  noambient vertex:vert
	#pragma multi_compile __ CLIP_BOX
	#pragma shader_feature RETRACT_BACKFACES
	#include "CGIncludes/section_clipping_CS.cginc"

	struct Input {
		float3 worldPos;
	};

	half _BackfaceExtrusion;

	void vert (inout appdata_full v) {
		#if RETRACT_BACKFACES
		//float3 viewDir = ObjSpaceViewDir(v.vertex);
		//float dotProduct = dot(v.normal, viewDir);
		//if(dotProduct<0) {
			float3 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
			float3 worldNorm = UnityObjectToWorldNormal(v.normal);
			worldPos -= worldNorm * _BackfaceExtrusion;
			v.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;
		//}
		#endif
	}

	fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
	{
		fixed4 c;
		c.rgb = s.Albedo;
		c.a = s.Alpha;
		return c;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		PLANE_CLIPWITHCAPS(IN.worldPos);
		o.Albedo = float3(1,1,1);
		o.Alpha = 1;
	}
	ENDCG

	}
		FallBack "Diffuse"
	
}
