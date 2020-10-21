using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MotorBaseFisicaMG38.MyGame.Components;
using MotorBaseFisicaMG38.SistemaDibujado;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MotorBaseFisicaMG38.MyGame
{
    class MenuScene : Escena
    {
        Camara camara;

        public MenuScene()
        {
            camara = new Camara(new Vector2(0, 0), 1, 0);
            camara.HacerActiva();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Component cp in components)
            {
                cp.Update(gameTime);
            }
        }
    }
}
