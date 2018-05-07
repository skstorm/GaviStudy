Shader "Custom/FireShader" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2("Albedo (RGB)", 2D) = "white" {}
		_XScrollWeight("_XScrollWeight", Float) = 0
		_YScrollWeight("_YScrollWeight", Float) = 1
	}
	SubShader 
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard alpha:blend
		
		sampler2D _MainTex;
		sampler2D _MainTex2;
		float _XScrollWeight;
		float _YScrollWeight;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 d = tex2D(_MainTex2, float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y - _Time.y));
			float2 uvMain = float2(0,0);
			uvMain.x = IN.uv_MainTex.x + d.r*_XScrollWeight;
			uvMain.y = IN.uv_MainTex.y + d.r*_YScrollWeight;
			fixed4 c = tex2D(_MainTex, uvMain);

			o.Emission = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
