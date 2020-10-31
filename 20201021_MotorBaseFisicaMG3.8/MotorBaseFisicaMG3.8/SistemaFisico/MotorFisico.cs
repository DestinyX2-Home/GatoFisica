using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorBaseFisicaMG38.SistemaFisico
{
    public static class MotorFisico
    { 
        public static float WorldSizeScale = 100f;
        public static Vector2 Gravity = new Vector2(0, 9.8f);
        public static float Roce = .1f;
        public static float RoceAire = .1f;
        public static List<ObjetoFisico> objetosFisicos = new List<ObjetoFisico>();
        public static void agregarObjetoFisico(ObjetoFisico of)
        {
            objetosFisicos.Add(of);
        }
        public static void removerObjetoFisico(ObjetoFisico of)
        {
            objetosFisicos.Remove(of);
        }
        public static void Update(GameTime gameTime)
        {
            foreach(ObjetoFisico of in objetosFisicos)
            {
                of.isColliding = false;
            }
            for(int i=0; i<objetosFisicos.Count;i++)
            {
                
                for(int j=i+1; j < objetosFisicos.Count; j++)
                {
                    ObjetoFisico objetoA = objetosFisicos[i];
                    ObjetoFisico objetoB = objetosFisicos[j];
                    Vector2 collisionPoint;
                    if ((!objetoA.isStatic || !objetoB.isStatic) && objetoA.Colisiona(objetoB, out collisionPoint))
                    {
                        if(objetoA.OnCollision !=null && objetoB.GetObject != null)
                        {
                            objetoA.OnCollision(objetoB.GetObject());
                        }
                        if (objetoB.OnCollision != null && objetoA.GetObject != null)
                        {
                            objetoB.OnCollision(objetoA.GetObject());
                        }
                        //verificamos si alguno de los objetos no es trigger para que sólo en ese caso se apliquen fuerzas
                        if (!(objetoA.isTrigger || objetoB.isTrigger))
                        {
                            //Aplicación de fuerzas
                            objetoA.isColliding = objetoB.isColliding = true;
                            
                            Vector2 CentrosAB = collisionPoint - objetoA.pos;
                            Vector2 CentrosBA = collisionPoint - objetoB.pos;                           
                            {
                                Vector2 velA = ((objetoA.vel + objetoA.prevVel) / 2);
                                objetoA.vel = velA;
                                //Debug.WriteLine("Velocidad Entrada" + velA);
                                Vector2 velB = ((objetoB.vel+objetoB.prevVel)/2);
                                objetoB.vel = velB;
                                //No olvidar verificar que la dirección lleva a un acercamiento, si se están alejando
                                //no debemos calcular equilibrio de fuerzas
                                double angleAB = AngleBetween(objetoA.vel, CentrosAB);
                                Vector2 FuerzaAB = Vector2.Zero;
                                Vector2 FuerzaBA = Vector2.Zero;                                
                                if (angleAB < MathF.PI/2 && angleAB > -MathF.PI/2)
                                {
                                    CentrosAB.Normalize();
                                    //Descomponemos la velocidad de A en 2 fuerzas en base a la colisión
                                    FuerzaAB = CentrosAB * (float)Math.Cos(angleAB) * velA.Length() * objetoA.masa;
                                    FuerzaBA = (velA * objetoA.masa - FuerzaAB) * (1 - objetoB.absorcionChoque);
                                    FuerzaAB *= (1 - objetoA.absorcionChoque);
                                    objetoA.vel -= velA;
                                    if (objetoB.isStatic)
                                    {
                                        objetoA.AplicaFuerza(-FuerzaAB, 1, collisionPoint, true, true); 
                                        //Debug.WriteLine("FuerzaAB" + -FuerzaAB);
                                        objetoA.AplicaFuerza(FuerzaBA, 1, collisionPoint, true, true);
                                        //Debug.WriteLine("FuerzaBA" + FuerzaBA);
                                    }
                                    else
                                    {
                                        objetoB.AplicaFuerza(FuerzaAB, 1, collisionPoint, true, true);
                                        objetoA.AplicaFuerza(FuerzaBA, 1, collisionPoint, true, true);

                                    }
                                }                                                           
                             

                                //Descomponemos la velocidad de B en 2 fuerzas en base a la colisión
                                double angleBA = AngleBetween(objetoB.vel, CentrosBA);
                                if (angleBA < MathF.PI / 2 && angleBA > -MathF.PI / 2)
                                {
                                    CentrosBA.Normalize();
                                    FuerzaBA = CentrosBA * (float)Math.Cos(angleBA) * velB.Length() * objetoB.masa;
                                    FuerzaAB = (velB * objetoB.masa - FuerzaBA) * (1 - objetoA.absorcionChoque);
                                    FuerzaBA *= (1 - objetoB.absorcionChoque);
                                    objetoB.vel -= velB;
                                    if (objetoA.isStatic)
                                    {
                                        objetoB.AplicaFuerza(FuerzaAB, 1, collisionPoint, true, true);
                                        objetoB.AplicaFuerza(-FuerzaBA, 1, collisionPoint, true, true);
                                    }
                                    else
                                    {
                                        objetoB.AplicaFuerza(FuerzaAB, 1, collisionPoint, true, true);
                                        objetoA.AplicaFuerza(FuerzaBA, 1, collisionPoint, true, true);
                                    }
                                }
                            }                            
                        }
                    }
                }                
            }
            foreach (ObjetoFisico of in objetosFisicos)
            {                
                if (!of.isStatic)
                {                   
                    float tiempo = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    of.AplicaFuerza(Gravity * of.masa, tiempo);
                    if (of.lastCollision != null)
                    {
                        Vector2 normal = new Vector2(MathF.Sin(of.lastCollision.rot), MathF.Cos(of.lastCollision.rot));
                        Debug.WriteLine(normal);
                        of.AplicaFuerza(normal * of.masa, tiempo);
                    }
                    of.Update(tiempo);
                }
            }

        }
        public static double AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos);
        }
        public static Vector2 Rotate(Vector2 v, double degrees)
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
