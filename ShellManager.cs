using Godot;

[Tool]
public partial class ShellManager : Node3D
{
	[Export]
	public int shellCount;
	[Export]
	public bool generateShells = false;

	private float shellHeight = 0;
	private float height_threshold = 0.01f;
	private bool alreadyGenerated = false;
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
			// if you press the button to generate and there are no shells, generates them
			if(generateShells)
			{
				GD.Print("Shells generated");
				DestroyShells();
				GenerateShells();
				generateShells = false;
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
			shell.Position = new(0, shellHeight, 0);
			shellMat.SetShaderParameter("shell_index", i);
			shellMat.SetShaderParameter("shell_count", shellCount);
			shellMat.SetShaderParameter("height_threshold", height_threshold);

			shellHeight += 0.065f;
			height_threshold += 0.1f;
			GD.Print(height_threshold);
			shell.Owner = GetTree().EditedSceneRoot;
		}
		shellHeight = 0;
		height_threshold = 0;
	}

	private void DestroyShells()
	{
		for(int i = 0; i < GetChildCount(); i++) // destroys all children nodes
		{
			GetChild(i).QueueFree();
		}
	}
}