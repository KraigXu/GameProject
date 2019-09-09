  Shader "CrossSection/VertexColor" {
    Properties {
      _Color ("Main Color", Color) = (1,1,1,1)
	  _MainTex ("Texture", 2D) = "white" {}
	  
      _SectionColor ("Section Color", Color) = (1,0,0,1)
    }
    
    
    SubShader {
    	Tags { "RenderType"="Opaque" }
		LOD 200

        Cull front // cull only front faces
         
		CGPROGRAM
		#pragma surface surf Lambert nodynlightmap 
		#pragma multi_compile __ CLIP_PLANE CLIP_TWO_PLANES
		#include "UnityCG.cginc"
		#include "CGIncludes/section_clipping_CS.cginc"

		fixed4 _SectionColor;

		struct Input {
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			PLANE_CLIP(IN.worldPos);
			fixed4 c = _SectionColor;
			o.Emission =  c.rgb;
		}
		ENDCG

		//------------------------
	      
	      
	      Cull Back
	      
	      CGPROGRAM
	      #pragma surface surf BlinnPhong
	      #pragma debug
	      fixed4 _Color;
		  
		 #pragma multi_compile __ CLIP_PLANE CLIP_TWO_PLANES
		 
		 #include "CGIncludes/section_clipping_CS.cginc"

		  sampler2D _MainTex;
	      
	      struct Input {
	          float2 uv_MainTex;
	          float3 worldPos;
			  float4 color: Color;
	      };

	      void surf (Input IN, inout SurfaceOutput o) {
			  PLANE_CLIP(IN.worldPos);
	          fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	          o.Albedo = tex.rgb * IN.color.rgb * _Color.rgb;
	          o.Alpha = tex.a * _Color.a;
	      }
	      ENDCG

    } 

    Fallback "Diffuse"
}