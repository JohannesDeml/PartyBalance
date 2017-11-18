// ----------------------------------------------------------------------------
// <copyright file="Confetti.shader" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// ----------------------------------------------------------------------------

// <summary>
//   Unlit Particle shader with random hued particles and fake reflections
//   Uses a custom Vertex stream (one value with a random number between 0 and 6)
//   Hue is random, saturation and value are always 1
//   HUE	 |012345|
//   RED1    |_    _|
//   RED0    | \__/ |
//   GREEN1  | __   |
//   GREEN0  |/  \__|
//   BLUE1   |   __ |
//   BLUE0   |__/  \|
// </summary>
Shader "Unlit/Confetti"
{
	Properties
	{
		_ReflectionThreshold("Reflection Threshold", Range(0,1)) = 0.7
		_ReflectionMultiplier("Reflection Multiplier", Range(0,8)) = 3
		
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	fixed _ReflectionThreshold;
	fixed _ReflectionMultiplier;
	
	struct appdata
	{
		float4 vertex : POSITION;
		float custom : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		fixed4 color : COLOR;
		float4 vertex : SV_POSITION;
		float3 worldNormal : NORMAL;
		float3 objToCamera : TEXCOORD0;
	};

	v2f vert (appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		float hue = v.custom;
		fixed colorX = 1.0 - max(abs(2.0 - clamp(fmod(hue+2.0,6.0), 0.0, 4.0))-1.0, 0.0);
		fixed colorY =  1.0 - max(abs(2.0 - clamp(hue, 0.0, 4.0))-1.0, 0.0);
		fixed colorZ =  1.0 - max(abs(4.0 - clamp(hue, 2.0, 6.0))-1.0, 0.0);
		o.color = fixed4(colorX, colorY, colorZ, 1.0);
		o.worldNormal = UnityObjectToWorldNormal(v.normal);
		o.objToCamera = WorldSpaceViewDir(o.vertex);
		return o;
	}
	
	fixed4 frag (v2f i) : SV_Target
	{
		half cameraNormalAngle = abs(dot(normalize(i.worldNormal), normalize(i.objToCamera)));
		fixed4 whiteReflection = (_ReflectionMultiplier).xxxx * max(cameraNormalAngle - _ReflectionThreshold, 0.0);
		return i.color + whiteReflection;
	}
	ENDCG
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
