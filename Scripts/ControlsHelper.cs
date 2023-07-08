using Godot;

namespace GMTKGameJam2023.Scripts
{
    public static class ControlsHelper
    {
        enum Controls
        {
            PauseGame
        }

        public static bool IsPauseEvent(this InputEvent inputEvent)
        {
            if (inputEvent.IsAction(Controls.PauseGame.ToString()) && inputEvent.IsActionReleased(Controls.PauseGame.ToString(), true))
            {
                return true;
            }
            return false;
        }
    }
}