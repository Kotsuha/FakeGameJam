Shader "Custom/DirectionalMeltingEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Albedo texture
        _MeltAmount ("Melt Amount", Range(0, 1)) = 0.0
        _MeltSpeed ("Melt Speed", Range(0.1, 5.0)) = 1.0
        _MeltDirection ("Melt Direction", Vector) = (0, -1, 0, 0) // Default to downward direction
        _MeltProgress ("Melt Progress", Range(0, 1)) = 0.0 // Control melting progress
        _MaxMeltOffset ("Max Melt Offset", Float) = 1.0 // Maximum melt offset
        _BottomLimit ("Bottom Limit", Float) = 0.0 // Bottom limit for melting effect
        _Offset ("Offset", Float) = 0.0 // Offset for melting effect
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            ZTest LEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MeltAmount;
            float _MeltSpeed;
            float4 _MeltDirection; // XYZ for direction, W not used
            float _MeltProgress;
            float _MaxMeltOffset;
            float _BottomLimit;
            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Compute the world position of the vertex
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calculate the melt offset based on world position and direction
                float meltOffset = dot(_MeltDirection.xyz, o.worldPos) * _MeltProgress + _Offset;

                // Clamp the melt offset to the maximum value
                meltOffset = min(meltOffset, _MaxMeltOffset);

                // Compute the new vertex position and clamp it to the bottom limit
                float3 newVertexPos = o.vertex.xyz + _MeltDirection.xyz * meltOffset;
                if (newVertexPos.y < _BottomLimit)
                {
                    newVertexPos.y = _BottomLimit;
                }

                o.vertex.xyz = newVertexPos;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Darken the color based on the melt amount
                col.rgb *= (1.0 - _MeltAmount);

                return col;
            }
            ENDCG
        }
    }
}
