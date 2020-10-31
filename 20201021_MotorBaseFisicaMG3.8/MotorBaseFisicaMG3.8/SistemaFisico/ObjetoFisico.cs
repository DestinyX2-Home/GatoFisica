using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorBaseFisicaMG38.SistemaDibujado;
using System.Diagnostics;

namespace MotorBaseFisicaMG38.SistemaFisico
{
    public class ObjetoFisico
    {
        public Dibujable dibujable;
        public bool isStatic = false;
        public bool isTrigger = false;
        public float absorcionChoque = 0.9f;
        public float rot { get { return dibujable.rot; } set { dibujable.rot = value; } }
        public float rotFuerza;
        public float rotVel;
        public float rotRoce = 1f;
        public Vector2 pos { get { return dibujable.pos; } set{dibujable.pos = value; } }
        private Vector2 prevPos;
        public Vector2 vel { get; set; }
        public Vector2 prevVel { get; private set; }
        private Vector2 acel;
        private Vector2 prevAcel;
        private Vector2 forcedAcel;
        public bool isColliding;
        public delegate void OnCollisionDelegate(Object obj);
        public delegate Object GetObjectDelegate();
        public OnCollisionDelegate OnCollision;
        public GetObjectDelegate GetObject;
        public ObjetoFisico lastCollision = null;

        public class FFOffset
        {
            public FormaFisica ff;
            public Vector2 offset;
        }
        public List<FFOffset> formasFisicasOffset = new List<FFOffset>();
        public float masa = 1;
        public ObjetoFisico(Dibujable dibujable, float masa = 1)
        {
            this.dibujable = dibujable;
            this.masa = masa;
            MotorFisico.agregarObjetoFisico(this);
            prevPos = pos;
            prevVel = vel;
        }
        public void AddVelocity(Vector2 newVel)
        {
                vel += newVel;
        }
        public void SumarVelocidadAngular(float rotVel)
        {
            this.rotVel += rotVel;
        }
        public void SetVelocity(Vector2 newVel)
        {
            if (!isColliding)
            {
                vel = newVel;
            }
        }
        public void agregarFFCirculo(float radio, Vector2 offsetPos)
        {
            FormaFisica ff = new FormaFisicaCirculo(radio);
            FFOffset ffo = new FFOffset();
            ffo.ff = ff;
            ffo.offset = offsetPos;

            formasFisicasOffset.Add(ffo);
        }
        public void agregarFFRectangulo(float ancho,float alto, Vector2 offsetPos)
        {
            FormaFisica ff = new FormaFisicaRectangulo(ancho, alto);
            FFOffset ffo = new FFOffset();
            ffo.ff = ff;
            ffo.offset = offsetPos;

            formasFisicasOffset.Add(ffo);
        }
        public bool Colisiona(ObjetoFisico otro, out Vector2 collisionPoint)
        {
            collisionPoint = Vector2.Zero;          
            foreach (FFOffset ffo in formasFisicasOffset)
            {
                //Esta linea actualiza la posición de la forma física
                ffo.ff.pos = pos + Rotate(ffo.offset, rot);
                ffo.ff.rot = rot;

                //Se verifica la colición entre formas físicas de este objeto físico y otro.
                foreach(FFOffset otro_ffo in otro.formasFisicasOffset)
                {
                    otro_ffo.ff.pos = otro.pos + Rotate(otro_ffo.offset, otro.rot);
                    otro_ffo.ff.rot = otro.rot;
                    if (otro_ffo.ff.colisiona(ffo.ff, out collisionPoint))
                    {
                        lastCollision = otro;
                        return true;                      
                    }
                }
            }
            return false;
        }
        public void AplicaFuerza(Vector2 fuerza, float deltaTiempoSeg, bool forceVel = false, bool rotar = false)
        {
            AplicaFuerza(fuerza, deltaTiempoSeg, Vector2.Zero, forceVel, rotar);
        }
        public void AplicaFuerza(Vector2 fuerza, float deltaTiempoSeg, Vector2 puntoCol, bool forceVel = false, bool rotar = true)
        {
            if (forceVel)
            {
                forcedAcel += (fuerza / masa);              
                if (rotar)
                {
                    Vector2 puntoFuerzaRot = puntoCol - pos;
                    float sumVel = (float)MotorFisico.AngleBetween(fuerza, puntoFuerzaRot);                    
                    sumVel *= fuerza.Length();                    
                    rotVel += sumVel/3;                    
                }
                return;
            }
            acel += (fuerza / masa);
            
        }
        public void Update(float deltaTiempoSeg)
        {
            prevPos = pos;
            prevVel = vel;
            if (isColliding)
            {
                prevVel = vel += forcedAcel;// + (acel) * deltaTiempoSeg;
            }
            else
            {
                vel += (acel) * deltaTiempoSeg;
            }
            
            if (vel.LengthSquared() < .005f)
            {
                //don't move
            }
            else
            {
                pos += ((prevVel + vel) / 2) * deltaTiempoSeg * MotorFisico.WorldSizeScale;
            }

            vel -= vel * (MotorFisico.RoceAire)*deltaTiempoSeg;            
            forcedAcel = acel = Vector2.Zero;
            rot += rotVel * deltaTiempoSeg;
            rotVel -= rotVel * rotRoce * deltaTiempoSeg;
            if (lastCollision != null)
            {
                if (Math.Sqrt((pos - lastCollision.pos).LengthSquared()) > dibujable.ancho / 2)
                {
                    lastCollision = null;
                }
            }
        }
        public void Destruir()
        {
            MotorFisico.removerObjetoFisico(this);
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
