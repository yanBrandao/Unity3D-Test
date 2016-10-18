Shader "Flat Vertex"{
	Properties{
	}
	SubShader{
		//Flat Vertex shader remove all texture and deth from
		// surface and put only color defined by meshFilter
		// in UpdateVerts.
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vertexInput{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct vertexOutput{
				float4 pos : SV_POSITION;
				float4 verCol : COLOR;
			};

			vertexOutput vert(vertexInput v){
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.verCol = v.color;
				return o;
			}

			float4 frag(vertexOutput i) : COLOR{
				return i.verCol;
			}
			ENDCG
		}
	}
}