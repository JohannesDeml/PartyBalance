Shader "Supyrb/Skybox/ScreenspaceGradient"
{
	Properties
	{
		_Color0 ("Color Top", Color) = (1, 1, 1, 1)
		_Color1 ("Color Bottom", Color) = (1, 1, 1, 1)
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	
	fixed4 _Color0;
	fixed4 _Color1;
	
	struct v2f
	{
		float4 position : SV_POSITION;
		half2 normalizedScreenPos : TEXCOORD0;
	};
	
	v2f vert (float4 vertex : POSITION)
	{
		v2f o;
		o.position = UnityObjectToClipPos (vertex);
		float4 screenPos = ComputeScreenPos(o.position);
		o.normalizedScreenPos = screenPos.xy / max(screenPos.w, 0.001);
		return o;
	}
	
	fixed4 frag (v2f i) : COLOR
	{
		return lerp (_Color1, _Color0, i.normalizedScreenPos.y);
	}
	ENDCG

	SubShader
	{
		Tags { "RenderType"="Background" "Queue"="Background" }
		Pass
		{
			ZWrite Off
			Cull Off
			Fog { Mode Off }
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
