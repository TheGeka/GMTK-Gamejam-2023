using Godot;
using System;
using GMTKGameJam2023.Scripts.Enums;

public partial class CameraControl : Camera3D
{
    [Export] private float cameraSpeed = 100.0f;

    [Export] private float zoomSpeed = 1000f;
    [Export] private float maxZoom = 2.0f;
    [Export] private float minZoom = 0.5f;

    private Vector2 _previousMousePosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var cameraTranslation = new Vector3();
        if (Input.IsActionPressed(Controls.Up.ToString()))
        {
            cameraTranslation.Y += cameraSpeed * (float)delta;
        }

        if (Input.IsActionPressed(Controls.Down.ToString()))
        {
            cameraTranslation.Y -= cameraSpeed * (float)delta;
        }

        if (Input.IsActionPressed(Controls.Left.ToString()))
        {
            cameraTranslation.X -= cameraSpeed * (float)delta;
        }

        if (Input.IsActionPressed(Controls.Right.ToString()))
        {
            cameraTranslation.X += cameraSpeed * (float)delta;
        }
        
        //ZoomCamera(zoomAmount * zoomSpeed * delta);
        Vector2 mousePosition = GetViewport().GetMousePosition();
        if (Input.IsMouseButtonPressed(MouseButton.Middle))
        {
            Vector2 deltaMouse = mousePosition - _previousMousePosition;
            cameraTranslation.X = -deltaMouse.X * 0.1f;
            cameraTranslation.Y = deltaMouse.Y * 0.1f;
            // RotateY(Mathf.DegToRad();
            // RotateX(Mathf.DegToRad();
        }
        _previousMousePosition = mousePosition;
        Translate(cameraTranslation);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
            {
                var cameraTranslation = new Vector3();
                cameraTranslation.Z += zoomSpeed * (float)GetProcessDeltaTime();
                Translate(cameraTranslation);
            }

            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
            {
                var cameraTranslation = new Vector3();
                cameraTranslation.Z -= zoomSpeed * (float)GetProcessDeltaTime();
                Translate(cameraTranslation);
            }
            base._Input(@event);
        }
    }
}