using CarProto.CustomComponents;
using CarProto.CustomGameObjects;
using GeonBit;
using GeonBit.ECS;
using GeonBit.ECS.Components.Graphics;
using GeonBit.ECS.Components.Sound;
using GeonBit.ECS.Components.Physics;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarProto
{
    /// <summary>
    /// Your main game class!
    /// </summary>
    internal class Game1 : GeonBitGame
    {
        GameObject carObject;
        GameObject cameraObject;
        GameObject trackObject;
        GameObject gameManager;
        gameManager gm;

        bool showTutorial = true; 
        /// <summary>
        /// Initialize your GeonBitGame properties here.
        /// </summary>
        public Game1()
        {
            InitParams.UiTheme = "hd";
            InitParams.DebugMode = true;
            InitParams.EnableVsync = true;
            InitParams.Title = "Death 'N' Derby";
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
            if (Managers.GameInput.IsKeyDown(GeonBit.Input.GameKeys.Escape) || gm.quitFlag)
            {
                Exit();
            }
            else
            {
                
                if (!gm.gameIsRunning) { return; }

                if(gm.isGameOver())
                {
                    ActiveScene.Root.Find("ui").GetComponent<uiUpdate>().DisplayGameOver();
                }
                /// TBD add any custom Update functionality here.
            }
        }

        /// <summary>
        /// Initialize to implement per main type.
        /// </summary>
        override public void Initialize()
        {
            // Draw physics shapes
            Managers.Diagnostic.DebugRenderPhysics = true;

            gameManager = new GameObject("gameManager");
            gm = new gameManager();
            gameManager.AddComponent(gm);
            
            /// Example 3: add 3d shape to scene
            carObject = new GameObject("player");
            Model carModel = Resources.GetModel("Models/MuscleCar");
            carObject.AddComponent(new ModelRenderer(carModel));
            PlayerController pc = new PlayerController();
            pc.weight = 10;

            gm.pc = pc;

            carObject.AddComponent(pc);
            carObject.SceneNode.Rotation = new Vector3(util.degToRad(0f), util.degToRad(270f), util.degToRad(270f));
            carObject.Parent = ActiveScene.Root;

            // Add body just for visual diagnostics
            KinematicBody playerBody = new KinematicBody(new BoxInfo(new Vector3(8, 8, 5)));
            playerBody.InvokeCollisionEvents = true;
            carObject.AddComponent(playerBody);

            // Add obstacle
            Obstacle obstacle1 = new Obstacle(5, 100, carObject);
            Obstacle obstacle2 = new Obstacle(-5, 150, carObject);
            Obstacle obstacle3 = new Obstacle(10, 200, carObject);
            Obstacle obstacle4 = new Obstacle(8, 250, carObject);
            Obstacle obstacle5 = new Obstacle(6, 230, carObject);
            Obstacle obstacle6 = new Obstacle(-10, 350, carObject);
            Obstacle obstacle7 = new Obstacle(10, 350, carObject);
            Obstacle obstacle8 = new Obstacle(0, 400, carObject);
            // Add body just for visual diagnostics
            obstacle1.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle2.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle3.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle4.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle5.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle6.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle7.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));
            obstacle8.AddComponent(new StaticBody(new BoxInfo(new Vector3(8, 8, 8))));

            trackObject = new GameObject("track");
            Model trackModel = Resources.GetModel("Models/Track");
            trackObject.AddComponent(new ModelRenderer(trackModel));
            trackObject.SceneNode.Rotation = new Vector3(util.degToRad(0f), util.degToRad(270f), util.degToRad(270f));
            trackObject.SceneNode.Scale = new Vector3(.50f, .25f, .25f);
            trackObject.Parent = ActiveScene.Root;
            trackObject.SceneNode.Position = new Vector3(25f, -60f, 0f);

            GameObject backgroundObject = new GameObject("background");
            Texture2D backgroundimage = Resources.GetTexture("Images/SpyHunter");
            SceneBackground background = new SceneBackground(backgroundimage)
            {
                DrawMode = BackgroundDrawMode.Tiled
            };
            backgroundObject.AddComponent(background);
            backgroundObject.Parent = ActiveScene.Root;

            cameraObject = new GameObject("camera", SceneNodeType.Simple);
            cameraObject.AddComponent(new Camera());
            cameraObject.AddComponent(new CameraFollow());
            cameraObject.SceneNode.Rotation = new Vector3(util.degToRad(60), util.degToRad(0), util.degToRad(0));

            CameraFollow cf = cameraObject.GetComponent<CameraFollow>();
            cf.offset = new Vector3(0, -35, 30);
            cf.dampingStrength = .07f;

            cameraObject.Parent = ActiveScene.Root;

            // cameraObject.AddComponent(new PlayerController());
            // add diagnostic data paragraph to scene
            var diagnosticData = new GeonBit.UI.Entities.Paragraph("", GeonBit.UI.Entities.Anchor.BottomLeft, offset: Vector2.One * 10f, scale: 0.7f);
            diagnosticData.BeforeDraw = (GeonBit.UI.Entities.Entity entity) =>
            {
                diagnosticData.Text = Managers.Diagnostic.GetReportString();
            };
            ActiveScene.UserInterface.AddEntity(diagnosticData);

            //addGrid();
            if(showTutorial)
            {
                addTutorialGui();
            }

            addSound();
            addGameGui();
            
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

            Button quitButton = new Button("Quit", ButtonSkin.Fancy, Anchor.BottomCenter);
            quitButton.OnClick = (Entity btn) =>
            {
                gm.quitFlag = true;
            };
            gameOverPanel.AddChild(quitButton);

            GameObject uiManager = new GameObject("ui");
            uiUpdate ui = new uiUpdate(timeDisplay, damageDisplay, speedDisplay, gameOverPanel);
            uiManager.AddComponent(ui);
            uiManager.Parent = ActiveScene.Root;

            

            UserInterface.Active.AddEntity(statPanel);
            UserInterface.Active.AddEntity(gameOverPanel);
        }

        void addSound()
        {           
            GameObject backMusic = new GameObject("background_music");
            backMusic.AddComponent(new BackgroundMusic("Audio/BGM"));
            backMusic.Parent = ActiveScene.Root;
            backMusic.GetComponent<BackgroundMusic>().Play();
        }
        void addTutorialGui()
        {
            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 600), PanelSkin.Default, Anchor.CenterLeft);
            UserInterface.Active.AddEntity(panel);

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
            Model planeModel = Resources.GetModel("GeonBit.Core/BasicMeshes/Plane");
            gridObject.AddComponent(new ModelRenderer(planeModel));
            Texture2D gridTex = Resources.GetTexture("Images/grid");
            gridObject.GetComponent<ModelRenderer>().GetMaterials()[0].Texture = gridTex;
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
