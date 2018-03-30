// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Custom/RenderDepth" //The shader take the name after the slash. In this case the name is RenderDepth and not Custom/RenderDepth.
 {
     Properties //Equivalent to public fields in a C# script. Gives access from the inspector to the hidden variables within a shader.
     {
         _MainTex ("Base (RGB)", 2D) = "white" {} //Type 2D indicates that the parameters are textures. They can be initialised to white, black or gray. You can also use bump to indicate that the texture will be used as a normal map.
         //I don't understand why we need this texture.
		 _DepthLevel ("Depth Level", Range(0, 1)) = 1 //Internal name - Inspector title - Property type - Default value
     }
     SubShader //Can have multiple SubShader. Code runs till shader found compatible with GPU. Defines the variables. Executed for every pixel of the image.
     {
         Pass //The Cg section of vertex & fragment shaders need to be enclosed in a Pass section (more about this later). Can have a collection of passes. For each pass, the object geometry is rendered, so there must be at leas one pass.
         {
             CGPROGRAM //The lines between CGPROGRAM and ENDCG contain the actual Cg code.
			 // We only target the HoloLens (and the Unity editor), so take advantage of shader model 5.
			 #pragma target 5.0
			 #pragma only_renderers d3d11
			 //There is 2 types of shaders (surface and vertex & fragment shaders). The latter is used in this shader. Surface shaders are used for materials that need to be affected by light in a realistic way.
             #pragma vertex vert //Compile function vert as the vertex shader.
             #pragma fragment frag //Vertex & fragment shaders work close to the way the GPU renders triangles, and have no built-in concept of how light should behalf. The geometry of your model is first passed 
									//through a function called vert which can alter its vertices. Then, individual triangles are passed through another function called frag which decides the final RGB colour for every pixel.
			 #include "UnityCG.cginc"
             
             uniform sampler2D _MainTex; // is the type used for texture
             uniform sampler2D _CameraDepthTexture; //Declares a sampler that is able to sample the main depth texture for the camera. It always refers to the camera's primary depth texture. 
													//By contrast, you can use _LastCameraDepthTexture to refer to the last depth texture rendered by any camera.
             uniform fixed _DepthLevel;
             uniform half4 _MainTex_TexelSize; //A texel, texture element is the fundamental unit of a texture map. Textures are represented by arrays of texels representing the texture space, just as other images are represented by arrays of pixels.
			 //When texturing a 3D surface or surface, the renderer maps texels to appropriate pixel in the output picture.
 
             struct input
             {
				 //When a colon is placed after a variable or a function (binding semantic), it is used to indicate that the variable itself will play a special role.
				 //For example, this is how vertInput is actually initalised; The first line indicates that we want Unity3D to initialise pos with the vertex position.
				 // POSITION, SV_POSITION, COLOR, TEXCOORD0 are some of the most common binding semantics available in Cg for the field of vertInput and vertOutput.
                 float4 pos : POSITION; //The position of the vertex in world coordinates (object space). Vectors are defines as a type float4
                 half2 uv : TEXCOORD0; //UV mapping is the 3D modeling process of projecting a 2D image to a 3D model's surface for texture mapping.
             };
 
             struct output
             {
                 float4 pos : SV_POSITION; //Initialise with screen position of a vertex. Despite requiring only two values (X and Y), SV_POSITION typically contains also a Z and W components, used to store the depth (ZTest) and one value for the homogeneous space, respectively.
                 half2 uv : TEXCOORD0;
             };
 
             output vert(input i) //What vert receives is he position of a vertices in world coordinates, which has to be converted into screen coordinates (model-view projection).
             {
                 output o; //The struct called vertOutput can take e.g. the screen position of a vertex. The output will later handed to the frag function.
                 o.pos = UnityObjectToClipPos(i.pos); //Converts the vertices from their native 3D space to their final 2D position on the screen. UNITY_MATRIX_MVP hides the math behind it.
                 o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, i.uv);
                 // Avoid flipped image. see: http://docs.unity3d.com/Manual/SL-PlatformDifferences.html
                 #if UNITY_UV_STARTS_AT_TOP
                 if (_MainTex_TexelSize.y < 0) //Needs to be done because the 0 position is different in Direct3D-like and OpenGL-like
                         o.uv.y = 1 - o.uv.y;
                 #endif
                 return o;
             }
             
             fixed4 frag(output o) : COLOR //Gives colour to every pixel. Since vertex & fragment shaders don't have any notion of lighting, returning red means that the entire model will be #ffoooo red, with no shades or details; just a red silhouette.
             {
                 float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, o.uv)); //tex2D gets the value at the uv value; 
                 depth = pow(Linear01Depth(depth), _DepthLevel); //pow := power; Linear01Depth(depth) gives high precision value from depth texture o, retruns corresponding linear depth in range between 0 and 1.
                 return depth;
             }   
             ENDCG
         }
     }
 }