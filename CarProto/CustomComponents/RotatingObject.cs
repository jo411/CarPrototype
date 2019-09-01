using GeonBit.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto.CustomComponents
{
    class RotatingObject : BaseComponent
    {
        float speedX;
        float speedY;
        float speedZ;

        public RotatingObject(float speedX, float speedY, float speedZ)
        {
            this.speedX = speedX;
            this.speedY = speedY;
            this.speedZ = speedZ;
        }

        protected override void OnUpdate()
        {
            _GameObject.SceneNode.RotationX += speedX * Managers.TimeManager.TimeFactor;
            _GameObject.SceneNode.RotationY += speedY * Managers.TimeManager.TimeFactor;
            _GameObject.SceneNode.RotationZ += speedZ * Managers.TimeManager.TimeFactor;
        }

        public override BaseComponent Clone()
        {
            return new RotatingObject(speedX, speedY,speedZ);
        }
    }
}
