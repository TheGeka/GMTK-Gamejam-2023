using GMTKGameJam2023.Scripts;
using GMTKGameJam2023.Scripts.Enums;
using Godot;

namespace GMTKGameJam2023
{
	public partial class PauseMenu : CanvasLayer
	{
		private Game _gameManager;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var selectGrunt = GetNode<Button>("FlowContainer/Grunt");
			_gameManager = GetNode<Game>("/root/Demo");
			selectGrunt.Pressed += () =>
			{
				GD.Print("Grunt Selected");
				_gameManager._SelectedUnit = SelectableUnits.Grunt;

			};
			var SelectUnit2 = GetNode<Button>("FlowContainer/Unit2");
			SelectUnit2.Pressed += () =>
			{
				GD.Print("Unit2 Selected");
				_gameManager._SelectedUnit = SelectableUnits.Unit2;
			};
			var unpause = GetNode<Button>("FlowContainer/Unpause");
			unpause.Pressed += OnPauseButtonPressed;
		}

		private void OnPauseButtonPressed()
		{
			Hide();
			
			_gameManager.TurnTimer.Start();
			GetTree().Paused = false;
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public override void _Input(InputEvent @event)
		{
			if (@event.IsControl(Controls.PauseGame) && Visible)
			{
				OnPauseButtonPressed();
				@event.Dispose();
			}

			
		}
	}
}