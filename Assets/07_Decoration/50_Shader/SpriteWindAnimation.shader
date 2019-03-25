﻿// Upgrade NOTE: upgraded instancing buffer 'PerDrawSprite' to new syntax.

// ----------------------------------------------------------------------------
// <copyright file="SpriteWindAnimation.shader" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// ----------------------------------------------------------------------------

// <summary>
//   Sprite dancing in the wind (sin wave)
// </summary>
Shader "Sprites/WindAnimation"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Frequency("Wind Frequency", Range(0.0, 16.0)) = 2
		_Amplitude("Wind Amplitude", Range(-4.0, 4.0)) = 0.3
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"

	#ifdef UNITY_INSTANCING_ENABLED

    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
        // SpriteRenderer.Color while Non-Batched/Instanced.
        UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
        // this could be smaller but that's how bit each entry is regardless of type
        UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

    #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
    #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

	#endif // instancing

	CBUFFER_START(UnityPerDrawSprite)
	#ifndef UNITY_INSTANCING_ENABLED
		fixed4 _RendererColor;
		fixed2 _Flip;
	#endif
		float _EnableExternalAlpha;
	CBUFFER_END

	// Material Color.
	fixed4 _Color;
	float _Frequency;
	float _Amplitude;

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
	{
		return float4(pos.xy * flip, pos.z, 1.0);
	}
	
	v2f vert(appdata_t IN)
	{
		v2f OUT;

		UNITY_SETUP_INSTANCE_ID (IN);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
		
		OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
		OUT.vertex.x += sin(_Time.w * _Frequency) * IN.vertex.y * _Amplitude;
		OUT.vertex = UnityObjectToClipPos(OUT.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color * _RendererColor;

		#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap (OUT.vertex);
		#endif

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;

	fixed4 SampleSpriteTexture (float2 uv)
	{
		fixed4 color = tex2D (_MainTex, uv);

	#if ETC1_EXTERNAL_ALPHA
		fixed4 alpha = tex2D (_AlphaTex, uv);
		color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
	#endif

		return color;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
		c.rgb *= c.a;
		return c;
	}
	
	ENDCG
	
	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		ENDCG
		}
	}
}