using GeonBit.ECS;
using GeonBit.ECS.Components;
using Microsoft.Xna.Framework;

namespace CarProto.CustomComponents
{
    class CameraFollow : BaseComponent
    {
        private GameObject target;
        public Vector3 offset { get; set; }
        public float dampingStrength { get; set; }
        protected override void OnAddToScene()
        {
            target = _GameObject.ParentScene.Root.Find("player");
        }

        public override BaseComponent Clone()
        {
            return new CameraFollow();
        }

        /// <summary>
        /// Do on-frame based update.
        /// </summary>
        protected override void OnUpdate()
        {
            _GameObject.SceneNode.Position = Vector3.Lerp(_GameObject.SceneNode.Position, target.SceneNode.Position + offset, dampingStrength);
        }
    }
}
