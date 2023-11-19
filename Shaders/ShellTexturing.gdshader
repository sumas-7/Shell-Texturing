shader_type spatial;
render_mode unshaded, cull_disabled;

// shell generation parameters
uniform float shell_index; // current shell pos (0 1 2 3 4 5 ecc.)
uniform float shell_count; // total shells
uniform float shell_height; // normalized shell height (between 0 and 1)
uniform float shell_spacing; // the space between shells

// texture moving speed (useful for example in a sphere)
uniform vec2 uv_speed;

// fake wind parameters
uniform float wind_speed;
uniform float wind_curve_intensity;
uniform vec2 wind_dir;
uniform vec2 wind_intensity;

// color of the tip and the bottom
uniform vec3 tip_color;
uniform vec3 bottom_color;

// others
uniform float tiling;

// 2d noise texture
uniform sampler2D noise;

void vertex()
{	
	VERTEX += NORMAL * shell_height * shell_spacing; // displace the shells
	
	// fake wind
	float fakeWind = (sin((TIME + (VERTEX.x  * wind_speed) + (VERTEX.z  * wind_speed)) + shell_height * wind_curve_intensity) * sin(TIME * wind_speed)) * shell_height;
	VERTEX.x += sin(shell_height * wind_dir.y) + fakeWind * wind_intensity.y;
	VERTEX.z += sin(shell_height * wind_dir.x) + fakeWind * wind_intensity.x;
}
void fragment()
{
	vec2 uv = UV;
	uv *= tiling;

	//fake rotation
	uv.x += TIME * uv_speed.x;
	uv.y += TIME * uv_speed.y;
	
	float height = texture(noise, uv).r; // texture data as float based on coord on the plane
	
	if(height > shell_height)
	{
		ALBEDO = mix(bottom_color, tip_color, shell_height);
	}
	else
	{
		discard;
	}
}