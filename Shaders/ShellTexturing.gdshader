shader_type spatial;
render_mode cull_disabled;

uniform float shell_index;
uniform float shell_count;
uniform float height_threshold;
uniform sampler2D noise;

void fragment()
{
	float height = texture(noise, UV).r;
	float shell_height = shell_index / shell_count;
	
	if(height > height_threshold)
	{
		ALBEDO = vec3(0.0, 0.1 + 1.0 * shell_height, 0.0);
	}
	else
	{
		ALPHA = 0.0;
	}
}