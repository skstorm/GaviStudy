﻿Shader "Custom/Fire" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo (RGB)", 2D) = "black" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent"  "Queue"="Transparent"}
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf Standard alpha:fade

		
		sampler2D _MainTex;
		sampler2D _MainTex2;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 d = tex2D (_MainTex2,  float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y - _Time.y) );
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + d.r);
			o.Emission = c.rgb;
			//float aaa = _Time.y - round(_Time.y);
			//o.Emission = aaa;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
