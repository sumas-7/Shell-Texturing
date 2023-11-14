shader_type spatial;
render_mode unshaded, cull_disabled;

uniform float shell_index; // current shell pos (0 1 2 3 4 5 ecc.)
uniform float shell_count; // total shells
uniform float shell_spacing; // the space between shells
uniform float height_threshold; // required height for the pixel to exist
uniform vec3 tip_color;
uniform vec3 bottom_color; // color of the tip and the bottom
uniform sampler2D noise; // 2d noise texture

varying float shell_height;

void vertex()
{
	shell_height = shell_index / shell_count; // normalized shell position
	VERTEX += NORMAL * shell_height * shell_spacing;
}
void fragment()
{
	float height = texture(noise, UV).r; // texture data as float based on coord on the plane

	if(height > height_threshold)
	{
		ALBEDO = mix(bottom_color, tip_color, shell_height);
	}
	else
	{
		discard;
	}
}