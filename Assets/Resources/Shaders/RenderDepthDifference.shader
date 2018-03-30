// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Custom/RenderDepthDifference"
 {
     Properties 
     {
         _MainTex ("Base (RGB)", 2D) = "white" {} 
     }
     SubShader 
     { 
         Pass 
         {
             CGPROGRAM
			 // We only target the HoloLens (and the Unity editor), so take advantage of shader model 5.
			 #pragma target 5.0
			 #pragma only_renderers d3d11
			 #pragma vertex vert
             #pragma fragment frag
			 #include "UnityCG.cginc"

             uniform sampler2D _MainTex;
             uniform sampler2D _LastCameraDepthTexture; // Last depth which is in the buffer (reference)
			 uniform sampler2D _CameraDepthTexture;     // Current (live camera) depth
			 uniform fixed _DepthDifferenceThreshold;   // Threshold on the difference due to sensor imprecision
			 uniform fixed _SpatialMappingIntensity;    // Intensity in color of the texture of the spatial mapping
             uniform half4 _MainTex_TexelSize;
			 fixed4 _Color;

             struct input
             {
                 float4 pos : POSITION;
                 half2 uv : TEXCOORD0;
             };
 
             struct output
             {
                 float4 pos : SV_POSITION;
                 half2 uv : TEXCOORD0;
             };
 
             output vert(input i)
             {
                 output o; 
                 o.pos = UnityObjectToClipPos(i.pos); 
                 o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, i.uv);
                 // Avoid flipped image. see: http://docs.unity3d.com/Manual/SL-PlatformDifferences.html
                 #if UNITY_UV_STARTS_AT_TOP
                 if (_MainTex_TexelSize.y < 0)
                         o.uv.y = 1 - o.uv.y;
                 #endif
                 return o;
             }

			fixed4 frag(output o) : COLOR
             {
				
				 // The depths are between 0 (max sensor range ~ infity) and 1 (closest ~ distance 0)
                 float ReferenceDepth = UNITY_SAMPLE_DEPTH(tex2D(_LastCameraDepthTexture, o.uv));
				 float LiveDepth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, o.uv));

				 float DepthDifference = Linear01Depth(LiveDepth) - Linear01Depth(ReferenceDepth);
				 //float DepthDifference = ReferenceDepth - LiveDepth;

				 // Lowest possible shade for the color, also influences range available (b/w 0 and 1)
				 float MinIntensity = 0.1;

				 // Factor that makes colors brighter
				 float ColorFactor = 3;

				 // Closest object for depth-impression lighting effects 
				 float NearestDepth = max(ReferenceDepth, LiveDepth);

				 // Checks whether live view is in front or behind scanned model and colors it
				 if (DepthDifference > _DepthDifferenceThreshold / 2) {
					 // Red
					 _Color = ColorFactor*fixed4(MinIntensity + (1-MinIntensity)*NearestDepth,0,0,0.1);
				 } else if (DepthDifference < -_DepthDifferenceThreshold / 2) {
					 // Blue
					 _Color = ColorFactor*fixed4(0,0, (MinIntensity + (1 - MinIntensity)*NearestDepth),0.1);
				 } else {
					 // Transparent
					 _Color = fixed4(0, 0, 0, 0);
				 }

				 //return _Color;
				 // Adds color to the existing texture (if it exists)
				 return tex2D(_MainTex, o.uv) == 0 ? _Color : _SpatialMappingIntensity*tex2D(_MainTex, o.uv) + _Color;
			}
             ENDCG
         }	 
     }
 }