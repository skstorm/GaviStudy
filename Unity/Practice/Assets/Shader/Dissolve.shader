Shader "Custom/Dissolve" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NoiseTex("NoiseTex", 2D) = "white"{}
		_Cut("Alpha Cut", Range(0,1)) = 0
		[HDR]_OutColor("OutColor", Color) = (1,1,1,1)
		_OutThinkness("OutThinkness",Range(1,1.5)) = 1.15
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		float _Cut;
		float4 _OutColor;
		float _OutThinkness;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_NoiseTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float4 noise = tex2D(_NoiseTex, IN.uv_NoiseTex);
			o.Albedo = c.rgb;

			float noiseCorr = noise.r * 20;

			float alpha;
			if (noiseCorr >= _Cut)
			{
				alpha = 1;
			}
			else
			{
				alpha = 0;
			}

			float3 outline;
			if (noiseCorr >= _Cut*_OutThinkness)
			{
				outline = float3(0,0,0);
			}
			else
			{
				outline = _OutColor.rgb;
			}
			o.Emission = outline;

			o.Alpha = alpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
