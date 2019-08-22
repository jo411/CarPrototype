﻿using Microsoft.Xna.Framework;
using GeonBit;
using GeonBit.ECS;
using GeonBit.ECS.Components;
using GeonBit.ECS.Components.Graphics;
using GeonBit.ECS.Components.Misc;
using GeonBit.ECS.Components.Particles;
using GeonBit.ECS.Components.Physics;
using GeonBit.ECS.Components.Sound;
using System.Diagnostics;
using GeonBit.Core.Graphics.Materials;

using CarProto.Custom_Components;
using Microsoft.Xna.Framework.Graphics;
using CarProto.CustomComponents;
using GeonBit.UI.Entities;
using GeonBit.UI;

namespace CarProto
{
    /// <summary>
    /// Your main game class!
    /// </summary>
    internal class Game1 : GeonBitGame
    {
        GameObject shapeObject;
        
        /// <summary>
        /// Initialize your GeonBitGame properties here.
        /// </summary>
        public Game1()
        {
            InitParams.UiTheme = "hd";
            InitParams.DebugMode = true;
            InitParams.EnableVsync = true;
            InitParams.Title = "New GeonBit Project";
            InitParams.FullScreen = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        override public void Update(GameTime gameTime)
        {
            /// exit application on escape
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Escape))
            {
                Exit();
            }

            /// TBD add any custom Update functionality here.
        }

        /// <summary>
        /// Initialize to implement per main type.
        /// </summary>
        override public void Initialize()
        {
            /// TBD create your scene, components and init resources here.
            /// The code below contains a simple example of how to use UI, camera, and basic entity renderer.

            /// Example 1: create UI text
            ActiveScene.UserInterface.AddEntity(new GeonBit.UI.Entities.Paragraph("Welcome to GeonBit! Here's a sphere:"));

            /// Example 3: add 3d shape to scene
            shapeObject = new GameObject("shape");
            Model carModel = Resources.GetModel("Models/MuscleCar");
            shapeObject.AddComponent(new ModelRenderer(carModel));
            shapeObject.Name = "player";
            shapeObject.AddComponent(new PlayerController());
            shapeObject.SceneNode.Rotation = new Vector3(util.degToRad(0f), util.degToRad(270f), util.degToRad(270f));
            shapeObject.SceneNode.Scale = new Vector3(1f, 1f, 1f);
            shapeObject.Parent = ActiveScene.Root;

            GameObject backgroundObject = new GameObject("background");
            Texture2D backgroundimage = Resources.GetTexture("Images/SpyHunter");
            SceneBackground background = new SceneBackground(backgroundimage)
            {
                DrawMode = BackgroundDrawMode.Tiled
            };
            backgroundObject.AddComponent(background);
            backgroundObject.Parent = ActiveScene.Root;

            GameObject cameraObject = new GameObject("camera", SceneNodeType.Simple);
            cameraObject.AddComponent(new Camera());
            cameraObject.AddComponent(new CameraFollow());
            cameraObject.SceneNode.Rotation = new Vector3(util.degToRad(60), util.degToRad(0), util.degToRad(0));

            CameraFollow cf = cameraObject.GetComponent<CameraFollow>();
            cf.setOffset(new Vector3(0, -50, 30));
            cf.dampingStrength = .07f;


            cameraObject.Parent = ActiveScene.Root; //shapeObject

            // cameraObject.AddComponent(new PlayerController());
            // add diagnostic data paragraph to scene
            var diagnosticData = new GeonBit.UI.Entities.Paragraph("", GeonBit.UI.Entities.Anchor.BottomLeft, offset: Vector2.One * 10f, scale: 0.7f);
            diagnosticData.BeforeDraw = (GeonBit.UI.Entities.Entity entity) =>
            {
                diagnosticData.Text = Managers.Diagnostic.GetReportString();
            };
            ActiveScene.UserInterface.AddEntity(diagnosticData);

            addGrid();
            addGui();
        }
        void addGui()
        {
            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.CenterLeft);
            UserInterface.Active.AddEntity(panel);

            // add title and text
            panel.AddChild(new Header("Death 'N' Derby"));
            panel.AddChild(new HorizontalLine());
            var richParagraph = new Paragraph(@"This text will have default color, but {{RED}}this part will be red{{DEFAULT}}.
                            This text will have regular weight, but {{BOLD}}this part will be bold{{DEFAULT}}.");
            panel.AddChild(new Paragraph("This is a simple panel with a button."));

            // add a button at the bottom
            panel.AddChild(new Button("Click Me!", ButtonSkin.Default, Anchor.BottomCenter));
        }

        void addGrid()
        {
            GameObject gridObject;
            gridObject = new GameObject("shape");
            Model planeModel = Resources.GetModel("GeonBit.Core/BasicMeshes/Plane");            
            gridObject.AddComponent(new ModelRenderer(planeModel));
            Texture2D gridTex = Resources.GetTexture("Images/grid");            
            gridObject.GetComponent<ModelRenderer>().GetMaterials()[0].Texture=gridTex;
            gridObject.GetComponent<ModelRenderer>().GetMaterials()[0].TextureEnabled = true;
            gridObject.SceneNode.Rotation = new Vector3(util.degToRad(0), util.degToRad(0), util.degToRad(0));
            gridObject.SceneNode.Scale = new Vector3(100, 100, 1);
            gridObject.Parent = ActiveScene.Root;
        }

        /// <summary>
        /// Draw function to implement per main type.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        override public void Draw(GameTime gameTime)
        {
            /// TBD add any custom drawing functionality here.
            /// Note: since GeonBit handle its own drawing internally, usually you don't need to do anything here.
        }
    }
}
