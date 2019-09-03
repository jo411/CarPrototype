using System;
using System.Collections.Generic;
using GeonBit.ECS;
using GeonBit.ECS.Components;
using CarProto.CustomComponents;
using Microsoft.Xna.Framework;
using GeonBit.ECS.Components.Physics;

namespace CarProto.CustomComponents
{
    class PondCollision : BaseComponent
    {
        GameObject player;
        Vector3 from;
        Vector3 to;
        float xDiff;
        float yDiff;
        float slope;
        PlayerController playerController;
        bool dir;

        public PondCollision(GameObject player, Vector3 from, Vector3 to)
        {
            Random rand = new Random();
            if (rand.Next(0, 2) == 0)
                dir = true;
            else
                dir = false;

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
            return new PondCollision(player, from, to);
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
                currentX = from.X + (player.SceneNode.PositionY - from.Y) * slope;

            if (player.SceneNode.PositionX - currentX < 10 &&
                player.SceneNode.PositionX - currentX > -10)
            {
                playerController.RandomShift(dir);
            }
        }
    }
}
