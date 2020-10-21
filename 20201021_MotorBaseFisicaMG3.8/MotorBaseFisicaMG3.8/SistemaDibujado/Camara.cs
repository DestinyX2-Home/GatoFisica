using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace MotorBaseFisicaMG38.SistemaDibujado
{
    public class Camara
    {
        public Vector2 pos;
        public Vector2 tam;
        public float escala;
        public float rot;
        public static Camara ActiveCamera;
        public Dibujable centro;

        public Camara(Vector2 pos, float escala, float rot)
        {
            this.pos = pos;
            this.escala = escala;
            this.rot = rot;
            if (ActiveCamera == null)
            {
                ActiveCamera = this;
            }
        }

        public void Dibujar(SpriteBatch SB)
        {
            if (centro != null)
            {
                Vector2 cameraOffset = new Vector2(SB.GraphicsDevice.Viewport.Width, SB.GraphicsDevice.Viewport.Height) / escala / 2;
                this.pos = centro.pos - Rotate(cameraOffset, -rot);
            }
            foreach (Dibujable dib in Escena.INSTANCIA.dibujables)
            {
                dib.Draw(SB, pos, rot, escala);
            }
        }
        public bool EsCamaraActiva()
        {
            return (this == ActiveCamera);
        }
        public void HacerActiva()
        {
            ActiveCamera = this;
        }
        // Metodo para recuperar la posición del mouse en el mundo, respecto a esta cámara
        public Vector2 PosMouseEnCamara()
        {
            Point cameraPoint = Mouse.GetState().Position;
            Vector2 cameraRawPos = new Vector2(cameraPoint.X, cameraPoint.Y);
            return Rotate((cameraRawPos) / escala, -rot) + pos;
        }

        // Metodo utilitario para rotar un vector por cierta cantidad de grador
        public Vector2 Rotate(Vector2 v, double degrees)
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
