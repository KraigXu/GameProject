// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutLine1" {
	Properties{
		_MainTex("Main Tex",2D)="black"{}
		_RimColor("Rim color",Color)=(1,1,1,1)  //边缘颜色
		_RimPower("Rim Power",Range(1,10))=2    //边缘强度
	}

	SubShader{
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f{
				float4 vertex:POSITION;
				float4 uv:TEXCOORD0;
				float4 NdotV:COLOR;
			};

			sampler2D _MainTex;
			float4 _RimColor;
			float _RimPower;

			v2f vert(appdata_base v){
				v2f o;
				o.vertex=UnityObjectToClipPos(v.vertex);
				o.uv=v.texcoord;
				float3 V=WorldSpaceViewDir(v.vertex);
				V=mul(unity_WorldToObject,V);
				o.NdotV.x=saturate(dot(v.normal,normalize(V)));
				
				return o;
			}

			half4 frag(v2f IN):COLOR{
				half4 c=tex2D(_MainTex,IN.uv);
				c.rgb+=pow((1-IN.NdotV.x),_RimPower)*_RimColor.rgb;
				return c;
			}
			ENDCG

		}
	}
	FallBack "Diffuse"
}
