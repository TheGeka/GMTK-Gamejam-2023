using Godot;
using System;
using System.Numerics;
using GMTKGameJam2023.Scripts;
using Vector2 = Godot.Vector2;

public partial class Selector : Control
{
	public Vector2 MousePosition;

	public Vector2 SelectionStartPosition;

	private readonly Color _selectionBoxColor = Colors.Blue;

	private readonly int _selectionBoxLineWidth = 3;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (Visible && MousePosition != SelectionStartPosition)
		{
			DrawLine(SelectionStartPosition, new Vector2(MousePosition.X, SelectionStartPosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
			DrawLine(SelectionStartPosition, new Vector2(SelectionStartPosition.X, MousePosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
			DrawLine(MousePosition, new Vector2(MousePosition.X, SelectionStartPosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
			DrawLine(MousePosition, new Vector2(SelectionStartPosition.X, MousePosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
		}
	}
}
