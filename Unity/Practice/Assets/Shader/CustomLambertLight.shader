Shader "Custom/CustomLambertLight" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader 
		{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf Test noambient

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		float4 LightingTest(SurfaceOutput s, float3 lightDir, float atten)
		{
			//float ndotl = saturate(dot(s.Normal, lightDir));
			float ndotl = (dot(s.Normal, lightDir)) * 0.5 + 0.5;
			float4 final;
			final.rgb = ndotl * s.Albedo * _LightColor0.rgb * atten;
			final.a = s.Alpha;
			return final;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
