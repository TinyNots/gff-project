Shader "Sprites/Glitch"
{
	Properties
	{
		// 弄る安いために、変数をＧＵＩ化する
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_GlitchInterval ("Glitch interval time [seconds]", Float) = 0.16
		_DispProbability ("Displacement Glitch Probability", Float) = 0.022
		_DispIntensity ("Displacement Glitch Intensity", Float) = 0.09
		_ColorProbability("Color Glitch Probability", Float) = 0.02
		_ColorIntensity("Color Glitch Intensity", Float) = 0.07
		[MaterialToggle] _WrapDispCoords ("Wrap disp glitch (off = clamp)", Float) = 1
		[MaterialToggle] _DispGlitchOn ("Displacement Glitch On", Float) = 1
		[MaterialToggle] _ColorGlitchOn ("Color Glitch On", Float) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma exclude_renderers xbox360
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			// スｐライトじょうの頂点座標の情報を参照する
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			// 描画するための変数を構造体として定義する
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			
			// 参照した情報を自分が欲しい効果にする
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif
				return OUT;
			}

			sampler2D _MainTex;
			
			// 特別乱数
			float rand(float x, float y)
			{
				return frac(sin(x*12.9898 + y*78.233)*43758.5453);
			}
			
			float _GlitchInterval;
			float _DispIntensity;
			float _DispProbability;
			float _ColorIntensity;
			float _ColorProbability;
			float _DispGlitchOn;
			float _ColorGlitchOn;
			float _WrapDispCoords;
			fixed4 frag(v2f IN) : SV_Target
			{
				// これは毎間隔しか更新しない時間間隔です。例えば、毎フレームが違う値としても、時間間隔を同じままになる
				float intervalTime = floor(_Time.y / _GlitchInterval) * _GlitchInterval;

				// もっとランダムを出すために、２個目の時間間隔を作る
				float intervalTime2 = intervalTime + 2.793;

				// グリッチ効果を同時に発生しないために、時間間隔とスプライトの移動を足して自分専用のランダムを生成する
				float3 offset = UnityObjectToViewPos(float3(0.0,0.0,0.0));
				float timePositionVal = intervalTime + offset.x + offset.y;
				float timePositionVal2 = intervalTime2 + offset.x + offset.y;

				// 変位と変色のランダム値を生成する
				float dispGlitchRandom = rand(timePositionVal, -timePositionVal);
				float colorGlitchRandom = rand(timePositionVal, timePositionVal);

				// ＵＶマップをＲＧＢごとにずらすために生成する
				float rShiftRandom = (rand(-timePositionVal, timePositionVal) - 0.5) * _ColorIntensity;
				float gShiftRandom = (rand(-timePositionVal, -timePositionVal) - 0.5) * _ColorIntensity;
				float bShiftRandom = (rand(-timePositionVal2, -timePositionVal2) - 0.5) * _ColorIntensity;

				// 変位の切り戦を真ん中に置くだけじゃなくて、少しずらす
				float shiftLineOffset = float((rand(timePositionVal2, timePositionVal2) - 0.5) / 50);

				if(dispGlitchRandom < _DispProbability && _DispGlitchOn == 1)
				{
					IN.texcoord.x += (rand(floor(IN.texcoord.y / (0.2 + shiftLineOffset)) - timePositionVal, floor(IN.texcoord.y / (0.2 + shiftLineOffset)) + timePositionVal) - 0.5) * _DispIntensity;
					if(_WrapDispCoords == 1)
					{
						IN.texcoord.x = fmod(IN.texcoord.x, 1);
					}
					else
					{
						IN.texcoord.x = clamp(IN.texcoord.x, 0, 1);
					}
				}

				// テクスチャを元のスプライトとして扱う
				fixed4 normalC = tex2D(_MainTex, IN.texcoord);
				
				// ＲＧＢごとにテクスチャをずらす
				fixed4 rShifted = tex2D(_MainTex, float2(IN.texcoord.x + rShiftRandom, IN.texcoord.y + rShiftRandom));
				fixed4 gShifted = tex2D(_MainTex, float2(IN.texcoord.x + gShiftRandom, IN.texcoord.y + gShiftRandom));
				fixed4 bShifted = tex2D(_MainTex, float2(IN.texcoord.x + bShiftRandom, IN.texcoord.y + bShiftRandom));
				
				fixed4 c = fixed4(0.0,0.0,0.0,0.0);
				if(colorGlitchRandom < _ColorProbability && _ColorGlitchOn== 1)
				{
					// 描画するためのＲＧＢを前にずらしたテクスチャのＲＧＢをそれぞれ入れる
					c.r = rShifted.r;
					c.g = gShifted.g;
					c.b = bShifted.b;
					c.a = (rShifted.a + gShifted.a + bShifted.a) / 3;
				}
				else
				{
					c = normalC;
				}
				
				// 色を元色と重ねて、少し透明する
				c.rgb *= IN.color;
				c.a *= IN.color.a;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}