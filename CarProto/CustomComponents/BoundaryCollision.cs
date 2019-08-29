using System;
using System.Collections.Generic;
using GeonBit.ECS;
using GeonBit.ECS.Components;
using CarProto.CustomComponents;
using Microsoft.Xna.Framework;
using GeonBit.ECS.Components.Physics;

namespace CarProto.CustomComponents
{
    class BoundaryCollision : BaseComponent
    {
        GameObject player;
        int damage;
        Vector3 from;
        Vector3 to;
        float xDiff;
        float yDiff;
        float slope;
        PlayerController playerController;

        public BoundaryCollision(GameObject player, Vector3 from, Vector3 to, int damage)
        {
            if (from.Y > to.Y)
            {
                this.from = to;
                this.to = from;
            }
            else
            {
                this.from = from;
                this.to = to;
            }

            this.player = player;

            this.damage = damage;
            xDiff = this.to.X - this.from.X;
            yDiff = this.to.Y - this.from.Y;
            playerController = this.player.GetComponent<PlayerController>();

            if (yDiff == 0)
                slope = -1;
            else
                slope = xDiff / yDiff;
        }

        public override BaseComponent Clone()
        {
            return new BoundaryCollision(player, from, to, damage);
        }

        protected override void OnUpdate()
        {
            if (player.SceneNode.PositionY < from.Y || player.SceneNode.PositionY > to.Y)
            {
                return;
            }

            float currentX;
            if (slope == -1)
                currentX = from.X;
            else
                currentX = from.X + (player.SceneNode.PositionY - from.Y) * slope ;

            if (player.SceneNode.PositionX - currentX < 4 &&
                player.SceneNode.PositionX - currentX > -4)
            {
                playerController.addDamage(damage);
                if (player.SceneNode.PositionX > currentX)
                {
                    playerController.AddFakeForce(false);
                }
                else
                {
                    playerController.AddFakeForce(true);
                }
            }
        }
    }
}
