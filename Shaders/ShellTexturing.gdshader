shader_type spatial;

uniform float height_threshold;
uniform sampler2D noise;

void fragment()
{
	float height = texture(noise, UV).r;
	
	if(height > height_threshold)
	{
		ALBEDO = vec3(0.0, 1.0, 0.0);
	}
	else
	{
		ALPHA = 0.0;
	}
}