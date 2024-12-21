Shader "Unlit/transparentMagic"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Tiling", Float) = 1.0
        _Off ("Offset", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Zwrite Off
        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 pos : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Scale;
            float4 _Off;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv = _Scale * i.pos.xy / i.pos.w + _Off.xy * _Time.x;
                float2 vu = -_Scale * i.pos.xy / i.pos.w + _Off.zw * _Time.x;
                fixed4 col = (tex2D(_MainTex, uv) + tex2D(_MainTex, vu)) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
