Shader "UI/SoftShadow"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _BlurSize ("Blur Size", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _BlurSize;

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 blur = float2(_BlurSize/1000.0, _BlurSize/1000.0);
                float4 col =
                    tex2D(_MainTex, i.uv + blur) +
                    tex2D(_MainTex, i.uv - blur) +
                    tex2D(_MainTex, i.uv + float2(blur.x, -blur.y)) +
                    tex2D(_MainTex, i.uv + float2(-blur.x, blur.y));

                col /= 4.0;      // 平均化してなめらかに
                return col * _Color;
            }
            ENDCG
        }
    }
}
