Shader "Custom/waterSurf"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Normal ("Normal map", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Direction ("Direction", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade

        #pragma target 3.0

        sampler2D _Normal;

        struct Input
        {
            float2 uv_Normal;
            fixed facing : VFACE;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _Direction;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a;
            fixed3 norm = UnpackNormal (tex2D (_Normal, IN.uv_Normal + _Direction.xy*_Time.x))+UnpackNormal (tex2D (_Normal, IN.uv_Normal + _Direction.zw*_Time.x));
            if (IN.facing < 0) {norm = 1 - norm;}
            o.Normal = norm;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
