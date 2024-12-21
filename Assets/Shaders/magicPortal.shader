Shader "Unlit/magicPortal"
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
        Tags { "RenderType"="Opaque" }
        LOD 100


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 posPantalla : TEXCOORD0;
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
                o.posPantalla = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = _Scale * i.posPantalla.xy / i.posPantalla.w + _Off.xy * _Time.x;
                float2 vu = _Scale * i.posPantalla.xy / i.posPantalla.w + _Off.zw * _Time.x;
                fixed4 col = (1 - (1 - tex2D(_MainTex, uv)) * (1 - tex2D(_MainTex, vu))) / 2 * _Color;
                return col;
            }
            ENDCG
        }
    }
}
