shader_type spatial;
render_mode unshaded, cull_disabled;

// shell generation parameters
uniform float shell_index; // current shell pos (0 1 2 3 4 5 ecc.)
uniform float shell_count; // total shells
uniform float shell_spacing; // the space between shells

// texture moving speed (useful for example in a sphere)
uniform float uvXSpeed = 0.0;
uniform float uvYSpeed = 0.0;

// fake wind parameters
uniform float wind_speed;
uniform float wind_curve_intensity;
uniform vec2 wind_dir;
uniform vec2 wind_intensity;

// various parameters and values
uniform float height_threshold; // required height for the pixel to exist
uniform vec3 tip_color;
uniform vec3 bottom_color; // color of the tip and the bottom
uniform sampler2D noise; // 2d noise texture

varying float shell_height;

void vertex()
{
	shell_height = shell_index / shell_count; // normalized shell position
	
	VERTEX += NORMAL * shell_height * shell_spacing; // displace the shells
	
	// fake wind
	float fakeWind = (sin(TIME + shell_height * wind_curve_intensity) * sin(TIME * wind_speed)) * shell_height;
	VERTEX.x += sin(shell_height * wind_dir.y) + fakeWind * wind_intensity.y;
	VERTEX.z += sin(shell_height * wind_dir.x) + fakeWind * wind_intensity.x;
}
void fragment()
{
	vec2 uv = UV;
	uv *= 5.0;

	//fake rotation
	uv.x += TIME * uvXSpeed;
	uv.y += TIME * uvYSpeed;
	
	float height = texture(noise, uv).r; // texture data as float based on coord on the plane
	
	if(height > height_threshold)
	{
		ALBEDO = mix(bottom_color, tip_color, shell_height);
	}
	else
	{
		discard;
	}
}