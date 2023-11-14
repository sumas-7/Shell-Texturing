using Godot;

[Tool]
public partial class ShellManager : Node3D
{
	[Export]
	private bool generateShells = false;

	[Export]
	private PackedScene shell_Scene;
	
	[Export(PropertyHint.Range, "0, 75")]
	private int shellCount = 0;

	[Export(PropertyHint.Range, "0, 1.5f")]
	private float shellsSpacing = 0.2f;

	[Export(PropertyHint.Range, "0, 0.2f")]
	private float heightThresholdScaling = 0.03f;
	
	

	private float heightThreshold = 0.01f;

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
			shellMat.SetShaderParameter("shell_index", i);
			shellMat.SetShaderParameter("shell_count", shellCount);
			shellMat.SetShaderParameter("shell_spacing", shellsSpacing);
			shellMat.SetShaderParameter("height_threshold", heightThreshold);

			heightThreshold += heightThresholdScaling;

			shell.Owner = GetTree().EditedSceneRoot; // updates the editor to show the newly added nodes
		}
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