using GeonBit;
using GeonBit.ECS.Components;
using GeonBit.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CarProto.Custom_Components
{
    /// <summary>
    /// A component to move the sphere using the game controls.
    /// </summary>
    class PlayerController : BaseComponent
    {
        private readonly float movingSpeed = 20f;

        /// <summary>
        /// Clone this component.
        /// </summary>
        /// <returns>Cloned PlayerController instance.</returns>
        public override BaseComponent Clone()
        {
            return new PlayerController();
        }

        /// <summary>
        /// Do on-frame based update.
        /// </summary>
        protected override void OnUpdate()
        {
            // Move up
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Forward))
            {
                _GameObject.SceneNode.PositionY += Managers.TimeManager.TimeFactor * movingSpeed;
            }
            // Move down
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Backward))
            {
                _GameObject.SceneNode.PositionY -= Managers.TimeManager.TimeFactor * movingSpeed;
            }
            // Move left
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Left))
            {
                _GameObject.SceneNode.PositionX -= Managers.TimeManager.TimeFactor * movingSpeed;
            }
            // Move right
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Right))
            {
                _GameObject.SceneNode.PositionX += Managers.TimeManager.TimeFactor * movingSpeed;
            }
        }
    }
}
