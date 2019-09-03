using CarProto.CustomGameObjects;
using GeonBit.ECS;
using GeonBit.ECS.Components;
using Microsoft.Xna.Framework;
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
        public float turningSpeed { get; set; }
        public float knockBackSpeed = 20f;
        public float knockBackDistance = 1.7f;
        public float damage { get; set; }
        public float weight { get; set; }
        public float carPartDamageReduction=1f;

        public bool freeControl = false;

        public bool dead = false;

        public int maxWeight;
        public int minWeight;

        private bool knocked;
        private bool knockedDirLeft;
        private float distanceCounter;

        private float damageReductionFactor = 0.5f;
        private float currentDamageReductionFactor = 1;

        float zOffset = .5f;
        float carBounceSpeed = 0f;
        private int offsetDir = 1;
        private float maxZoffset;

        private CarGameObjectBuilder carState;

        private GameObject[] wheels; 
        private enum wheel
        {
            FRONT_LEFT,
            FRONT_RIGHT,
            BACK_LEFT,
            BACK_RIGHT
        }
        public PlayerController(CarGameObjectBuilder carState)
        {
           // weight = Util.randomBetween(minWeight, maxWeight);
            maxZoffset = zOffset;
            this.carState = carState;
                  
        }

        protected override void OnAddToScene()
        {
            init();
        }
        
        void init()
        {
            minWeight = carState.getMinWeight();
            maxWeight =carState.getMaxWeight();

            weight = carState.getCarWeight();
            turningSpeed = carState.getCarTurnSpeed();
            carPartDamageReduction = carState.getCarDamageReduction();

            wheels = new GameObject[4];

            wheels[(int)wheel.FRONT_LEFT] = _GameObject.Find("lfWheel",true);
            wheels[(int)wheel.FRONT_RIGHT] = _GameObject.Find("rfWheel", true);
            wheels[(int)wheel.BACK_LEFT] = _GameObject.Find("lbWheel", true);
            wheels[(int)wheel.BACK_RIGHT] = _GameObject.Find("rbWheel", true);            
        }
        /// <summary>
        /// Clone this component.
        /// </summary>
        /// <returns>Cloned PlayerController instance.</returns>
        public override BaseComponent Clone()
        {
            return new PlayerController(carState);
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
                _GameObject.SceneNode.PositionY += Managers.TimeManager.TimeFactor * movingSpeed;// * (Math.Max(damageShift() / 1.5f, 1
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
                    currentDamageReductionFactor = 1;
                }
                return;
            }
            //shift up and down

            _GameObject.SceneNode.PositionZ += Managers.TimeManager.TimeFactor * carBounceSpeed * offsetDir;
            if (_GameObject.SceneNode.PositionZ <= 0)
            {
                zOffset = maxZoffset;
                offsetDir = 1;
            }
            else if (_GameObject.SceneNode.PositionZ >= zOffset)
            {
                zOffset = -maxZoffset;
                offsetDir = -1;
            }

            // Move left
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Left))
            {
                _GameObject.SceneNode.PositionX -= Managers.TimeManager.TimeFactor * (turningSpeed * damageShift());
                addRotation(false);
            }
            else if (Managers.GameInput.IsKeyReleased(GeonBit.Input.GameKeys.Left))
            {
                addRotation(true);
            }
            // Move right
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Right))
            {
                _GameObject.SceneNode.PositionX += Managers.TimeManager.TimeFactor * (turningSpeed * damageShift());
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

            updateSpeed((CarObject)_GameObject);
        }

        /// <summary>
        /// Slowly increase speed and make wheels rotate more as speed increases
        /// </summary>
        void updateSpeed(CarObject carObject)
        {
            movingSpeed += weight / 350;

            foreach (GameObject wheelObject in carObject.getWheelObjects())
            {
                RotatingObject rotatingObject = wheelObject.GetComponent<RotatingObject>();
                rotatingObject.UpdateSpeed(0f, 0f, movingSpeed / 10f);
            }
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

            damage = (int)((damage * carPartDamageReduction) * currentDamageReductionFactor);
            currentDamageReductionFactor *= damageReductionFactor;

            this.damage += damage;
            if (this.damage >= 100)
            {
                dead = true;
                GameObject charObj = _GameObject.Find("Character");
                charObj.AddComponent(new RotatingObject(Util.randomBetween(0, 10), Util.randomBetween(0, 10), Util.randomBetween(0, 10)));
                charObj.AddComponent(new LinearMovingObject(33, new Vector3(Util.randomBetween(0, 10)/10f, Util.randomBetween(0, 10) / 10f, Util.randomBetween(0, 10) / 10f)));
            }

            if(this.damage>=90)
            {
                detachObjects(wheel.FRONT_LEFT);
            }
            else if(this.damage>=75)
            {
                carBounceSpeed = 2.5f;
                detachObjects(wheel.BACK_RIGHT);
            }
            else if(this.damage>=50)
            {
                carBounceSpeed = 1.5f;
                detachObjects(wheel.FRONT_RIGHT);

            }
            else if(this.damage>=20)
            {
                detachObjects(wheel.BACK_LEFT);
            }
        }

        public void AddFakeForce(bool left)
        {
            knockedDirLeft = left;
            knocked = true;
        }

        void detachObjects(wheel wheel)
        {
            if(wheels[(int)wheel]!=null)
            {
                //wheels[(int)wheel].SceneNode.PositionZ += 15;
                GameObject holder = new GameObject();
                holder.SceneNode.Position = _GameObject.SceneNode.Position;
                holder.Parent = _GameObject.ActiveScene.Root;
                wheels[(int)wheel].Parent = holder;
                wheels[(int)wheel] = null;
            }
        }
    }
}
