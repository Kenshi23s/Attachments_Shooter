
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FacundoColomboMethods
{
    public enum RaycastType
    {
        Sphere,
        Default
    }

    public enum MustBeOnSight
    {
        Yes,
        No
    }

    public static class ColomboMethods
    {
        //metodos utiles que puede ayudar a la hora de desarrollar el juego

        public static List<T> CloneList<T>(List<T> listToClone)
        {
            List<T> aux = new List<T>();
            for (int i = 0; i < listToClone.Count; i++)
            {
                aux.Add(listToClone[i]);
            }
            return aux;
        }
        /// <summary>
        /// devuelve los componentes que tengas de hijos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Father"></param>
        /// <returns></returns>
        /// 
        public static T[] GetChildrenComponents<T>(Transform Father)
        {
            T[] Components = new T[Father.childCount];
            for (int i = 0; i < Father.childCount; i++)
            {
                var item = Father.transform.GetChild(i).GetComponent<T>();
                if (item != null)
                {
                    Components[i] = item;
                }
            }
            return Components;
        }
        /// <summary>
        /// obtiene todos los componentes de tipo T que haya cerca
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<T> GetNearby<T>(Vector3 pos, float radius)
        {
            List<T> list = new List<T>();
            Collider[] colliders = Physics.OverlapSphere(pos, radius);

            foreach (Collider Object in colliders)
            {
                T item = Object.GetComponent<T>();
                if (item != null)
                {
                    list.Add(item);
                }
            }


            return list;
        }
        /// <summary>
        ///  chequea si tenes algun objeto del tipo T a la vista y en cierto radio
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="Wall"></param>
        /// <returns></returns>
        public static bool CheckNearbyInSigth<T>(Vector3 pos, float radius, LayerMask Wall) where T : MonoBehaviour
        {
            
            Collider[] colliders = Physics.OverlapSphere(pos, radius);
            foreach (Collider Object in colliders)
            {
                var item = Object.GetComponent<T>();
                if (item != null)
                {
                    if (InLineOffSight(pos, item.transform.position, Wall))
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// Obtiene todos los componentes cercanos de tipo "T" que haya a la vista,
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="Wall"></param>
        /// <returns></returns>
        public static List<T> GetALLNearbyInSigth<T>(Vector3 pos, float radius,LayerMask Wall) where T: MonoBehaviour
        {
            List<T> list = new List<T>();
            Collider[] colliders = Physics.OverlapSphere(pos, radius);

            foreach (Collider Object in colliders)
            {
                var item = Object.GetComponent<T>();
                if (item != null)
                {
                    if (InLineOffSight(pos,item.transform.position, Wall))
                    {
                        list.Add(item);
                    }
                  
                }
            }

            return list;
        }

        /// <summary>
        /// chequea si tengo un punto en la vista o esta siendo obstruido por algo
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="maskWall"></param>
        /// <returns></returns>
        public static bool InLineOffSight(Vector3 start, Vector3 end, LayerMask maskWall)
        {
            Vector3 dir = end - start;

            return !Physics.Raycast(start, dir, dir.magnitude, maskWall);
        }

        public static Vector3 CheckForwardRayColision(Vector3 pos, Vector3 dir, float range = 15)
        {
            //aca se guardan los datos de con lo que impacte el rayo
            RaycastHit hit;

            //si el rayo choco contra algo
            if (Physics.Raycast(pos, dir, out hit, range))
            {
                return hit.point;

            }

            return new Vector3(pos.x, pos.y, pos.z + range);

        }

        /// <summary>
        /// obtiene el componente "T" mas cercano sin importar si esta a la vista o no
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objPosition"></param>
        /// <param name="myPos"></param>
        /// <returns></returns>
        /// 
        public static T GetNearest<T>(T[] objPosition, Vector3 myPos) where T : MonoBehaviour
        {
            int nearestIndex = 0;

            float nearestMagnitude = (objPosition[0].transform.position - myPos).magnitude;

            for (int i = 1; i < objPosition.Length; i++)
            {
                float tempMagnitude = (objPosition[i].transform.position - myPos).magnitude;

                if (nearestMagnitude > tempMagnitude)
                {
                    nearestMagnitude = tempMagnitude;
                    nearestIndex = i;
                }

            }

            return objPosition[nearestIndex];
        }

        //public static T CheckNearest<T>(Transform[] objPosition, Vector3 myPos)
        //{
        //    int nearestIndex = 0;

        //    float nearestMagnitude = (objPosition[0].transform.position - myPos).magnitude;

        //    for (int i = 1; i < objPosition.Length; i++)
        //    {
        //        float tempMagnitude = (objPosition[i].transform.position - myPos).magnitude;

        //        if (nearestMagnitude > tempMagnitude)
        //        {
        //            nearestMagnitude = tempMagnitude;
        //            nearestIndex = i;
        //        }

        //    }

        //    return objPosition[nearestIndex].GetComponent<T>();
        //}
        /// <summary>
        /// obtiene el componente "T" mas cercano a la vista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objPosition"></param>
        /// <param name="myPos"></param>
        /// <param name="walls"></param>
        /// <returns></returns>
        public static T GetNearestOnSigth<T>(List<T> objPosition, Vector3 myPos,LayerMask walls) where T : MonoBehaviour
        {
            List<T> listOnSigth = GetWhichAreOnSight(objPosition, myPos,walls);
           
            switch (listOnSigth.Count)
            {
                  
                //ninguno a la vista
                case 0:
                    return null;
                //1 a la vista
                case 1:
                    return listOnSigth[0];
                //mas de  1 a la vista
                default:
                 float nearestMagnitude = (listOnSigth[0].transform.position - myPos).magnitude;
                 int nearestIndex = 0;

                 for (int i = 1; i < listOnSigth.Count; i++)
                 {
                     float tempMagnitude = (listOnSigth[i].transform.position - myPos).magnitude;
                   
                     if (nearestMagnitude > tempMagnitude)
                     {
                         Debug.Log("cambie el mas cercano");

                         nearestMagnitude = tempMagnitude;
                         nearestIndex = i;
                     }

                 }
                  
                 return listOnSigth[nearestIndex];

                    
            }                          
        }

        /// <summary>
        /// devuelve todos los objetos de tipo "T" que esten a la vista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemsPassed"></param>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <param name="wallMask"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<T> GetWhichAreOnSight<T>(List<T> itemsPassed, Vector3 pos,  LayerMask wallMask = default, RaycastType type = RaycastType.Default, float radius=10f) where T : MonoBehaviour
        {
           
            switch (type)
            {
                case RaycastType.Sphere:
                    return FacundoSphereCastAll(pos, itemsPassed,radius, wallMask);                 
                    
                default:
                  return FacundoRaycastAll(pos, itemsPassed);
                    
            }
          
        }
        /// <summary>
        /// devuelve el objeto T si
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pos"></param>
        /// <param name="layer"></param>
        /// <param name="type"></param>
        /// <param name="sphereRadius"></param>
        /// <returns></returns>
        public static T Get_IsOnSight<T>(T item, Vector3 pos, LayerMask layer, RaycastType type = RaycastType.Default, float sphereRadius = 0) where T : MonoBehaviour
        {

            Vector3 dir = item.transform.position - pos;
            switch (type)
            {
                case RaycastType.Sphere:
                    if (FacundoSphereCast<T>(pos, dir, item, sphereRadius,layer))
                    {
                        return item;
                    }
                    break;
                default:
                    if (FacundoRaycast<T>(pos, dir, item))
                    {
                        return item;
                    }

                    break;
            }
            return (T)default;

        }

        // mis raycasts
        static bool FacundoRaycast<T>(Vector3 pos, Vector3 dir, T item) where T : MonoBehaviour
        {
            RaycastHit hit;
            if (!Physics.Raycast(pos, dir, out hit,dir.magnitude))
            {
                return true;
            }
            else
            {
                Transform HitObject = hit.transform;
                if (HitObject == item.transform)
                {
                    return true;
                }
            }
            return false;


        }

        static List<T> FacundoRaycastAll<T>(Vector3 pos, List<T> items) where T : MonoBehaviour
        {
            List<T> Tlist = new List<T>();

            foreach (T tempItem in items)
            {
                Vector3 dir = tempItem.transform.position - pos;
                if (FacundoRaycast(pos, dir, tempItem))
                {
                    Tlist.Add(tempItem);
                    //Debug.Log(" añadi el item" + tempItem.name);
                    continue;
                }
                //Debug.Log("no añadi el item" + tempItem.name);
            }
            if (Tlist.Count>=1)
            {
                return Tlist;
            }
            return new List<T>();


        }

        static bool FacundoSphereCast<T>(Vector3 pos, Vector3 dir, T item, float radius,LayerMask layer) 
        {
        
            if (!Physics.SphereCast(pos, radius, dir,out RaycastHit hit,dir.magnitude,layer))
            {
                
                return true;
            }
            
            return false;


        }

        static List<T> FacundoSphereCastAll<T>(Vector3 pos, List<T> items, float radius,LayerMask wallMask) where T : MonoBehaviour
        {
            List<T> Tlist = new List<T>();

            foreach (T tempItem in items)
            {
                Vector3 dir = tempItem.transform.position - pos;
                if (FacundoSphereCast<T>(pos, dir, tempItem,radius,wallMask))
                {
                    Tlist.Add(tempItem);
                }

            }
            return Tlist;



        }

    }
    
}
