#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

struct CustomLightingData {
	// Position and orientation
	float3 positionWS;
	float3 normalWS;
	float3 viewDirectionWS;
    // Surface attributes
    float3 albedo;
};


#ifndef SHADERGRAPH_PREVIEW
float3 CustomLightHandling(CustomLightingData d, Light light) {

	float3 radiance = light.color * (light.distanceAttenuation * light.shadowAttenuation);

	float diffuse = saturate(dot(d.normalWS, light.direction));
	float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
	float specular = specularDot * diffuse;

	float3 color = d.albedo * radiance * (diffuse + specular);

	return color;
}
#endif

float3 CalculateCustomLighting(CustomLightingData d) {
#ifdef SHADERGRAPH_PREVIEW
	// In preview, estimate diffuse + specular
	float3 lightDir = float3(0.5, 0.5, 0);
	float intensity = saturate(dot(d.normalWS, lightDir));
	return d.albedo * intensity;
#else
	// Get the main light. Located in URP/ShaderLibrary/Lighting.hlsl
	Light mainLight = GetMainLight();

	float3 color = 0;
	// Shade the main light
	color += CustomLightHandling(d, mainLight);

	#ifdef _ADDITIONAL_LIGHTS
		// Shade additional cone and point lights. Functions in URP/ShaderLibrary/Lighting.hlsl
		uint numAdditionalLights = GetAdditionalLightsCount();
		for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
			Light light = GetAdditionalLight(lightI, d.positionWS, 1);
			color += CustomLightHandling(d, light);
		}
	#endif

	return color;
#endif
}

void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 ViewDirection,
	float3 Albedo,
	out float3 Color, out float3 AdditionalLightDirection) {

	CustomLightingData d;
	d.positionWS = Position;
	d.normalWS = Normal;
	d.viewDirectionWS = ViewDirection;
	d.albedo = Albedo;

	#ifdef SHADERGRAPH_PREVIEW
		// In preview, estimate diffuse + specular
		float3 lightDir = float3(0.5, 0.5, 0);
		float intensity = saturate(dot(d.normalWS, lightDir));
		Color =  d.albedo * intensity;
		AdditionalLightDirection = lightDir;
	#else
		// Get the main light. Located in URP/ShaderLibrary/Lighting.hlsl
		Light mainLight = GetMainLight();
		float3 color = 0;
		// Shade the main light
		color += CustomLightHandling(d, mainLight);

	#ifdef _ADDITIONAL_LIGHTS
		// Shade additional cone and point lights. Functions in URP/ShaderLibrary/Lighting.hlsl
		float3 additionalLightDir = float3(0,0, 0);
		uint numAdditionalLights = GetAdditionalLightsCount();
		for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
			Light light = GetAdditionalLight(lightI, d.positionWS, 1);
			additionalLightDir += light.direction;
			color += CustomLightHandling(d, light);
		}
		AdditionalLightDirection = additionalLightDir;
	#else
		AdditionalLightDirection = GetMainLight().direction;
	#endif

		Color = color;
	#endif
}

#endif