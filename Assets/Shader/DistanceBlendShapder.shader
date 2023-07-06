Shader "Custom/DistanceBlendShapder"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Dist("Fading Distance", float) = 0
        _MinAlpha("Minimum Alpha", float) = 0
        _MaxAlpha("Maximum Alpha", float) = 0
        _Power("Fading Power", float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend

        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        fixed4 _Color;
        float _Dist;
        float _MinAlpha;
        float _MaxAlpha;
        float _Power;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            float dist = distance(IN.worldPos, _WorldSpaceCameraPos);   //tanh ( dist * power - offset ) * gap/2 + gap/2
            

            float gap = (_MaxAlpha - _MinAlpha) * 0.5f;
            float a = tanh(dist * _Power - _Dist) * gap + gap + _MinAlpha;  // Power°¡ ¹®Á¦°¡ ÀÖ¾î...¹º°¡ ¹º°¡...

            //a = dist - 1;
            
            o.Emission = _Color.rgb;
            o.Alpha = saturate(a);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
