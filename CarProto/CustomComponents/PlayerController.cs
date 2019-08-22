using GeonBit;
using GeonBit.ECS.Components;
using GeonBit.Managers;
using Microsoft.Xna.Framework;
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
        private float turningAngle = 15f;
        private int turnState = 1; //0 Left | 1 Straight | 2 Right
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
                addRotation(false);
            }
            else if(Managers.GameInput.IsKeyReleased(GeonBit.Input.GameKeys.Left))
            {
                addRotation(true);
            }
            // Move right
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Right))
            {
                _GameObject.SceneNode.PositionX += Managers.TimeManager.TimeFactor * movingSpeed;
                addRotation(true);
            }
            else if (Managers.GameInput.IsKeyReleased(GeonBit.Input.GameKeys.Right))
            {
                addRotation(false);
            }

           
        }

        /// <summary>
        /// TEMP, playing with rotating the car as it moves 
        /// </summary>
        void addRotation(bool right)
        {
            if (turnState == 1 && right)//Straight turning right
            {
                _GameObject.SceneNode.RotationX -= util.degToRad(turningAngle);
                turnState = 2;
            }
            else if (turnState == 1 && !right) // Straight turning left
            {
                _GameObject.SceneNode.RotationX += util.degToRad(turningAngle);
                turnState = 0;
            }
            else if (turnState == 2 && !right) //Right turning left
            {
                _GameObject.SceneNode.RotationX += util.degToRad(turningAngle);
                turnState = 1;
            }
            else if (turnState==0 && right)//Left turning right
            {
                _GameObject.SceneNode.RotationX -= util.degToRad(turningAngle);
                turnState = 1;
            }

           
        }
    }
}
