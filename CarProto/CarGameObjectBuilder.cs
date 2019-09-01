using CarProto.CustomComponents;
using CarProto.CustomGameObjects;
using GeonBit.Core;
using GeonBit.ECS;
using GeonBit.ECS.Components.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
       public BodyTypes body{get; private set;}
       public WheelTypes frontWheels { get; private set; }
       public WheelTypes backWheels { get; private set; }

        List<String> bodies;
        List<String> wheels;

        Dictionary<BodyTypes, int> bodyWeights = new Dictionary<BodyTypes, int>();
        Dictionary<BodyTypes, float> bodyDR = new Dictionary<BodyTypes, float>();
        Dictionary<WheelTypes, int> wheelWeights = new Dictionary<WheelTypes, int>();
        Dictionary<WheelTypes, float> wheelTurnSpeeds= new Dictionary<WheelTypes, float>();
        Dictionary<WheelTypes, float> wheelDR = new Dictionary<WheelTypes, float>();

        public CarGameObjectBuilder()
        {
            initLists();
            buildRandomCar();

            //body = BodyTypes.STONE;
            //frontWheels = WheelTypes.PUMPKIN;
            //backWheels = WheelTypes.PUMPKIN;
        }

        public void buildRandomCar()
        {           
            Array bodyValues = Enum.GetValues(typeof(BodyTypes));
            Array wheelValues = Enum.GetValues(typeof(WheelTypes));
            body = (BodyTypes)bodyValues.GetValue(Util.randomBetween(0,bodyValues.Length));
            frontWheels = (WheelTypes)wheelValues.GetValue(Util.randomBetween(0, wheelValues.Length));
            backWheels = (WheelTypes)wheelValues.GetValue(Util.randomBetween(0, wheelValues.Length));
        }

        public int getMaxWeight()
        {
            return bodyWeights[BodyTypes.STONE] + wheelWeights[WheelTypes.STONE] * 2;
        }

        public int getMinWeight()
        {
            return bodyWeights[BodyTypes.PAPER] + wheelWeights[WheelTypes.PAPER] * 2;
        }

        public float getMinTurningSpeed()
        {
            return wheelTurnSpeeds[WheelTypes.STONE];
        }
        public float getMaxTurningSpeed()
        {
            return wheelTurnSpeeds[WheelTypes.PAPER];
        }

        public float getMaxDamageReduction()
        {
            return (bodyDR[BodyTypes.STONE] + wheelDR[WheelTypes.STONE] + wheelDR[WheelTypes.STONE])/3;
        }
        public float getMinDamageReduction()
        {
            return (bodyDR[BodyTypes.PAPER] + wheelDR[WheelTypes.PAPER] + wheelDR[WheelTypes.PAPER])/3;
        }
        void initLists()
        {
            bodies = new List<string>();
            wheels = new List<string>();

            bodyWeights.Add(BodyTypes.SHOPPING_CART, 5);
            bodyDR.Add(BodyTypes.SHOPPING_CART, .89f);
            bodies.Add("Shopping Cart");

            bodyWeights.Add(BodyTypes.STONE, 8);
            bodyDR.Add(BodyTypes.STONE, .85f);
            bodies.Add("Stone");

            bodyWeights.Add(BodyTypes.WOOD, 4);
            bodyDR.Add(BodyTypes.WOOD, .98f);
            bodies.Add("Wood(not implemented)");

            bodyWeights.Add(BodyTypes.PAPER, 2);
            bodyDR.Add(BodyTypes.PAPER, 1.15f);
            bodies.Add("Toilet Paper");


            wheelWeights.Add(WheelTypes.SHOPPING_CART, 4);
            wheelTurnSpeeds.Add(WheelTypes.SHOPPING_CART, 28);
            wheelDR.Add(WheelTypes.SHOPPING_CART, .98f);
            wheels.Add("Shopping Cart(not implemented)");

            wheelWeights.Add(WheelTypes.STONE, 6);
            wheelTurnSpeeds.Add(WheelTypes.STONE, 14);
            wheelDR.Add(WheelTypes.STONE, .96f);
            wheels.Add("Stone");

            wheelWeights.Add(WheelTypes.WOOD, 2);
            wheelTurnSpeeds.Add(WheelTypes.WOOD, 24);
            wheelDR.Add(WheelTypes.WOOD, .99f);
            wheels.Add("Wood(not implemented)");

            wheelWeights.Add(WheelTypes.PAPER, 1);
            wheelTurnSpeeds.Add(WheelTypes.PAPER, 32);
            wheelDR.Add(WheelTypes.PAPER, 1.1f);
            wheels.Add("Toilet Paper");

            wheelWeights.Add(WheelTypes.PUMPKIN, 3);
            wheelTurnSpeeds.Add(WheelTypes.PUMPKIN, 24);
            wheelDR.Add(WheelTypes.PUMPKIN, 1f);
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

        public void updateSelectedWheel(int index, bool front)
        {
            if (front)
            {
                frontWheels = (WheelTypes)index;
            }
            else
            {
                backWheels = (WheelTypes)index;
            }
        }

        public CarObject getCarGameObject()
        {
            CarObject carObject = new CarObject("player");
            //TODO: figure out culling of paper model
            carObject.SceneNode.DisableCulling = true;
            carObject.SceneNode.PositionZ += 2;

            GameObject bodyObject = new GameObject("Body", SceneNodeType.Simple);
            bodyObject.SceneNode.DisableCulling = true;

            GameObject rightFrontWheelsObj = new GameObject("Wheel");
            GameObject leftFrontWheelsObj = new GameObject("Wheel");

            GameObject rightBackWheelsObj = new GameObject("Wheel");
            GameObject leftBackWheelsObj = new GameObject("Wheel");

            if (body == BodyTypes.STONE)
            {
                Model carModel = ResourcesManager.Instance.GetModel("Models/rockCar/rockCar");
                bodyObject.AddComponent(new ModelRenderer(carModel));
                bodyObject.SceneNode.Scale = new Vector3(2f, 2f, 2f);
                bodyObject.SceneNode.Rotation = new Vector3(Util.degToRad(270f), Util.degToRad(270f), Util.degToRad(0f));
                bodyObject.SceneNode.PositionY += 3f;
            }
            else if (body == BodyTypes.SHOPPING_CART)
            {
                Model carModel = ResourcesManager.Instance.GetModel("Models/cart");
                bodyObject.AddComponent(new ModelRenderer(carModel));
            }
            else if (body == BodyTypes.PAPER)
            {
                Model carModel = ResourcesManager.Instance.GetModel("Models/paperCar/paperRollCar");
                bodyObject.AddComponent(new ModelRenderer(carModel));
                bodyObject.SceneNode.Rotation = new Vector3(Util.degToRad(0f), Util.degToRad(90f), Util.degToRad(0f));
                bodyObject.SceneNode.Scale = new Vector3(1.33f, 1.33f, 1.33f);
                bodyObject.SceneNode.PositionY += 1.5f;
            }
            else//default
            {
                Model carModel = ResourcesManager.Instance.GetModel("Models/MuscleCar");
                bodyObject.AddComponent(new ModelRenderer(carModel));
            }


            if (frontWheels == WheelTypes.PUMPKIN)
            {
                Model fWheelModel = ResourcesManager.Instance.GetModel("Models/extraWheels/pumpkin");

                rightFrontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                rightFrontWheelsObj.SceneNode.PositionX -= 2;
                rightFrontWheelsObj.SceneNode.PositionY += .5f;
                rightFrontWheelsObj.SceneNode.PositionZ -= 2.5f;
                rightFrontWheelsObj.SceneNode.Scale = new Vector3(.25f, .25f, .25f);

                leftFrontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                leftFrontWheelsObj.SceneNode.PositionX -= 2;
                leftFrontWheelsObj.SceneNode.PositionY += .5f;
                leftFrontWheelsObj.SceneNode.Scale = new Vector3(.25f, .25f, .25f);
            }
            else if (frontWheels == WheelTypes.STONE)
            {
                Model fWheelModel = ResourcesManager.Instance.GetModel("Models/rockCar/rockWheel");

                rightFrontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                rightFrontWheelsObj.SceneNode.Scale = new Vector3(.5f, .5f, .5f);
                rightFrontWheelsObj.SceneNode.PositionX -= 2f;
                rightFrontWheelsObj.SceneNode.PositionY += .5f;
                rightFrontWheelsObj.SceneNode.PositionZ -= 1.25f;

                leftFrontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                leftFrontWheelsObj.SceneNode.Scale = new Vector3(.5f, .5f, .5f);
                leftFrontWheelsObj.SceneNode.PositionX -= 2f;
                leftFrontWheelsObj.SceneNode.PositionY += .5f;
                leftFrontWheelsObj.SceneNode.PositionZ += 1.25f;
            }
            else if(frontWheels == WheelTypes.PAPER)
            {
                Model fWheelModel = ResourcesManager.Instance.GetModel("Models/paperCar/paperRollWheel");

                rightFrontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                rightFrontWheelsObj.SceneNode.PositionX -= 1.75f;
                rightFrontWheelsObj.SceneNode.PositionY += .5f;
                rightFrontWheelsObj.SceneNode.PositionZ += .4f;

                leftFrontWheelsObj.AddComponent(new ModelRenderer(fWheelModel));
                leftFrontWheelsObj.SceneNode.PositionX -= 1.75f;
                leftFrontWheelsObj.SceneNode.PositionY += .5f;
                leftFrontWheelsObj.SceneNode.PositionZ += 3f;
            }


            if (backWheels == WheelTypes.PUMPKIN)
            {
                Model bWheelModel = ResourcesManager.Instance.GetModel("Models/extraWheels/pumpkin");
                rightBackWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                rightBackWheelsObj.SceneNode.PositionX += 2;
                rightBackWheelsObj.SceneNode.PositionY += .5f;
                rightBackWheelsObj.SceneNode.PositionZ -= 2.5f;
                rightBackWheelsObj.SceneNode.Scale = new Vector3(.25f, .25f, .25f);

                leftBackWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                leftBackWheelsObj.SceneNode.PositionX += 2;
                leftBackWheelsObj.SceneNode.PositionY += .5f;
                leftBackWheelsObj.SceneNode.Scale = new Vector3(.25f, .25f, .25f);
            }
            else if (backWheels == WheelTypes.STONE)
            {
                Model bWheelModel = ResourcesManager.Instance.GetModel("Models/rockCar/rockWheel");

                rightBackWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                rightBackWheelsObj.SceneNode.Scale = new Vector3(.5f, .5f, .5f);
                rightBackWheelsObj.SceneNode.PositionX += 2f;
                rightBackWheelsObj.SceneNode.PositionY += .5f;
                rightBackWheelsObj.SceneNode.PositionZ -= 1.25f;

                leftBackWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                leftBackWheelsObj.SceneNode.Scale = new Vector3(.5f, .5f, .5f);
                leftBackWheelsObj.SceneNode.PositionX += 2f;
                leftBackWheelsObj.SceneNode.PositionY += .5f;
                leftBackWheelsObj.SceneNode.PositionZ += 1.25f;
            }
            else if(backWheels == WheelTypes.PAPER)
            {
                Model bWheelModel = ResourcesManager.Instance.GetModel("Models/paperCar/paperRollWheel");

                rightBackWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                rightBackWheelsObj.SceneNode.PositionX += 1.75f;
                rightBackWheelsObj.SceneNode.PositionY += .5f;
                rightBackWheelsObj.SceneNode.PositionZ += .4f;

                leftBackWheelsObj.AddComponent(new ModelRenderer(bWheelModel));
                leftBackWheelsObj.SceneNode.PositionX += 1.75f;
                leftBackWheelsObj.SceneNode.PositionY += .5f;
                leftBackWheelsObj.SceneNode.PositionZ += 3f;
            }

            //causes a crash
            //foreach (MaterialAPI material in carObject.GetComponent<ModelRenderer>().GetMaterials())
            //{
            //    material.DiffuseColor = Color.Red;
            //}


            bodyObject.Parent = carObject;

            rightFrontWheelsObj.Parent = carObject;
            leftFrontWheelsObj.Parent = carObject;

            rightBackWheelsObj.Parent = carObject;
            leftBackWheelsObj.Parent = carObject;

            return carObject;
        }

        public float getCarWeight()
        {
            return bodyWeights[body] + wheelWeights[frontWheels] + wheelWeights[backWheels];
        }

        public float getCarTurnSpeed()
        {
            return (wheelTurnSpeeds[backWheels]+ wheelTurnSpeeds[frontWheels])/2;
        }

        public float getCarDamageReduction()
        {
            return (bodyDR[body] + wheelDR[frontWheels] + wheelDR[backWheels])/3;
        }
    }

}
