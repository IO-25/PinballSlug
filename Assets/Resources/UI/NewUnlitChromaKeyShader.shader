Shader "UI/UI_ChromaKey"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _MaskColor ("Mask Color", Color) = (0,1,0,1) // 크로마키로 제거할 색상 (기본: 초록색)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _MaskColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                fixed4 finalColor = col * _Color;

                // 크로마키 로직: 픽셀 색상이 MaskColor와 비슷하면 투명하게 만듦
                float diff = distance(finalColor.rgb, _MaskColor.rgb);
                if (diff < 0.1) // 0.1은 민감도, 필요에 따라 조절 가능
                {
                    finalColor.a = 0;
                }

                return finalColor;
            }
            ENDCG
        }
    }
}