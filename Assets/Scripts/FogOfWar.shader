Shader "Custom/FogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0, 0, 0, 0.7)
        _TintColor ("Tint Color", Color) = (1, 1, 1, 0.2)
        _VisionRadius ("Vision Radius", Float) = 5.0
        _PlayerPos ("Player Position", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags {"Queue"="Overlay" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always

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
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _FogColor;
            float4 _TintColor;
            float _VisionRadius;
            float4 _PlayerPos;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float2 playerScreenPos = _PlayerPos.xy;
                
                float distance = length(screenUV - playerScreenPos) * 2.0;
                float reveal = smoothstep(_VisionRadius + 0.2, _VisionRadius - 0.2, distance);
                
                fixed4 fogCol = _FogColor;
                fogCol.a = lerp(_TintColor.a, fogCol.a, reveal);
                
                fixed4 tintCol = _TintColor;
                tintCol.a = (1.0 - reveal) * _TintColor.a;
                
                return fogCol + tintCol;
            }
            ENDCG
        }
    }
}
