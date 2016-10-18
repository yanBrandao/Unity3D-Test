Shader "Custom/AnotherGroundWave" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf Lambert vertex:vert


		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		//  Vertex function will offset pixel position
		// according to sin function and time variation
		// creating a wave in the object. 
		void vert(inout appdata_full v) {
			half offsetvert = sin(v.vertex.x + _Time[1]);

			v.vertex.y += offsetvert;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
