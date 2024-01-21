Shader "Hidden/RetroFX Pro Filter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            float4 _MainTex_TexelSize;

            fixed _ColoringMode;
            bool _LuminosityAdjustment;

            float _Fade;
            float3 _BitDepth;
            float _ResolutionScale;
            uint _PaletteLength;
            fixed4 _Palette[256];
            float _DitherStrength;

            float _BayerMatrix[64];
            int _BayerLength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float getLuminosity(float4 c)
            {
                return sqrt(0.299 * c.r * c.r + 0.587 * c.g * c.g + 0.114 * c.b * c.b);
            }

            float getScore(fixed4 c0, fixed4 c1)
            {
                fixed r = c0.r - c1.r;
                fixed g = c0.g - c1.g;
                fixed b = c0.b - c1.b;
                return r*r + g*g + b*b;
            }

            float floorToNearest(float x, float n)
            {
                return floor(x / n) * n;
            }

            float roundToNearest(float x, float n)
            {
                return round(x / n) * n;
            }

            int mod(float x, float y)
            {
                return frac(x / y) * y;
            }

            float dither(uint x, uint y)
            {
                y = mod(y, _BayerLength);
                x = mod(x, _BayerLength);

                return 2 * _BayerMatrix[y * _BayerLength + x] - 1;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // For dithering
                int2 puv = i.uv * _MainTex_TexelSize.zw / _ResolutionScale;

                // Resolution scale
                if(_ResolutionScale > 1)
                {
                    float2 units = 1 / _ScreenParams.xy;

                    i.uv.x = floorToNearest(i.uv.x, units.x * _ResolutionScale);
                    i.uv.y = floorToNearest(i.uv.y, units.y * _ResolutionScale);
                }
                
                // Sample the image
                fixed4 c = tex2D(_MainTex, i.uv);


                // Dithering
                fixed ditherAmount = dither(puv.x, puv.y) * _DitherStrength / 3;

                if(_LuminosityAdjustment)
                {
                    fixed luminosity = getLuminosity(c);
                    ditherAmount *= 4 * luminosity - 4 * luminosity * luminosity;
                }

                c += ditherAmount;
                c = saturate(c);


                // Fade
                c -= _Fade;
                c = saturate(c);

                switch (_ColoringMode)
                {
                case 0:
                    // Nothing

                    return c;
                case 1:
                    // Bit depth

                    c.r = roundToNearest(c.r, 1. / (pow(2, _BitDepth.r) - 1));
                    c.g = roundToNearest(c.g, 1. / (pow(2, _BitDepth.g) - 1));
                    c.b = roundToNearest(c.b, 1. / (pow(2, _BitDepth.b) - 1));

                    return c;
                case 2:
                    // Palette

                    fixed4 closestColor = _Palette[0];
                    float bestScore = getScore(closestColor, c);

                    // Get the color in the palette closest to sample

                    for (uint i = 0; i < _PaletteLength; i++) {
                        fixed4 thisColor = _Palette[i];
                        float thisScore = getScore(thisColor, c);

                        if (thisScore < bestScore) {
                            closestColor = thisColor;
                            bestScore = thisScore;
                        }
                    }

                    return fixed4(closestColor.rgb, c.a);
                case 3:
                    // Luminosity based

                    int idx = floor(getLuminosity(c) * (_PaletteLength - 1));

                    return _Palette[idx];
                case 4:
                    // Steps

                    c.r = roundToNearest(c.r, 1 / _BitDepth.r);
                    c.g = roundToNearest(c.g, 1 / _BitDepth.g);
                    c.b = roundToNearest(c.b, 1 / _BitDepth.b);

                    return c;
                }
                return 1;
            }
            ENDCG
        }
    }

    CustomEditor "LUTShaderGUI"
}
