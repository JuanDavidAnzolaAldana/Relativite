Shader "Images/hurtView"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Total ("Percentage effect", float) = 0
        _Tint ("Tint color", Color) = (1,1,1,1)
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

            fixed _Total;
            fixed4 _Tint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed bw;
                fixed4 dal;
                bw = (col.r + col.g + col.b) / 3;
                dal.r = max(col.r, bw);
                dal.g = min((col.g + col.b) / 2, bw);
                dal.b = dal.g;
                dal.a = 1;
                return lerp(col, dal * _Tint, _Total);
            }
            ENDCG
        }
    }
}
