using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorBaseFisicaMG38.SistemaDibujado;
using MotorBaseFisicaMG38.SistemaFisico;

namespace MotorBaseFisicaMG38.SistemaGameObject
{    
    public class UTGameObject
    {
        public ObjetoFisico objetoFisico;
        public Dibujable dibujable;
        public float rot { get { return dibujable.rot; } set { dibujable.rot = value; } }
        public enum FF_form { Circulo, Rectangulo,DosCirculos};
        public event EventHandler Click;
        public UTGameObject(string imagen, Vector2 pos, float escala, FF_form forma, bool isStatic = false)
        {
            dibujable = new Dibujable(imagen, pos, escala);
            objetoFisico = new ObjetoFisico(dibujable);
            if (forma == FF_form.Circulo)
            {
                objetoFisico.agregarFFCirculo(dibujable.ancho/2f, new Vector2(0, 0));
            }
            else if(forma == FF_form.Rectangulo)
            {
                objetoFisico.agregarFFRectangulo(dibujable.ancho, dibujable.alto, new Vector2(0, 0));
            }
            else if (forma == FF_form.DosCirculos)
            {
                objetoFisico.agregarFFCirculo(dibujable.ancho / 2f, new Vector2(0,dibujable.alto/2-dibujable.ancho/2));
                objetoFisico.agregarFFCirculo(dibujable.ancho / 2f, new Vector2(0,-dibujable.alto/2 + dibujable.ancho/2));
            }
            objetoFisico.isStatic = isStatic;
            objetoFisico.OnCollision = OnCollision;
            objetoFisico.GetObject = GetObject;


            UTGameObjectsManager.suscribirObjeto(this);
        }

        private void OnCollision(Object other)
        {
            UTGameObject otherUTG = other as UTGameObject;
            OnCollision(otherUTG);
        }
        public Object GetObject()
        {
            return this as Object;
        }

        public virtual void OnCollision(UTGameObject other)
        {
            Click?.Invoke(this, new EventArgs());
        }
        public void Destroy()
        {
            UTGameObjectsManager.DestruirObjeto(this);
        }
        public void OnDestroy()
        {
            dibujable.Destruir();
            objetoFisico.Destruir();
        }
        public virtual void Update(GameTime gameTime)
        {

        }

    }
}
