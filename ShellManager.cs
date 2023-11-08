using Godot;

[Tool]
public partial class ShellManager : Node3D
{
	[Export]
	public int shellCount;
	[Export]
	public bool generateShells = false, destroyShells = false;

	private int shellIndex = 0;
	private float shellHeight = 0;
	private PackedScene shell_Scene;

	public override void _Ready()
	{
		shell_Scene = (PackedScene)GD.Load("res://Shell.tscn");

		if(!Engine.IsEditorHint())
			GenerateShells();
	}
    public override void _Process(double delta)
    {
		if(Engine.IsEditorHint())
		{
			if(generateShells)
			{
				GenerateShells();
				generateShells = false;
			}

			if(destroyShells)
			{
				DestroyShells();
				destroyShells = false;
			}
		}
    }

    void GenerateShells()
	{
		for(int i = 0; i < shellCount; i++)
		{
			MeshInstance3D shell = (MeshInstance3D)shell_Scene.Instantiate();
			AddChild(shell);
			shell.Position = new(0, shellHeight, 0);
			shellHeight += 0.02f;
		}
		shellHeight = 0;
	}

	private void DestroyShells()
	{
		for(int i = 0; i < GetChildCount(); i++)
		{
			GetChild(i).QueueFree();
		}
	}
}