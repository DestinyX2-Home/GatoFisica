using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotorBaseFisicaMG38.SistemaDibujado;
using MotorBaseFisicaMG38.SistemaFisico;
using MotorBaseFisicaMG38.SistemaGameObject;

namespace MotorBaseFisicaMG38.SistemaGO
{
    public class Eslabon
    {
        Vector2 p1;
        Vector2 p2;
        Vector2 lp1;
        Vector2 lp2;

        float lenght;
        public Dibujable texture;
        public Eslabon prev;
        public Eslabon next;
        public bool fix;

        public Eslabon(Vector2 pos1, Vector2 pos2, string textureName, bool fixd = false)
        {
            p1 = pos1;
            p2 = pos2;
            lp1 = p1;
            lp2 = p2;
            fix = fixd;
            texture = new Dibujable(textureName, (p2 + p1) / 2, 1);
            texture.rot = (float)Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
            lenght = texture.ancho - 15;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MovePoints(elapsed);
            //ConstraintPoints();

            texture.rot = (float)Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
            texture.pos = (p2 + p1) / 2;

            UpdateNext(gameTime);
        }


        public void MovePoints(float time)
        {
            var dy = ((p2 - lp2) / 4) * 3;
            lp1 = p1;
            lp2 = p2;


            if (!fix)
            {
                if (prev != null)
                {
                    prev.p2 = p1;
                }
                if (next != null)
                {
                    p2 = next.p1;
                }
                p1 += dy;
            }

        }
        public void UpdateNext(GameTime gameTime)
        {
            if (next != null)
            {
                next.Update(gameTime);
            }
        }

        public void CreateChain(int lengt)
        {
            Eslabon current = this;

            int tipe = 2;

            for (int i = 0; i < lengt; i++)
            {
                if (tipe == 2)
                {
                    current.next = new Eslabon(current.p2, new Vector2(current.p2.X, current.p2.Y + current.lenght), "eslabon2");
                    tipe = 1;
                }
                else
                {
                    current.next = new Eslabon(current.p2, new Vector2(current.p2.X, current.p2.Y + current.lenght), "eslabon1");
                    tipe = 2;
                }
                current.next.prev = current;
                current = current.next;

            }
        }

        public float DistanceBetween(Vector2 p1, Vector2 p2)
        {
            float dist = (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            return dist;
        }
        public void MovePrevs(Vector2 force, Eslabon first)
        {
            Eslabon toMove = first;
            if (toMove.prev != null)
                AddForce(toMove.prev, force);
        }
        public void AddForce(Eslabon slb, Vector2 force)
        {
            if (DistanceBetween(slb.p1, slb.p2 + force * 2) < lenght + 10)
            {
                slb.p2 += force * 2;
                MovePrevs(force, slb);
            }
        }
        public void MoveLast(Vector2 move)
        {
            Eslabon slb = this;
            while (slb.next != null)
            {
                slb = slb.next;
            }
            AddForce(slb, move);

        }
    }
}