using Godot;

namespace GMTKGameJam2023
{
	public partial class PauseMenu : Control
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var selectGrunt = GetNode<Button>("Grunt");
			selectGrunt.Pressed += () =>
			{
				GD.Print("Grunt Selected");
			};
			var SelectUnit2 = GetNode<Button>("Unit2");
			SelectUnit2.Pressed += () =>
			{
				GD.Print("Unit2 Selected");
			};
			var unpause = GetNode<Button>("Unpause");
			unpause.Pressed += OnPauseButtonPressed;
		}

		private void OnPauseButtonPressed()
		{
			Hide();
			GetTree().Paused = false;
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	
	}
}