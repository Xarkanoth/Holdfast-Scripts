Shader "Xarks/WorldFogDomeV2"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.8, 0.85, 0.9, 0.3)
        _FogDensity("Fog Density", Float) = 0.3
        _MinFog("Minimum Fog", Float) = 0.2
        _Falloff("Fog Falloff Distance", Float) = 500
        _NoiseScale("Noise Scale", Float) = 0.1
        _NoiseStrength("Noise Strength", Float) = 1.0
        _TimeScale("Noise Scroll Speed", Float) = 0.2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off // Keep it inward-facing
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            float4 _FogColor;
            float _FogDensity;
            float _MinFog;
            float _Falloff;
            float _NoiseScale;
            float _NoiseStrength;
            float _TimeScale;


            // float3 _WorldSpaceCameraPos;

            // Simple 3D hash noise function
            float hash(float3 p)
            {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            float noise(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                float n = lerp(
                    lerp(
                        lerp(hash(i), hash(i + float3(1, 0, 0)), f.x),
                        lerp(hash(i + float3(0, 1, 0)), hash(i + float3(1, 1, 0)), f.x), f.y),
                    lerp(
                        lerp(hash(i + float3(0, 0, 1)), hash(i + float3(1, 0, 1)), f.x),
                        lerp(hash(i + float3(0, 1, 1)), hash(i + float3(1, 1, 1)), f.x), f.y),
                    f.z);
                return n;
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 worldPos = TransformObjectToWorld(input.positionOS).xyz;
                output.worldPos = worldPos;
                output.viewDir = normalize(worldPos - _WorldSpaceCameraPos);
                output.positionHCS = TransformWorldToHClip(worldPos);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float3 camPos = _WorldSpaceCameraPos;
                float dist = distance(input.worldPos, camPos);

                // Distance-based falloff
                float baseFog = lerp(_MinFog, _FogDensity, saturate(dist / _Falloff));

                // Vertical fade (optional, makes ground clearer)
                float verticalFade = saturate((input.worldPos.y - camPos.y + 10) / 20.0);

                // Fresnel effect (fade edges)
                float fresnel = pow(1.0 - saturate(dot(input.viewDir, float3(0, 1, 0))), 2.0);

                // World-space scrolling noise
                float3 noiseInput = input.worldPos * _NoiseScale + (_Time.y * _TimeScale);
                float n = noise(noiseInput);
                float noiseFog = lerp(0.0, _NoiseStrength, n);

                float finalAlpha = baseFog * verticalFade * fresnel + noiseFog;

                return float4(_FogColor.rgb, saturate(finalAlpha));
            }
            ENDHLSL
        }
    }
}