Shader "Custom/Reflection" 
{
	Properties
	{
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
		zwrite off

		GrabPass{}

		CGPROGRAM
		#pragma surface surf nolight noembient

		sampler2D _GrabTexture;

		struct Input 
		{
			float4 color:COLOR;
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float3 screenUV = IN.screenPos.rgb / IN.screenPos.a;
			o.Emission = tex2D(_GrabTexture, screenUV.xy);
		}

		float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(0, 0, 0, 1);
		}

		ENDCG
	}
	FallBack "Diffuse"
}
