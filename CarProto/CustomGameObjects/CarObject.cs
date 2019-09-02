using GeonBit.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarProto.CustomGameObjects
{
    class CarObject : GameObject
    {
        public CarObject(string name) : base(name)
        {

        }

        public List<GameObject> getWheelObjects()
        {
            string name = "Wheel";
            // search for wheel objects in all children game objects
            // example being lfWheel, rfWheel
            return this._children.FindAll(FindNamedObjects(name));
        }

        /// <summary>
        /// Search for a name being in a selction of gameobjects
        /// </summary>
        static Predicate<GameObject> FindNamedObjects(string name)
        {
            return delegate (GameObject gameObject)
            {
                return gameObject.Name.Contains(name);
            };
        }
    }
}
