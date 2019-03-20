Shader "Custom/OutLineTest" {
	Properties {
		_MainTex("Main Tex",2D)="black"{}
		_RimColor("Rim color",Color)=(1,1,1,1)
		_RimPower("Rim Power",Range(1,10))=2
	}
	SubShader {
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

		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}

	    SubShader
    {
        //描边
        pass
        {
            Cull Front
            Offset -5,-1 //深度偏移
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            half4 _OutLineColor;
 
            struct v2f
            {
                float4  pos : SV_POSITION;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            float4 frag(v2f i) : COLOR
            {
                return _OutLineColor;
            }
            ENDCG
        }
 
        //正常渲染物体
        pass
        {
            //Cull Back
            //Offset 5,-1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
 
            struct v2f
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
 
            float4 frag(v2f i) : COLOR
            {
                float4 c = tex2D(_MainTex,i.uv);
                return c;
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
