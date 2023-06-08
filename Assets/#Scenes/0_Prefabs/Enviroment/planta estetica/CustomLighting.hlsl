
#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif

struct CustomLightingData{
	
	float3 positionWS;
	float3 viewDirectionWS;
	float4 shadowCoord;


	float3 albedo;

	float3 bakedGI;
};

#ifndef SHADERGRAPH_PREVIEW
float3 CustomGlobalIllumination(CustomLightingData d) 
{
	float3 indirectDiffuse = d.albedo * bakedGI;

	return indirectDiffuse;
}

float3 CustomLightHandling(CustomLightingData d, Light light) {

	float3 radiance = light.shadowAttenuation;
	float3 color = d.albedo * radiance;

	return color;
}
#endif

float3 CalculateCustomLighting(CustomLightingData d) 
{
#ifdef SHADERGRAPH_PREVIEW
	return d.albedo;
#else
	Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);

	MixRealtimeAndBakedGI(mainLight,d.normalWS,d.bakedGI);

	float3 color = 0;

	color += CustomLightHandling(d, mainLight);

	return color;
#endif
}

void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 ViewDirection, float3 Albedo, float AmbientOcclusion, out float3 Color) 
{
	CustomLightingData d;
	d.viewDirectionWS = ViewDirection;
	d.albedo = Albedo;
	

#ifdef SHADERGRAPH_PREVIEW
	d.shadowCoord = 0;
	d.bakedGI = 0;
#else
	float4 positionCS = TransformWorldToHClip(Position);
#if SHADOWS_SCREEN
	d.shadowCoord = ComputeScreenPos(positionCS);
#else
	d.shadowCoord = TransformWorldToShadowCoord(Position);
#endif

	//the following URP functions and macros

	float3 lightmapUV;
	OUTPUT_LIGHTMAP_UV(LightmapUV, unity_LightmapST, lightmapUV);

	float3 vertexSH;
	OUTPUT_SH(Normal, vertexSH);
	// This func
	d.bakedGI = SAMPLE_GI(lightmapUV, vertexSH, Normal);

#endif

	Color = CalculateCustomLighting(d);
}

#endif