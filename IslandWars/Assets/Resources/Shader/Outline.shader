Shader "Custom/Outline_2DSprite" {
    Properties 
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutLineSpread ("Outline Spread", Range(0,0.012)) = 0.007
    }
 
    SubShader
 
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite On Blend One OneMinusSrcAlpha Cull Off
        LOD 110
 
        CGPROGRAM
        #pragma surface surf Lambert alpha
 
        struct Input 
        {
            float2 uv_MainTex;
            fixed4 color : COLOR;
        };
 
        sampler2D _MainTex;
        float _OutLineSpread;
 
        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 mainColor = (tex2D(_MainTex, IN.uv_MainTex+float2(_OutLineSpread,_OutLineSpread)) + tex2D(_MainTex, IN.uv_MainTex-float2(_OutLineSpread,_OutLineSpread))) * fixed4(255,255,0,1);
            fixed4 addcolor = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
 
            if(addcolor.a > 0.95){
            mainColor = addcolor;}
 
            o.Albedo = mainColor.rgb;
            o.Alpha = mainColor.a;
        }
        ENDCG       
    }
 
    SubShader 
    {
       Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off Blend One OneMinusSrcAlpha Cull Off Fog { Mode Off }
        LOD 100
        Pass {
            Tags {"LightMode" = "Vertex"}
            ColorMaterial AmbientAndDiffuse
            Lighting On
            SetTexture [_MainTex] 
            {
                Combine texture * primary double, texture * primary
            }
        }
    }
    Fallback "Diffuse", 1
}