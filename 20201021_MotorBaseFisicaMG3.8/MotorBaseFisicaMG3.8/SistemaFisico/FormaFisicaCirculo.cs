using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorBaseFisicaMG38.SistemaFisico
{
    public class FormaFisicaCirculo : FormaFisica
    {
        public float radio;
        public FormaFisicaCirculo(float radio)
        {
            this.radio = radio;
        }
        public override bool colisiona(FormaFisica otra, out Vector2 collisionPoint)
        {
            collisionPoint = Vector2.Zero;
            try
            {
                FormaFisicaCirculo otroCirculo = otra as FormaFisicaCirculo;
                if (otroCirculo != null)
                {
                    collisionPoint = (otroCirculo.pos + pos) / 2;
                    float distanciaCuadrada = (otroCirculo.pos - this.pos).LengthSquared();
                    float sumaRadios = this.radio + otroCirculo.radio;
                    if (distanciaCuadrada < sumaRadios * sumaRadios)
                    {
                        return true;
                    }
                    return false;
                }
                FormaFisicaRectangulo otroRectangulo = otra as FormaFisicaRectangulo;
                if (otroRectangulo != null)
                {
                    return otroRectangulo.colisiona(this, out collisionPoint);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
