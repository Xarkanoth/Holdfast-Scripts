Shader "Xarks/WorldFogDome"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.8, 0.85, 0.9, 0.3)
        _FogDensity("Fog Density", Float) = 0.3
        _MinFog("Minimum Fog", Float) = 0.2
        _Falloff("Fog Falloff Distance", Float) = 500
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Front

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
            };

            float4 _FogColor;
            float _FogDensity;
            float _MinFog;
            float _Falloff;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.worldPos = TransformObjectToWorld(input.positionOS).xyz;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float3 camPos = GetCameraPositionWS();
                float distance = distance(input.worldPos, camPos);
                float fade = saturate(distance / _Falloff);
                float fog = lerp(_MinFog, _FogDensity, fade);

                return float4(_FogColor.rgb, fog * _FogColor.a);
            }
            ENDHLSL
        }
    }
}
