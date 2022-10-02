Shader "Unlit/Goal"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _TeamColor ("TeamColor", Color) = (0,0,0,1)
    }
    SubShader {
        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass {

            ZWrite Off
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _TeamColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float wave = abs(frac(i.uv.x * 6 + (_Time.y * 0.5)) * 2 - 1);
                wave *= (i.uv.x * 0.2);
                return _TeamColor * wave;
            }
            ENDCG
        }
    }
}
