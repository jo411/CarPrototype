using GeonBit.ECS.Components;
using System;

namespace CarProto.CustomComponents
{
    /// <summary>
    /// A component to move the sphere using the game controls.
    /// </summary>
    class PlayerController : BaseComponent
    {

        private float turningAngle = 25f;
        private int turnState = 1; //0 Left | 1 Straight | 2 Right

        public float movingSpeed { get; private set; } = 20f;
        public float knockBackSpeed = 20f;
        public float knockBackDistance = 5f;
        public float damage { get; set; }
        public float weight { get; set; }

        public bool freeControl = false;

        public bool dead = false;

        private int maxWeight = 20;
        private int minWeight = 8;

        private bool knocked;
        private bool knockedDirLeft;
        private float distanceCounter;

        public PlayerController()
        {
            weight = Util.randomBetween(minWeight, maxWeight);
        }

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
            if (dead)
            {
                return;
            }

            if (freeControl)
            {
                // Move up
                if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Forward))
                {
                    _GameObject.SceneNode.PositionY += Managers.TimeManager.TimeFactor * (movingSpeed * damageShift());
                }
                // Move down
                if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Backward))
                {
                    _GameObject.SceneNode.PositionY -= Managers.TimeManager.TimeFactor * movingSpeed;
                }
            }
            //AutoForward
            else
            {
                _GameObject.SceneNode.PositionY += Managers.TimeManager.TimeFactor * movingSpeed * (Math.Max(damageShift() / 1.5f, 1));
            }

            if (knocked)
            {
                float frameDis = Managers.TimeManager.TimeFactor * (knockBackSpeed * damageShift());
                if (knockedDirLeft)
                {
                    _GameObject.SceneNode.PositionX -= frameDis;
                    
                }
                else
                {
                    _GameObject.SceneNode.PositionX += frameDis;
                }
                distanceCounter += frameDis;

                if (distanceCounter > knockBackDistance)
                {
                    knocked = false;
                    distanceCounter = 0;
                    resetRotation();
                }
                return;
            }

            // Move left
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Left))
            {
                _GameObject.SceneNode.PositionX -= Managers.TimeManager.TimeFactor * (movingSpeed * damageShift());
                addRotation(false);
            }
            else if (Managers.GameInput.IsKeyReleased(GeonBit.Input.GameKeys.Left))
            {
                addRotation(true);
            }
            // Move right
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Right))
            {
                _GameObject.SceneNode.PositionX += Managers.TimeManager.TimeFactor * (movingSpeed * damageShift());
                addRotation(true);
            }
            else if (Managers.GameInput.IsKeyReleased(GeonBit.Input.GameKeys.Right))
            {
                addRotation(false);
            }

            if (Managers.GameInput.IsKeyPressed(GeonBit.Input.GameKeys.Jump))
            {
                addDamage(10);

            }

            updateSpeed();
        }

        /// <summary>
        /// Slowly increase speed
        /// </summary>
        void updateSpeed()
        {
            movingSpeed += weight / 350;
        }

        /// <summary>
        /// TEMP, playing with rotating the car as it moves 
        /// </summary>
        void addRotation(bool right)
        {
            if (turnState == 1 && right)//Straight turning right
            {
                _GameObject.SceneNode.RotationX -= Util.degToRad(turningAngle);
                turnState = 2;
            }
            else if (turnState == 1 && !right) // Straight turning left
            {
                _GameObject.SceneNode.RotationX += Util.degToRad(turningAngle);
                turnState = 0;
            }
            else if (turnState == 2 && !right) //Right turning left
            {
                _GameObject.SceneNode.RotationX += Util.degToRad(turningAngle);
                turnState = 1;
            }
            else if (turnState == 0 && right)//Left turning right
            {
                _GameObject.SceneNode.RotationX -= Util.degToRad(turningAngle);
                turnState = 1;
            }
        }

        /// <summary>
        /// Reset the rotation of the car
        /// </summary>
        void resetRotation()
        {
            turnState = 1;
            _GameObject.SceneNode.RotationX = 0;
        }

        /// <summary>
        /// Returns a multiplier to movespeed based on damage thresholds
        /// </summary>
        /// <returns></returns>
        float damageShift()
        {
            float[] shift = { 1, 1.4f, 1.8f, 2f, 2.2f };
            bool malfunction = Util.randomBetween(0, 100) <= 40;
            if (!malfunction) { return 1; }

            if (damage <= 0)
            {
                return shift[0];
            }
            else if (damage <= 12)
            {
                return shift[1];
            }
            else if (damage <= 50)
            {
                return shift[2];
            }
            else if (damage <= 75)
            {
                return shift[3];
            }
            else
            {
                return shift[4];
            }
        }

        public void addDamage(int damage)
        {
            float shift = (-1 * ((weight - minWeight) - maxWeight)) / maxWeight;//(-1*((X-8)-20))/20

            damage = (int)(damage * shift);
            this.damage += damage;
            if (this.damage >= 100)
            {
                dead = true;
            }
        }

        public void AddFakeForce(bool left)
        {
            knockedDirLeft = left;
            knocked = true;
        }
    }
}
