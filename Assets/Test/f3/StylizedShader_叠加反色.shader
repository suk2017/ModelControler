﻿Shader "Unlit/ChinesePainting0"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_EdgeRatio("EdgeRatio", Range(-1.0, 1.0)) = 1.0
		_Threshold("Threshold", Range(0.0, 1.0)) = 0.4
		_Gradation("Gradation", Range(0.0, 20.0)) = 10.0
		_RampTex("RampTex", 2D) = "white" {}
		_Strength("Strength", Float) = 1.0
	}

		SubShader
	{
		Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 100

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog


			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal:NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float edge : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _RampTex;
			float4 _Color;
			float4 _MainTex_ST;
			float _EdgeRatio;
			float _Threshold;
			float _Gradation;
			float _Strength;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);


				o.edge = dot(v.normal, ObjSpaceViewDir(v.vertex));

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//纹理采样
				fixed4 col = tex2D(_MainTex, i.uv);
				//降低色阶
				col = floor(col * _Gradation) / _Gradation;
				//风格化
				col *= _Strength * tex2D(_RampTex, i.uv);


				//提取边界
				float4 edge = i.edge * _EdgeRatio;
				edge = edge > _Threshold ? 1 : edge * edge;//0.13 0.74
				//edge = edge < _Threshold ? edge / 4 : 1;//线性 适合卡通 不适合水墨



				col = col * edge;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
