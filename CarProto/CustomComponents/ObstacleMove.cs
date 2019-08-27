using GeonBit.ECS;
using GeonBit.ECS.Components;

namespace CarProto.CustomComponents
{
    class ObstacleMove : BaseComponent
    {
        private float travelSpeed;
        private float travelDistance;

        private float distanceCounter;
        bool dirIsLeft;

        public ObstacleMove(float travelSpeed = 5, float travelDistance = 8)
        {
            this.travelSpeed = travelSpeed;
            this.travelDistance = travelDistance;
        }

        public override BaseComponent Clone()
        {
            return new ObstacleMove();
        }

        protected override void OnUpdate()
        {
            float disPerFrame = Managers.TimeManager.TimeFactor * travelSpeed;

            if (dirIsLeft)
            {
                _GameObject.SceneNode.PositionX += disPerFrame;
                distanceCounter += disPerFrame;

                if (distanceCounter > travelDistance / 2)
                {
                    dirIsLeft = false;
                }
            }
            else
            {
                _GameObject.SceneNode.PositionX -= disPerFrame;
                distanceCounter -= disPerFrame;

                if (distanceCounter < -travelDistance / 2)
                {
                    dirIsLeft = true;
                }
            }
        }
    }
}
