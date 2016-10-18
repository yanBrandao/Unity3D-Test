Shader "Custom/VertexColors"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Shininess ("Shininess", Range(0.03, 1)) = 0.078125
	}
	SubShader
	{
		//	Using BlinnPhong shading model this shader will put
		// color in pixels setted by MeshFilter in UpdateVerts
		// Script.
		Tags { "RenderType"="Opaque" }
		CGPROGRAM
			#pragma surface surf BlinnPhong vertex:vert
			
			half _Shininess;

			struct Input{
				half2 uv_MainTex;
				fixed3 vertColors;
			};

			sampler2D _MainTex;

			void vert (inout appdata_full v, out Input o) {
				o.vertColors = v.color.rgb;
				o.uv_MainTex = v.texcoord;
			}

			fixed4 surf(Input IN, inout SurfaceOutput o) {
				fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = IN.vertColors.rgb;
				o.Specular = 1;
				o.Gloss = _Shininess;
				return col;
			}
			ENDCG
	}
	Fallback "Specular"
}
