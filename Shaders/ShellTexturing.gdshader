shader_type spatial;
render_mode cull_disabled;

uniform float shell_index; // current shell pos (0 1 2 3 4 5 ecc.)
uniform float shell_count; // total shells
uniform float shell_spacing; // the space between shells
uniform float height_threshold; // required height value to be colored
uniform sampler2D noise; // 2d noise texture

varying float shell_height;

void vertex()
{
	shell_height = shell_index / shell_count; // normalized shell position
	VERTEX.y += shell_height * shell_spacing;
}
void fragment()
{
	float height = texture(noise, UV).r; // texture data as float based on coord on the plane

	if(height > height_threshold)
	{
		ALBEDO = vec3(0.0, 0.1 + 1.0 * shell_height, 0.0);
	}
	else
	{
		discard;
	}
}