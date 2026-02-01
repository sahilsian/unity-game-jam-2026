Shader "Custom/FogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0,0,0,0.8)
        _PlayerPos ("Player Position", Vector) = (0.5, 0.5, 0, 0)
        _VisionRadius ("Vision Radius", Float) = 0.2
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        ZTest Always
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _FogColor;
            float4 _PlayerPos;
            float _VisionRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the original scene
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Calculate distance from player
                float2 playerScreenPos = _PlayerPos.xy;
                float dist = distance(i.uv, playerScreenPos);
                
                // Create circular vision area
                float fog = smoothstep(_VisionRadius - 0.05, _VisionRadius, dist);
                
                // Mix scene color with fog
                return lerp(col, _FogColor, fog);
            }
            ENDCG
        }
    }
}