using GeonBit;
using GeonBit.ECS;
using GeonBit.ECS.Components;
using GeonBit.ECS.Components.Physics;
using GeonBit.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CarProto.CustomComponents
{
    class ObstacleCollision : BaseComponent
    {
        GameObject player;

        public ObstacleCollision(GameObject p)
        {
            player = p;
        }

        public override BaseComponent Clone()
        {
            return this;
        }

        protected override void OnUpdate()
        {
            if (player.SceneNode.PositionX - _GameObject.SceneNode.PositionX < 7.5 &&
                player.SceneNode.PositionX - _GameObject.SceneNode.PositionX > -7.5 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY < 10 &&
                player.SceneNode.PositionY - _GameObject.SceneNode.PositionY > -10)
            {
                player.GetComponent<PlayerController>().addDamage(10);
                _GameObject.Destroy();              
            }
        }
    }
}
