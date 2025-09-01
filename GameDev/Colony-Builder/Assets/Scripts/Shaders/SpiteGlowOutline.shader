Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0, 10)) = 1
        [Toggle] _EnableOutline ("Enable Outline", Float) = 1
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
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            #define MAX_OUTLINE_THICKNESS 5

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineThickness;
            float _EnableOutline;

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
            sampler2D _AlphaTex;
            float4 _MainTex_TexelSize;

            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                float currentAlpha = c.a;

                if (currentAlpha > 0.01)
                {
                    c.rgb *= c.a;
                    return c;
                }

                if (_EnableOutline < 0.5)
                {
                    return c;
                }

                // Check neighbors for outline.
                float maxAlpha = 0.0;
                float radiusStepX = _OutlineThickness * fwidth(IN.texcoord.x) / MAX_OUTLINE_THICKNESS;
                float radiusStepY = _OutlineThickness * fwidth(IN.texcoord.y) / MAX_OUTLINE_THICKNESS;

                for (int x = -MAX_OUTLINE_THICKNESS; x <= MAX_OUTLINE_THICKNESS; x++)
                {
                    for (int y = -MAX_OUTLINE_THICKNESS; y <= MAX_OUTLINE_THICKNESS; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        // Optional: approximate circular kernel to avoid boxy corners
                        float normDist = sqrt((float(x) * float(x) + float(y) * float(y)) / (MAX_OUTLINE_THICKNESS * MAX_OUTLINE_THICKNESS));
                        if (normDist > 1.0) continue;

                        float2 offset = float2(x * radiusStepX, y * radiusStepY);
                        fixed4 neighbor = SampleSpriteTexture(IN.texcoord + offset);
                        maxAlpha = max(maxAlpha, neighbor.a);
                    }
                }

                if (maxAlpha > 0.01)
                {
                    return _OutlineColor;
                }

                return c;
            }
            ENDCG
        }
    }
}