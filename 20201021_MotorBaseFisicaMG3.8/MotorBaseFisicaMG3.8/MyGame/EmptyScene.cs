using Microsoft.Xna.Framework;
using MotorBaseFisicaMG38.SistemaDibujado;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotorBaseFisicaMG38.MyGame
{
    class EmptyScene : Escena
    {
        Camara camara;

        public EmptyScene()
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
        public void SetCentro(Dibujable newCentro)
        {
            camara.centro = newCentro;
        }
    }
}
