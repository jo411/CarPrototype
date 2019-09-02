using GeonBit.ECS.Components;
using GeonBit.UI.Entities;
using System;

namespace CarProto.CustomComponents
{
    class UiUpdate : BaseComponent
    {
        public Paragraph timeDisplay;
        public Paragraph damageDisplay;
        public Paragraph speedDisplay;
        public Panel gameOverPanel;
        public Panel gameOverImagePanel;
        private TimeSpan current;

        PlayerController pc;

        public UiUpdate(Paragraph timeDisplay, Paragraph damageDisplay, Paragraph speedDisplay, Panel gameOver, Panel gameOverImagePanel)
        {
            this.timeDisplay = timeDisplay;
            this.damageDisplay = damageDisplay;
            this.speedDisplay = speedDisplay;
            this.gameOverPanel = gameOver;
            this.gameOverImagePanel = gameOverImagePanel;
            current = new TimeSpan();
        }

        protected override void OnAddToScene()
        {
            pc = _GameObject.ParentScene.Root.Find("player").GetComponent<PlayerController>();
        }

        public override BaseComponent Clone()
        {
            return new UiUpdate(timeDisplay, damageDisplay, speedDisplay, gameOverPanel, gameOverImagePanel);
        }

        /// <summary>
        /// Do on-frame based update.
        /// </summary>
        protected override void OnUpdate()
        {
            updateSpeedDisplay();
            updateTimeDisplay();
            updateDamageDisplay();
        }

        private void updateTimeDisplay()
        {
            current = current.Add(Managers.TimeManager.ElapsedTime);
            timeDisplay.Text = current.ToString(@"mm\:ss\:ff");

            timeDisplay.MarkAsDirty();
        }

        private void updateSpeedDisplay()
        {
            speedDisplay.Text = pc.movingSpeed.ToString("####.#") + " MPH";
            speedDisplay.MarkAsDirty();
        }

        private void updateDamageDisplay()
        {
            float damage = pc.damage;
            damageDisplay.Text = "  " + damage.ToString() + "% Damaged";


            if (damage < 50)
            {
                damageDisplay.FillColor = Microsoft.Xna.Framework.Color.Green;
            }
            else if (damage < 75)
            {
                damageDisplay.FillColor = Microsoft.Xna.Framework.Color.Yellow;
            }
            else
            {
                damageDisplay.FillColor = Microsoft.Xna.Framework.Color.Red;
            }

            damageDisplay.MarkAsDirty();
        }

        public void DisplayGameOver(bool gameWon)
        {
            Paragraph gameOverText = gameOverPanel.Find<Paragraph>("gameover");
           
            if (gameWon)
            {
                gameOverText.Text = "Hey you survived to the end; congratulations are in order I suppose...";
                Image image = gameOverImagePanel.Find<Image>("win");
                image.Visible = true;
            }
            else
            {
                gameOverText.Text = "Oh no thats a Game Over for you! \n" +
                                                   "Someone may want to call the medics...\n";
               Image image = gameOverImagePanel.Find<Image>("lose");
                image.Visible = true;
            }
            gameOverText.Text += "\n Final Time: " + current.ToString(@"mm\:ss\:ff");
            gameOverPanel.Visible = true;
        }
    }
}
