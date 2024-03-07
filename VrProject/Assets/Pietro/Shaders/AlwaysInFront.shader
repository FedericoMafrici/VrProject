Shader "Custom/AlwaysInFront"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
        SubShader
        {
            Tags { "Queue" = "Overlay" }
            //ColorMask RGB
            ZWrite On
            //Blend Off
            Lighting Off
            Cull Off
            ZTest Always
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 worldPos : TEXCOORD1;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _Color;
                float _Glossiness;
                float _Metallic;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Apply lighting if needed (here lighting is off)

                // Apply transparency if needed (here blend mode is off)

                return col;
            }
            ENDCG
        }
        }

            // Add any other SubShaders you need (e.g., for different platforms)
                Fallback "Diffuse"
}