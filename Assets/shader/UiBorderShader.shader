Shader "Unlit/UiBorderShader"

{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 size = float2(1.0,1.0) / _ScreenParams.xy;
                float edgeWidth = 1.0; // Adjust edge width
                float edgeIntensity = 1.0; // Adjust edge intensity

                float3x3 sobelX = float3x3(-1, 0, 1, -2, 0, 2, -1, 0, 1);
                float3x3 sobelY = float3x3(-1, -2, -1, 0, 0, 0, 1, 2, 1);

                float gx = 0;
                float gy = 0;

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        float4 sample = tex2D(_MainTex, i.uv + size * float2(x,y));
                        gx += sample.r * sobelX[x + 1][y + 1];
                        gy += sample.r * sobelY[x + 1][y + 1];
                    }
                }

                float g = sqrt(gx * gx + gy * gy);
                return fixed4(g, g, g, 1) * edgeIntensity;
            }
            ENDCG
        }
    }
}
