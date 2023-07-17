Shader "Custom/HalfLambert"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf HalfLambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        float4 LightingHalfLambert(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            float3 Diff;
            float nDotl = dot(s.Normal, lightDir);
            nDotl = pow(nDotl, 3);
            float halfLambert = nDotl * 0.5f + 0.5f;
            Diff.rgb = s.Albedo * halfLambert * atten * _LightColor0;

            float3 H = normalize(lightDir + viewDir);   // ºûÀÌ µé¾î¿À´Â º¤ÅÍ¿Í ½Ã¼± º¤ÅÍÀÇ Áß°£ º¤ÅÍ
            float spec = max(0, dot(H, s.Normal));
            spec = pow(spec, 80) / 2;

            float4 final;
            final.rgb = Diff + spec;
            final.a = s.Alpha;

            return final;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
