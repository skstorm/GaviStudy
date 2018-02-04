Shader "Custom/CustomHolo" 
{
	Properties 
	{
		_BumpMap ("NormalMap", 2D) = "white" {}
	}
	SubShader 
		{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf Lambert noambient alpha:fade

		sampler2D _BumpMap;

		struct Input 
		{
			float2 uv_BumpMap;
			float3 viewDir;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			float rim = saturate(dot(o.Normal, IN.viewDir));
			rim = pow(1 - rim, 3);

			int flag = 0;
			if(flag == 0)
			{
				
				rim = rim * (sin(_Time.y)*0.5 + 0.5);
				//rim = rim * abs(sin(_Time.y));
			}
			else if(flag == 1)
			{
				rim = saturate(pow(1 - rim, 3) + pow(frac(IN.worldPos.g * 3 - _Time.y), 30));
			}
			else
			{
				rim = 1;
			}

			o.Emission = float3(0, 1, 0);
			o.Alpha = rim;
		}

		float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(0,0,0,s.Alpha);
		}

		ENDCG
	}
	FallBack "Transparent/Diffuse"
}
