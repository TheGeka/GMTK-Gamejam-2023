using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GMTKGameJam2023.Scripts;
using GMTKGameJam2023.Scripts.Enums;
using GMTKGameJam2023.Units;

public partial class CameraControl : Camera3D
{
    [Export] private float cameraSpeed = 100.0f;

    [Export] private float zoomSpeed = 1000f;
    [Export] private float maxZoom = 2.0f;
    [Export] private float minZoom = 0.5f;

    private Vector2 _previousMousePosition;
    private List<Units> _selection = new();
    private Selector _selector;
    private Game _gameManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _selector = GetNode<Selector>("/root/Demo/Selector");
        _gameManager = GetNode<Game>("/root/Demo");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var cameraTranslation = new Vector3();
        if (Input.IsActionPressed(Controls.Up.ToString())) cameraTranslation.Z -= cameraSpeed * (float)delta;

        if (Input.IsActionPressed(Controls.Down.ToString())) cameraTranslation.Z += cameraSpeed * (float)delta;

        if (Input.IsActionPressed(Controls.Left.ToString())) cameraTranslation.X -= cameraSpeed * (float)delta;

        if (Input.IsActionPressed(Controls.Right.ToString())) cameraTranslation.X += cameraSpeed * (float)delta;

        //ZoomCamera(zoomAmount * zoomSpeed * delta);
        var mousePosition = GetViewport().GetMousePosition();
        if (Input.IsMouseButtonPressed(MouseButton.Middle))
        {
            var deltaMouse = mousePosition - _previousMousePosition;
            cameraTranslation.X = -deltaMouse.X * 0.1f;
            cameraTranslation.Z = -deltaMouse.Y * 0.1f;
            // RotateY(Mathf.DegToRad();
            // RotateX(Mathf.DegToRad();
        }

        _previousMousePosition = mousePosition;
        Position += cameraTranslation;

        if (Input.IsActionJustPressed("MainCommand"))
        {
            _selector.SelectionStartPosition = mousePosition;
            _selector.Visible = true;
        }

        if (Input.IsActionPressed("MainCommand"))
            _selector.MousePosition = mousePosition;
        else
            _selector.Visible = false;

        if (Input.IsActionJustReleased("MainCommand") && !GetTree().Paused) SelectUnits(mousePosition);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
            {
                var cameraTranslation = new Vector3();
                cameraTranslation.Z -= zoomSpeed * (float)GetProcessDeltaTime();
                Translate(cameraTranslation);
            }

            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
            {
                var cameraTranslation = new Vector3();
                cameraTranslation.Z += zoomSpeed * (float)GetProcessDeltaTime();
                Translate(cameraTranslation);
            }

            base._Input(@event);
        }
    }

    public void MoveUnits(Vector3 targetPos)
    {
        foreach (var select in _selection) select.MovementTarget = targetPos;
    }

    public Units GetUnitUnderMouse(Vector2 mousePosition)
    {
        var res = this.CastRay(GetWorld3D().DirectSpaceState, mousePosition);
        if (res.Count == 0) return null;

        if (res["collider"].Obj is not CharacterBody3D) return null;
        var unit = res["collider"].As<Units>();
        return unit.IsInGroup("Horde") ? unit : null;
    }

    public List<Units> GetUnitsInsideSelector(Vector2 topLeft, Vector2 bottomRight)
    {
        if (topLeft.X > bottomRight.X) (topLeft.X, bottomRight.X) = (bottomRight.X, topLeft.X);

        if (topLeft.Y > bottomRight.Y) (topLeft.Y, bottomRight.Y) = (bottomRight.Y, topLeft.Y);

        var box = new Rect2(topLeft, bottomRight - topLeft);

        var topLeftWorldPos = this.GetWorldCoordinates(GetWorld3D().DirectSpaceState, topLeft).Value;
        var bottomRightWorldPos = this.GetWorldCoordinates(GetWorld3D().DirectSpaceState, bottomRight).Value;

        var rightWorldPos = bottomRightWorldPos - topLeftWorldPos;
        rightWorldPos.Y = 1;
        var box3D = new Aabb(topLeftWorldPos, rightWorldPos);

        var units = GetNode<Node>("/root/Demo/UnitContainer").GetChildren();
        return units.Cast<Units>().Where(a => box3D.HasPoint(a.GlobalPosition)).ToList();
    }

    private void SelectUnits(Vector2 mousePositon)
    {
        if (mousePositon.DistanceSquaredTo(_selector.SelectionStartPosition) < 16)
        {
            // Select Single Unit if no unit under mouse && something selected move selected
            var unit = GetUnitUnderMouse(mousePositon);
            if (unit != null)
            {
                ClearSelection();
                _selection.Add(unit);
                unit.Selected = true;
            }
            else if (_selection.Count > 0)
            {
                MoveSelection(mousePositon);
            }
        }
        else
        {
            // Select everything inside selection box
            var selectedUnits = GetUnitsInsideSelector(mousePositon, _selector.SelectionStartPosition);
            foreach (var selectedUnit in selectedUnits) selectedUnit.Selected = true;
            ClearSelection();
            _selection.AddRange(selectedUnits);
        }
    }

    // private void SelectSingleUnit()
    // {
    //     var intersection = this.CastRay(GetWorld3D().DirectSpaceState, mouseButtonEvent.Position);
    //
    //     if (intersection != null && intersection.Count > 0)
    //     {
    //         var pos = intersection["position"].AsVector3();
    //         GD.Print($"Clicked on: X: {pos.X}, Y: {pos.Y}, Z: {pos.Z} ");
    //
    //         MoveUnits(pos);
    //     }
    // }

    private void ClearSelection()
    {
        foreach (var selected in _selection) selected.Selected = false;
        _selection.Clear();
    }

    private void MoveSelection(Vector2 mousePos)
    {
        var target = this.GetWorldCoordinates(GetWorld3D().DirectSpaceState, mousePos);
        if (target == null) return;
        foreach (var selected in _selection) selected.MovementTarget = target.Value;
    }
}