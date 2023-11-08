using Godot;

public partial class ShellManager : Node3D
{
	[Export]
	public int shellCount;

	private int shellIndex = 0;
	private float shellHeight = 0;
	private PackedScene shell_Scene;

	public override void _Ready()
	{
		shell_Scene = (PackedScene)GD.Load("res://Shell.tscn");

		for(int i = 0; i < shellCount; i++)
		{
			MeshInstance3D shell = (MeshInstance3D)shell_Scene.Instantiate();
			AddChild(shell);
			shell.Position = new(0, shellHeight, 0);
			shellHeight += 0.02f;
		}
	}
}