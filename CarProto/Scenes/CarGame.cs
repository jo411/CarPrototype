using CarProto.CustomComponents;
using CarProto.CustomGameObjects;
using GeonBit.ECS;
using GeonBit.ECS.Components.Graphics;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeonBit;
using GeonBit.ECS.Components.Physics;
using GeonBit.ECS.Components.Sound;
using GeonBit.Core;
using GeonBit.Managers;

namespace CarProto
{
    class CarGame : GameScene
    {
        private GameState gameState;

        GameObject carObject;
        GameObject cameraObject;
        GameObject trackObject;
        GameObject gameManager;
        gameManager gm;

        uiUpdate UIUpdater;

        bool showTutorial = true;
        bool showDebug = true;

        public CarGame(GameState gameState)
        {
            this.gameState = gameState;
            init();
        }


        public void doUpdate()
        {
           checkQuit();
        }
      
        void checkQuit()
        {
            if (!gm.gameIsRunning) { return; }

            if (gm.isGameOver())
            {
               UIUpdater.DisplayGameOver();
            }

        }
        void init()
        {
            addGameManager();
            addPlayer();
            addObstacles();
            addTrack();
            addCamera();
            //addTutorialGui();
            addSound();
            addGameGui();
            addDebug();

        }

        void addGameManager()
        {
            gameManager = new GameObject("gameManager");
            gm = new gameManager();
            gameManager.AddComponent(gm);
        }

        void addDebug()
        {
            if (showDebug)
            {
                var diagnosticData = new GeonBit.UI.Entities.Paragraph("", GeonBit.UI.Entities.Anchor.BottomLeft, offset: Vector2.One * 10f, scale: 0.7f);
                diagnosticData.BeforeDraw = (GeonBit.UI.Entities.Entity entity) =>
                {
                    diagnosticData.Text =  Diagnostic.Instance.GetReportString();
                };
                UserInterface.AddEntity(diagnosticData);

                Diagnostic.Instance.DebugRenderPhysics = true;

            }
        }

        void addCamera()
        {
            cameraObject = new GameObject("camera", SceneNodeType.Simple);
            cameraObject.AddComponent(new Camera());
            cameraObject.AddComponent(new CameraFollow());
            cameraObject.SceneNode.Rotation = new Vector3(util.degToRad(60), util.degToRad(0), util.degToRad(0));

            CameraFollow cf = cameraObject.GetComponent<CameraFollow>();
            cf.offset = new Vector3(0, -35, 30);
            cf.dampingStrength = .07f;

            cameraObject.Parent = Root;

        }

        void addTrack()
        {
            trackObject = new GameObject("track");
            Model trackModel = ResourcesManager.Instance.GetModel("Models/Track");
            trackObject.AddComponent(new ModelRenderer(trackModel));
            trackObject.SceneNode.Rotation = new Vector3(util.degToRad(0f), util.degToRad(270f), util.degToRad(270f));
            trackObject.SceneNode.Scale = new Vector3(.50f, .25f, .25f);
            trackObject.Parent = Root;
            trackObject.SceneNode.Position = new Vector3(25f, -60f, 0f);

            GameObject backgroundObject = new GameObject("background");
            Texture2D backgroundimage = ResourcesManager.Instance.GetTexture("Images/SpyHunter");
            SceneBackground background = new SceneBackground(backgroundimage)
            {
                DrawMode = BackgroundDrawMode.Tiled
            };
            backgroundObject.AddComponent(background);
            backgroundObject.Parent = Root;

        }
        void addObstacles()
        {
            // Add obstacle
            Obstacle obstacle1 = new Obstacle(5, 100, carObject,this);
            Obstacle obstacle2 = new Obstacle(-5, 150, carObject, this);
            Obstacle obstacle3 = new Obstacle(10, 200, carObject, this);
            Obstacle obstacle4 = new Obstacle(8, 250, carObject, this);
            Obstacle obstacle5 = new Obstacle(6, 230, carObject, this);
            Obstacle obstacle6 = new Obstacle(-10, 350, carObject, this);
            Obstacle obstacle7 = new Obstacle(10, 350, carObject, this);
            Obstacle obstacle8 = new Obstacle(0, 400, carObject, this);
            // Add body just for visual diagnostics
            obstacle1.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle2.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle3.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle4.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle5.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle6.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle7.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle8.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));

        }
        void addPlayer()
        {
            carObject = new GameObject("player");
            Model carModel = ResourcesManager.Instance.GetModel("Models/MuscleCar");
            carObject.AddComponent(new ModelRenderer(carModel));
            PlayerController pc = new PlayerController();           

            gm.pc = pc;

            carObject.AddComponent(pc);
            carObject.SceneNode.Rotation = new Vector3(util.degToRad(0f), util.degToRad(270f), util.degToRad(270f));
            carObject.Parent = Root;

            // Add body just for visual diagnostics
            KinematicBody playerBody = new KinematicBody(new BoxInfo(new Vector3(8, 8, 5)));
            playerBody.InvokeCollisionEvents = true;
            carObject.AddComponent(playerBody);
        }

        void addGameGui()
        {

            Panel statPanel = new Panel(new Vector2(600, 100), PanelSkin.Golden, Anchor.TopCenter);

            Paragraph timeDisplay = new Paragraph("", Anchor.Center);
            Paragraph speedDisplay = new Paragraph("", Anchor.CenterLeft);
            Paragraph damageDisplay = new Paragraph("", Anchor.CenterRight);

            statPanel.AddChild(timeDisplay);
            statPanel.AddChild(speedDisplay);
            statPanel.AddChild(damageDisplay);


            Panel gameOverPanel = new Panel(new Vector2(500, 500), PanelSkin.Fancy, Anchor.Center);
            Paragraph gameOverText = new Paragraph("Oh no thats a Game Over for you! \n" +
                                                   "Someone may want to call the medics...\n");
            gameOverText.Identifier = "gameover";
            gameOverPanel.AddChild(gameOverText);
            gameOverPanel.Visible = false;

            Button quitButton = new Button("Return to Menu", ButtonSkin.Fancy, Anchor.BottomCenter);
            quitButton.OnClick = (Entity btn) =>
            {
                gameState.changeScene(State.MAIN_MENU);
            };
            gameOverPanel.AddChild(quitButton);

            GameObject uiManager = new GameObject("ui");
             UIUpdater = new uiUpdate(timeDisplay, damageDisplay, speedDisplay, gameOverPanel);
            uiManager.AddComponent(UIUpdater);
            uiManager.Parent = Root;



            UserInterface.AddEntity(statPanel);
            UserInterface.AddEntity(gameOverPanel);
        }

        void addSound()
        {
            GameObject backMusic = new GameObject("background_music");
            backMusic.AddComponent(new BackgroundMusic("Audio/BGM"));
            backMusic.Parent = Root;
            backMusic.GetComponent<BackgroundMusic>().Play();
        }
        void addTutorialGui()
        {

            if (!showTutorial)
            {
                return;
            }
            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 600), PanelSkin.Default, Anchor.CenterLeft);
            UserInterface.AddEntity(panel);

            // add title and text
            panel.AddChild(new Header("   Welcome to \nDeath 'N' Derby!"));
            panel.AddChild(new HorizontalLine());
            var tutorialText = new Paragraph("Reach the end of the track without dying in a blaze of glory!\n\n" +
                                                "Use A and D to steer side to side to avoid objects.\n\n" +
                                                "Be careful not to take too much damage or you may find steering difficult...");
            panel.AddChild(tutorialText);

            // add a button at the bottom

            Button closeTut = new Button("Click to hide!", ButtonSkin.Fancy, Anchor.BottomCenter);
            closeTut.OnClick = (Entity btn) =>
            {
                btn.Parent.Visible = false;
            };
            panel.AddChild(closeTut);
        }

        void addGrid()
        {
            GameObject gridObject;
            gridObject = new GameObject("shape");
            Model planeModel = ResourcesManager.Instance.GetModel("GeonBit.Core/BasicMeshes/Plane");
            gridObject.AddComponent(new ModelRenderer(planeModel));
            Texture2D gridTex = ResourcesManager.Instance.GetTexture("Images/grid");
            gridObject.GetComponent<ModelRenderer>().GetMaterials()[0].Texture = gridTex;
            gridObject.GetComponent<ModelRenderer>().GetMaterials()[0].TextureEnabled = true;
            gridObject.SceneNode.Rotation = new Vector3(util.degToRad(0), util.degToRad(0), util.degToRad(0));
            gridObject.SceneNode.Scale = new Vector3(100, 100, 1);
            gridObject.Parent = Root;
        }
    }
}
