using Godot;

[Tool]
public partial class ShellManager : Node3D
{
	[Export]
	private bool generateShells = false;
	
	[ExportCategory("Basic Shells Properties")]
	[Export(PropertyHint.Range, "0, 1024")]
	private int shellCount = 64;

	[Export(PropertyHint.Range, "0, 5f")]
	private float shellsSpacing = 0.7f;
	
	[Export]
	private PackedScene shell_Scene;

	[ExportGroup("Color")]
	[Export(PropertyHint.ColorNoAlpha)]
	private Color tipColor = new(1, 1, 1), bottomColor = new(1, 1, 1);

	[ExportGroup("Moving texture speeds")]
	[Export]
	private Vector2 textureSpeed;

	[ExportGroup("Wind Properties")]
	[Export]
	private float windSpeed = 1.2f, windCurveIntensity = 1.2f;
	[Export]
	private Vector2 windDirection = new(0.0f, 1.0f), windIntensity = new(1.0f, 0.0f);

	private float shellHeight;

	public override void _Ready()
	{
		shell_Scene = (PackedScene)GD.Load("res://Shell.tscn");

		if(!Engine.IsEditorHint()) // if in game
		{
			DestroyShells();
			GenerateShells();
		}
		else
			DestroyShells();
	}
    public override void _Process(double delta)
    {
		if(Engine.IsEditorHint()) // if in editor uses bools as buttons to instanciate and destroy shells
		{
			// if you press the button to generate shells
			if(generateShells)
			{
				GD.Print("Shells generated");
				DestroyShells(); // destroys all previous shells if present
				GenerateShells();
				generateShells = false; // resets the button to make it clickable again
			}
		}
    }

    void GenerateShells()
	{
		for(int i = 0; i < shellCount; i++)
		{
			shellHeight = (float)((i+1f) / shellCount); // normalized shell height

			MeshInstance3D shell = (MeshInstance3D)shell_Scene.Instantiate();
			ShaderMaterial shellMat = (ShaderMaterial)shell.Mesh.SurfaceGetMaterial(0);

			AddChild(shell);

			shellMat.SetShaderParameter("shell_index", i);
			shellMat.SetShaderParameter("shell_count", shellCount);
			shellMat.SetShaderParameter("shell_height", shellHeight);
			shellMat.SetShaderParameter("shell_spacing", shellsSpacing);

			shellMat.SetShaderParameter("uv_speed", textureSpeed);

			shellMat.SetShaderParameter("wind_speed", windSpeed);
			shellMat.SetShaderParameter("wind_curve_intensity", windCurveIntensity);
			shellMat.SetShaderParameter("wind_dir", windDirection);
			shellMat.SetShaderParameter("wind_intensity", windIntensity);

			shellMat.SetShaderParameter("tip_color", tipColor);
			shellMat.SetShaderParameter("bottom_color", bottomColor);

			shell.Owner = GetTree().EditedSceneRoot;
		}
	}

	private void DestroyShells()
	{
		for(int i = 0; i < GetChildCount(); i++) // destroys all children nodes
		{
			GetChild(i).QueueFree();
		}
	}
}