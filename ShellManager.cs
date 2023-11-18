using Godot;

[Tool]
public partial class ShellManager : Node3D
{
	[Export]
	private bool generateShells = false;
	
	[Export]
	private PackedScene shell_Scene;

	[Export(PropertyHint.Range, "0, 1024")]
	private int shellCount = 0;

	[Export(PropertyHint.ColorNoAlpha)]
	private Color tipColor = new(1, 1, 1), bottomColor = new(1, 1, 1);
	
	[Export(PropertyHint.Range, "0, 5f")]
	private float shellsSpacing = 0.2f;

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