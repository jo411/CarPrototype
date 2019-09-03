using GeonBit.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto.CustomComponents
{
    class LinearMovingObject : BaseComponent
    {
        float speed;
        Vector3 direction;
        public LinearMovingObject(float speed, Vector3 direction)
        {
            this.speed = speed;
            this.direction = direction;
        }        
        public override BaseComponent Clone()
        {
            return new LinearMovingObject(speed,direction);
        }

        protected override void OnUpdate()
        {
            _GameObject.SceneNode.Position += (Managers.TimeManager.TimeFactor * speed)*direction;
        }
    }
}
