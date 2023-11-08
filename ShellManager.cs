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
	}
    public override void _Process(double delta)
    {
		if(Engine.IsEditorHint()) // if in editor uses bools as buttons to instanciate and destroy shells
		{
			// if you press the button to generate and there are no shells, generates them
			if(generateShells && !alreadyGenerated)
			{
				GD.Print("Shells generated");
				GenerateShells();
				alreadyGenerated = true;
				generateShells = false;
			}
			else
				generateShells = false;

			// if you press to destroy and there are shells, destroy them
			if(destroyShells && alreadyGenerated)
			{
				GD.Print("Shells removed");
				DestroyShells();
				alreadyGenerated = false;
				destroyShells = false;
			}
			else
				destroyShells = false;
		}
    }

    void GenerateShells()
	{
		for(int i = 0; i < shellCount; i++)
		{
			MeshInstance3D shell = (MeshInstance3D)shell_Scene.Instantiate();
			AddChild(shell);
			shell.Position = new(0, shellHeight, 0);
			shellHeight += 0.065f;
			shell.Owner = GetTree().EditedSceneRoot;
		}
		shellHeight = 0;
	}

	private void DestroyShells()
	{
		for(int i = 0; i < GetChildCount(); i++) // destroys all children nodes
		{
			GetChild(i).QueueFree();
		}
	}
}