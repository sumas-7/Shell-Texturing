using Godot;

[Tool]
public partial class ShellManager : Node3D
{
	[Export(PropertyHint.Range, "0, 75")]
	public int shellCount = 0;
	[Export(PropertyHint.Range, "0,1.5f")]
	public float shellsSpacing = 0.2f;
	[Export(PropertyHint.Range, "0,0.2f")]
	public float heightThresholdScaling = 0.03f;
	[Export]
	public bool generateShells = false;

	private Vector3 shellsPos = new(0, 0, 0);
	private float heightThreshold = 0.01f;
	private PackedScene shell_Scene;

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
			MeshInstance3D shell = (MeshInstance3D)shell_Scene.Instantiate();
			ShaderMaterial shellMat = (ShaderMaterial)shell.Mesh.SurfaceGetMaterial(0);

			AddChild(shell);
			shell.Position = shellsPos;
			shellMat.SetShaderParameter("shell_index", i);
			shellMat.SetShaderParameter("shell_count", shellCount);
			shellMat.SetShaderParameter("height_threshold", heightThreshold);

			shellsPos.Y += shellsSpacing;
			heightThreshold += heightThresholdScaling;
			shell.Owner = GetTree().EditedSceneRoot;
		}
		shellsPos.Y = 0;
		heightThreshold = 0;
	}

	private void DestroyShells()
	{
		for(int i = 0; i < GetChildCount(); i++) // destroys all children nodes
		{
			GetChild(i).QueueFree();
		}
	}
}