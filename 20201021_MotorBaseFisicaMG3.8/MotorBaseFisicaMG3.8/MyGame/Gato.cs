using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MotorBaseFisicaMG38.SistemaAudio;
using MotorBaseFisicaMG38.SistemaGameObject;

namespace MotorBaseFisicaMG38.MyGame
{
    class Gato : UTGameObject
    {
        
        public Gato() : base("meow_cookie", new Vector2(300, 300), 1, UTGameObject.FF_form.Circulo)
        {

        }
        public override void Update(GameTime gameTime)
        {
            float vel = 100;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {              
                rot += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {                
                rot -= (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Vector2 frente = Rotate(new Vector2(0, -(float)gameTime.ElapsedGameTime.TotalSeconds * vel), rot); 
                objetoFisico.AddVelocity(frente);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Vector2 frente = Rotate(new Vector2(0, -(float)gameTime.ElapsedGameTime.TotalSeconds * vel), rot);
                objetoFisico.AddVelocity(-frente);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Destroy();
            }
            
        }
        public override void OnCollision(UTGameObject other)
        {
            Coleccionable col = other as Coleccionable;
            if (col != null)
            {
                Debug.WriteLine("Velocidad Col:" + col.objetoFisico.vel);
                col.Destroy();
                AudioManager.Play(AudioManager.Sounds.Mystic);
            }
            else
            {
                //Sonido si choca con objetos no coleccionables
            }
        }
        Vector2 Rotate(Vector2 v, double degrees)
        {
            float sin = (float)Math.Sin(degrees);
            float cos = (float)Math.Cos(degrees);

            float tx = v.X;
            float ty = v.Y;
            v.X = (cos * tx) - (sin * ty);
            v.Y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}
