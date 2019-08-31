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
            return this._children.FindAll(FindNamedObjects(name));
        }

        static Predicate<GameObject> FindNamedObjects(string name)
        {
            return delegate (GameObject gameObject)
            {
                return name.Equals(gameObject.Name);
            };
        }
    }
}
