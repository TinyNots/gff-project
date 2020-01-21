Shader "Custom/Distortion"
{
    Properties
    {
        _MainTex ("Distortion Tex", 2D) = "white" {}
		_Opacity("Opacity", Range(0, 1)) = 1

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
		Tags{
		"RenderType" = "Opaque"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

            #include "UnityCG.cginc"


			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 projPos : TEXCOORD1;
			};

			VertexOutput vert(VertexInput v) {
				VertexOutput o;
				o.uv = v.uv;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);
				return o;
			}

		   struct Input {
			float2 uv_MainTex:TEXCOORD0;
		};

            sampler2D _MainTex;
			float _Opacity;

			float4 frag(Input IN, VertexOutput i): COLOR{
				float2 sceneUVs = (i.projPos.xy / i.projPos.w);

				float3 c = tex2D(_MainTex, IN.uv_MainTex).rgb;
				return fixed4(c,_Opacity);

			}
            ENDCG
        }
    }
	FallBack "Diffuse"

}
