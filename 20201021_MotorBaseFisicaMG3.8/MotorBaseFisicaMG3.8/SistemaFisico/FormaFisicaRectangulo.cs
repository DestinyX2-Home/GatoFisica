using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorBaseFisicaMG38.SistemaFisico
{

    public class FormaFisicaRectangulo : FormaFisica
    {
        public float ancho;
        public float alto;        
        public FormaFisicaRectangulo(float ancho, float alto)
        {
            if (ancho < 0)
            {
                ancho *= -1f;
            }
            if (alto < 0)
            {
                alto *= -1f;
            }
            this.ancho = ancho;
            this.alto = alto;
            //supongo que la posición es el centro del rectangulo
        }
        public bool puntoEnRect(Vector2 punto, Vector2[] Esquinas, float rot)
        {
            punto = MotorFisico.Rotate(punto, -rot);
            Vector2[] NCEsquinas =new Vector2[4];
            for(int i = 0; i < 4; i++)
            {
                NCEsquinas[i] = MotorFisico.Rotate(Esquinas[i], -rot);
            }

            if (punto.X >= NCEsquinas[0].X && punto.X <= NCEsquinas[2].X &&
                           punto.Y >= NCEsquinas[0].Y && punto.Y <= NCEsquinas[2].Y)
            {
                return true;
            }
            return false;
        }
        public override bool colisiona(FormaFisica otra, out Vector2 collisionPoint)
        {
            collisionPoint = Vector2.Zero;
            try
            {
                Vector2[] esquinasPropias = new Vector2[4];
                esquinasPropias[0] = pos + MotorFisico.Rotate(new Vector2(-ancho / 2f, -alto / 2f), rot);
                esquinasPropias[1] = pos + MotorFisico.Rotate(new Vector2(ancho / 2f, -alto / 2f), rot);
                esquinasPropias[2] = pos + MotorFisico.Rotate(new Vector2(ancho / 2f, alto / 2f), rot);
                esquinasPropias[3] = pos + MotorFisico.Rotate(new Vector2(-ancho / 2f, alto / 2f), rot);

                FormaFisicaCirculo otroCirculo = otra as FormaFisicaCirculo;
                if (otroCirculo != null)
                {
                    float distanciaCuadrada = (otroCirculo.pos - this.pos).LengthSquared();
                    //calcular colision entre circulo y rectangulo
                    foreach(Vector2 esquina in esquinasPropias)
                    {
                        if(Vector2.DistanceSquared(esquina,otroCirculo.pos)< otroCirculo.radio * otroCirculo.radio)
                        {
                            collisionPoint = esquina;
                            return true;
                        }
                    }
                    Vector2[] extremosCirculo = new Vector2[4];
                    extremosCirculo[0] = otroCirculo.pos + MotorFisico.Rotate(new Vector2(otroCirculo.radio, 0), rot);
                    extremosCirculo[1] = otroCirculo.pos + MotorFisico.Rotate(new Vector2(-otroCirculo.radio, 0), rot);
                    extremosCirculo[2] = otroCirculo.pos + MotorFisico.Rotate(new Vector2(0,otroCirculo.radio), rot);
                    extremosCirculo[3] = otroCirculo.pos + MotorFisico.Rotate(new Vector2(0,-otroCirculo.radio), rot);

                    foreach (Vector2 maxCirculo in extremosCirculo)
                    {                        
                        if (puntoEnRect(maxCirculo, esquinasPropias, rot))
                        {
                            collisionPoint = maxCirculo;
                            return true;
                        }
                    }
                    return false;
                }
                FormaFisicaRectangulo otroRectangulo = otra as FormaFisicaRectangulo;
                if (otroRectangulo != null) {                    
                    Vector2[] esquinasOtro = new Vector2[4];
                    esquinasOtro[0] = otroRectangulo.pos + new Vector2(-otroRectangulo.ancho / 2f, -otroRectangulo.alto / 2f);
                    esquinasOtro[1] = otroRectangulo.pos + new Vector2(otroRectangulo.ancho / 2f, -otroRectangulo.alto / 2f);
                    esquinasOtro[2] = otroRectangulo.pos + new Vector2(otroRectangulo.ancho / 2f, otroRectangulo.alto / 2f);
                    esquinasOtro[3] = otroRectangulo.pos + new Vector2(-otroRectangulo.ancho / 2f, otroRectangulo.alto / 2f);
                    //Verifico si alguna de mis esquinas está dentro del otro rectangulo
                    foreach (Vector2 esquina in esquinasPropias)
                    {
                        if (puntoEnRect(esquina, esquinasOtro, otroRectangulo.rot))
                        {
                            collisionPoint = esquina;
                            return true;
                        }
                    }
                    //verifico si alguna de sus esquinas está dentro de mi rectangulo
                    foreach (Vector2 esquina in esquinasOtro)
                    {
                        if (puntoEnRect(esquina, esquinasPropias, rot))
                        {
                            collisionPoint = esquina;
                            return true;
                        }
                    }
                    Vector2[] esquinasNoCentros = new Vector2[4];
                    esquinasNoCentros[0] = pos + new Vector2(pos.X, otra.pos.Y);
                    esquinasNoCentros[1] = pos + new Vector2(Math.Max(pos.X, otra.pos.X), Math.Min(pos.Y, otra.pos.Y));
                    
                    foreach (Vector2 esquina in esquinasNoCentros)
                    {
                        //Verifico si alguna de las esquinas formadas del rectángulo formado con los centros está dentro
                        if (puntoEnRect(esquina, esquinasOtro, otroRectangulo.rot) && puntoEnRect(esquina, esquinasPropias, rot))
                        {
                            collisionPoint = esquina;
                            return true;
                        }
                    }                    
                }
                return false;

            }
            catch
            {
                Console.WriteLine("Error Catched");
                return false;
            }
        }
    }
}
