Shader "Images/lightningShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTex("Noise", 2D) = "white" {}
        _Col1("Color 1", Color) = (0,0,0,0)
        _Col2("Color 2", Color) = (0,0,0,0)
        _Dir("Direccion", Vector) = (0,0,0,0)
        _Vel("Velocidades", Vector) = (0,0,0,0)
        _Size("Tamanos", Vector) = (0,0,0,0)
        _Weight("Pesos", Vector) = (0,0,0,0)
        _B("Grosor", float) = 1
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _Col1;
            float4 _Col2;
            float4 _Dir;
            float4 _Vel;
            float4 _Size;
            float4 _Weight;
            float _B;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float4 noise;
                noise.x = tex2D(_NoiseTex, i.uv * _Size.x + _Dir.xy * _Vel.x * _Time.x) - tex2D(_NoiseTex, i.uv * _Size.x + _Dir.zw * _Vel.x * _Time.x);
                noise.y = tex2D(_NoiseTex, i.uv * _Size.y + _Dir.xy * _Vel.y * _Time.x) - tex2D(_NoiseTex, i.uv * _Size.y + _Dir.zw * _Vel.y * _Time.x);
                noise.z = tex2D(_NoiseTex, i.uv * _Size.z + _Dir.xy * _Vel.z * _Time.x) - tex2D(_NoiseTex, i.uv * _Size.z + _Dir.zw * _Vel.z * _Time.x);
                noise.w = tex2D(_NoiseTex, i.uv * _Size.w + _Dir.xy * _Vel.w * _Time.x) - tex2D(_NoiseTex, i.uv * _Size.w + _Dir.zw * _Vel.w * _Time.x);
                float noise1 = dot(noise, _Weight);
                noise1 = clamp(1 - abs(noise1) * _B, 0, 1);
                noise1 = noise1 * noise1 * noise1;
                fixed4 color;
                color.a = lerp(_Col1.a, _Col2.a, 2 * clamp(noise1, 0, 0.5));
                color.rgb = lerp(_Col1.rgb, _Col2.rgb, 2 * clamp(noise1 * noise1, 0.5, 1) - 1);
                col.rgb = col.rgb + (color.rgb-col.rgb) * color.a;
                return col;
            }
            ENDCG
        }
    }
}
