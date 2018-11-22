Shader "Custom/PolygonStyle" {
	Properties {
		_MainColor("Main Color",Color)=(1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		Pass{
			ZWrite off
			AlphaTest Greater 0
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			Offset -1,-1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			
			fixed4 _MainColor;
			sampler2D _MainTex;
			float4x4 unity_Projector;

			struct vInput{
				float4 vertex:POSITION;
				fixed2 texcoord:TEXCOORD0;
			};

			struct vOutput{
				float4 uvMain:TEXCOORD0;
				UNITY_FOG_COORDS(2)
				float4 pos:SV_POSITION;
			};

			vOutput vert(vInput v){
				vOutput o;
				o.pos=UnityObjectToClipPos(v.vertex);
				o.uvMain=mul(unity_Projector,v.vertex);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(vOutput i):SV_Target{
				fixed4 main=tex2Dproj(_MainTex,UNITY_PROJ_COORD(i.uvMain));

				main*=_MainColor;

			//	fixed mainBlit=max(0,sign(i.uvMain-))
				
			//	fixed4 res=fixed4(0,0,0,0);
			//	res+=main*fixed4(mai)


				return main;

			}



			ENDCG
		}
		
	}
}

