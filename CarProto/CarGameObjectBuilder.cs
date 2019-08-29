using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarProto.CustomComponents;
using GeonBit.Core;
using GeonBit.Core.Graphics.Materials;
using GeonBit.ECS;
using GeonBit.ECS.Components.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarProto
{
   
    public enum BodyTypes
    {
        SHOPPING_CART,
        STONE,
        WOOD,
        PAPER
    }
    public enum WheelTypes
    {
        SHOPPING_CART,
        STONE,
        WOOD,
        PAPER,
        PUMPKIN
    }
    class CarGameObjectBuilder
    {
        BodyTypes body;
        WheelTypes frontWheels;
        WheelTypes backWheels;

        List<String> bodies;
        List<String> wheels;

        Dictionary<BodyTypes, int> bodyWeights=new Dictionary<BodyTypes, int>();
        Dictionary<WheelTypes, int> wheelWeights = new Dictionary<WheelTypes, int>();
        public CarGameObjectBuilder()
        {
            initLists();
            body = (BodyTypes)1;
            frontWheels = (WheelTypes)4;
            backWheels = (WheelTypes)4;
        }

        public int getMaxWeight()
        {
            return bodyWeights[BodyTypes.STONE]+wheelWeights[WheelTypes.STONE]*2;
        }

        public int getMinWeight()
        {
            return bodyWeights [BodyTypes.PAPER]+wheelWeights [WheelTypes.PAPER]*2;
        }
        void initLists()
        {
            bodies = new List<string>();
            wheels = new List<string>();

            bodyWeights.Add(BodyTypes.SHOPPING_CART, 5);
            bodies.Add("Shopping Cart");

            bodyWeights.Add(BodyTypes.STONE, 8);
            bodies.Add("Moai");

            bodyWeights.Add(BodyTypes.WOOD, 4);
            bodies.Add("Wood");

            bodyWeights.Add(BodyTypes.PAPER, 2);
            bodies.Add("Paper");





            wheelWeights.Add(WheelTypes.SHOPPING_CART, 4);
            wheels.Add("Shopping Cart");

            wheelWeights.Add(WheelTypes.STONE, 6);
            wheels.Add("Stone");

            wheelWeights.Add(WheelTypes.WOOD, 2);
            wheels.Add("Wood");

            wheelWeights.Add(WheelTypes.PAPER, 1);
            wheels.Add("Paper");

            wheelWeights.Add(WheelTypes.PUMPKIN, 3);
            wheels.Add("Pumpkin");
        }
        public List<String> getBodyStrings()
        {           
            return bodies;
        }
        public List<String> getWheelStrings()
        {
            return wheels;
        }

       public void updateSelectedBody(int index)
        {
            body = (BodyTypes)index;
        }

       public void updateSelectedWheel(int index,bool front)
        {
            if(front)
            {
                frontWheels = (WheelTypes)index;
            }
            else
            {
                backWheels = (WheelTypes)index;
            }
        }

       public GameObject getCarGameObject()
        {
            GameObject carObject = new GameObject("player");
            GameObject bodyObject = new GameObject();
            GameObject frontWheelsObj = new GameObject();
            GameObject backWheelsObj = new GameObject();

            if (body == BodyTypes.STONE)
            {                
                Model carModel = ResourcesManager.Instance.GetModel("Models/Moai_Body");
                bodyObject.AddComponent(new ModelRenderer(carModel));
                bodyObject.SceneNode.Scale = new Vector3(2f, 2f, 2f);
                bodyObject.SceneNode.Rotation = new Vector3(Util.degToRad(270f), Util.degToRad(270f), Util.degToRad(0f));
                bodyObject.SceneNode.PositionY += 3f;
            }
            else if(body == BodyTypes.SHOPPING_CART)
            {                
                Model carModel = ResourcesManager.Instance.GetModel("Models/cart");
                bodyObject.AddComponent(new ModelRenderer(carModel));
            }
            else//default
            {
                Model carModel = ResourcesManager.Instance.GetModel("Models/MuscleCar");
                bodyObject.AddComponent(new ModelRenderer(carModel));
            }
         
            

            
            if (frontWheels == WheelTypes.PUMPKIN)
            {
                
                Model fWheelModel = ResourcesManager.Instance.GetModel("Models/Pumpkin");
                frontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                frontWheelsObj.Parent = carObject;
                frontWheelsObj.SceneNode.PositionX -= 2;
                frontWheelsObj.SceneNode.Scale = new Vector3(.5f, .5f, .5f);
            }
            else if (frontWheels == WheelTypes.STONE)
            {

                Model fWheelModel = ResourcesManager.Instance.GetModel("Models/stoneFront");
                frontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                frontWheelsObj.Parent = carObject;
                //frontWheelsObj.SceneNode.PositionX -= 2;
                // frontWheelsObj.SceneNode.Scale = new Vector3(.5f, .5f, .5f);
            }
            else
            {

                Model fWheelModel = ResourcesManager.Instance.GetModel("Models/stoneFront");
                frontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                frontWheelsObj.Parent = carObject;
            }



            if (backWheels == WheelTypes.PUMPKIN)
            {
                
                Model bWheelModel = ResourcesManager.Instance.GetModel("Models/Pumpkin");
                backWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                backWheelsObj.Parent = carObject;
                backWheelsObj.SceneNode.PositionX += 4;
                backWheelsObj.SceneNode.Scale = new Vector3(.5f, .5f, .5f);
            }
            else if (backWheels == WheelTypes.STONE)
            {

                Model bWheelModel = ResourcesManager.Instance.GetModel("Models/stoneFront");
                backWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                backWheelsObj.Parent = carObject;
                backWheelsObj.SceneNode.PositionX += 8;
            }
            else
            {
                Model bWheelModel = ResourcesManager.Instance.GetModel("Models/stoneFront");
                backWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                backWheelsObj.Parent = carObject;
                backWheelsObj.SceneNode.PositionX += 8;
            }



            //causes a crash
            //foreach (MaterialAPI material in carObject.GetComponent<ModelRenderer>().GetMaterials())
            //{
            //    material.DiffuseColor = Color.Red;
            //}


            bodyObject.Parent = carObject;
            frontWheelsObj.Parent = carObject;
            backWheelsObj.Parent = carObject;            

            return carObject;
        }

        public float getCarWeight()
        {
            return bodyWeights[body] + wheelWeights[frontWheels] + wheelWeights[backWheels];
        }
    }

}
