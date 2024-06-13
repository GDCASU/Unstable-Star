Shader "Unlit/SFX/PulsingEffect"
{
    Properties
    {
        _MainColor ("Object Color", Color) = (0,0,0,0)

        _GlowingColor ("Glowing Color", Color) = (0, 1, 0, 1)
        
        _Amplitude("Amplitude", Float) = 0.25
        _Period("Period", Float) = 5
        _VerticalShift("Vertical Shift", Float) = 0.25
        _PhaseShift("Phase Shift", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcColor One

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
                float4 vertex : SV_POSITION;
            };

            float4 _MainColor;
            float4 _GlowingColor;
            float _Amplitude;
            float _Period;
            float _VerticalShift;
            float _PhaseShift;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _MainColor;
                
                float alpha = _Amplitude * sin(_Period * _Time.y + _PhaseShift) + _VerticalShift;
                col = lerp(col, _GlowingColor, alpha);
                col.a = alpha;

                return col;
            }
            ENDCG
        }
    }
}
