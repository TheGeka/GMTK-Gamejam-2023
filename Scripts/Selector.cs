using Godot;
using System;
using System.Numerics;
using GMTKGameJam2023.Scripts;
using Vector2 = Godot.Vector2;

public partial class Selector : Control
{
	private bool _isVisible = false;

	private Vector2 _mousePosition;

	private Vector2 _selectionStartPosition;

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
		if (_isVisible && _mousePosition != _selectionStartPosition)
		{
			DrawLine(_selectionStartPosition, new Vector2(_mousePosition.X, _selectionStartPosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
			DrawLine(_selectionStartPosition, new Vector2(_selectionStartPosition.X, _mousePosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
			DrawLine(_mousePosition, new Vector2(_mousePosition.X, _selectionStartPosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
			DrawLine(_mousePosition, new Vector2(_selectionStartPosition.X, _mousePosition.Y), _selectionBoxColor, _selectionBoxLineWidth);
		}
	}
}
