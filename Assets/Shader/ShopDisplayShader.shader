Shader "Custom/ShopDisplayShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _DetailTex("Detail Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf _Display

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _DetailTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DetailTex;
        };

        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 c2 = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Emission = c2.rgb * 2;
        }

        float4 Lighting_Display(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            float4 final;

            float ndotl = dot(lightDir, viewDir);

            final.rgb = s.Emission;
            final.a = s.Alpha;

            return final;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
