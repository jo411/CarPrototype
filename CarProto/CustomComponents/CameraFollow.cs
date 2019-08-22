using GeonBit.ECS;
using GeonBit.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto.CustomComponents
{
    class CameraFollow : BaseComponent
    {
        GameObject target;
        Vector3 offset = new Vector3(0, 0, 0);
       public float dampingStrength {get;set;}
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
            _GameObject.SceneNode.Position =Vector3.Lerp(_GameObject.SceneNode.Position, target.SceneNode.Position + offset,dampingStrength);
            
       }

        public void setOffset(Vector3 offset)
        {
            this.offset = offset;
        }
    }
}
