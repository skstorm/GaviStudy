Shader "Custom/NewSurfaceShader" {
	Properties {
		
		_red ("Red", Range(0,1)) = 0.0
		_green ("Green", Range(0,1)) = 0.0
		_blue ("Blue", Range(0,1)) = 0.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows


		struct Input 
		{
			float4 color : COLOR;
		};

		

		float _red;
		float _green;
		float _blue;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = float3(_red, _green, _blue);
			//o.Emission = 1;

			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
