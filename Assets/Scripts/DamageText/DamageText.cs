using TMPro;
using UnityEngine;
using System;

public class FloatingText : MonoBehaviour
{
    [System.Serializable]
    public struct FloatingTextParam
    {
        [Range(0f, 1f)]
        public float x_Spread;
        [Range(0f, 1f)]
        public float y_Spread;

        [Range(0, 1), Tooltip("la velocidad con la que se mueve el texto")]
        public float speed;

        [Range(0, 1), Tooltip("la velocidad con la que se desvanece el texto")]
        public float fadeSpeed;

        public int sortingOrderRead=> _sortingOrder;
        int _sortingOrder;

        public void IncreaseSortingOrder()
        {
            _sortingOrder++;
        }



    }
    FloatingTextParam _textParameters;

    public TextMeshPro _DmgText;

    Vector3 _TextForce, _initialPos;   
   
    Action<FloatingText> pool_ReturnMethod;




    #region Initialize
    //Al crearse
    public void Configure(Action<FloatingText> ReturnMethod)
    {      
      
        // MI metodo Return es igual al metodo Return que me pasaron por parametro
        this.pool_ReturnMethod = ReturnMethod;
        
    }
 

    /// <summary>
    /// este metodo es el encargado de preparar a el texto para que haga todos 
    /// los comportamientos necesarios para su correcta funcion
    /// </summary>
    public void InitializeText(int TextValueDamage, Vector3 pos, FloatingTextParam parametersPass)
    {
        //inital pos lo uso para fijarme(en el gizmos) en que direccion va el texto 
        //transform.position = pos;
       Vector3 TextForce = Vector3.zero;
       _initialPos = transform.position =  pos;
       _textParameters = parametersPass;
       SetText(TextValueDamage,parametersPass.sortingOrderRead);   
       SetRandomForce(parametersPass.x_Spread,parametersPass.y_Spread);

    }
 
    void SetText(int value,int sortOrder)
    {
        // esto es para que el texto actual tenga prioridad de renderizado sobre los anteriores
        _DmgText.sortingOrder = sortOrder;

        //aplico el int pasado al texto
        _DmgText.text = value.ToString();
        // se pone el alpha al maximo por las dudas de que no lo estuviera
        _DmgText.alpha = 255f;
        _DmgText.gameObject.name = ("Text Damage  " + value );
    }

    void SetRandomForce(float RandomX,float RandomZ)
    {
        _textParameters.x_Spread = SetRandomValue(RandomX);
        _textParameters.y_Spread = SetRandomValue(RandomZ);
        _TextForce = new Vector3(_textParameters.x_Spread, _textParameters.y_Spread);
    }

    float SetRandomValue(float Randomness) => UnityEngine.Random.Range(-Mathf.Abs(Randomness), Mathf.Abs(Randomness));
    #endregion

    #region Update 
    void Update()
    {
        //se le suma al transform una fuerza para que se mueva a lo largo del tiempo hasta que
        //el "Alpha" del texto llegue a 0 (va de 0 a 1, no de 0 a 255)

        this.transform.position += _TextForce.normalized * Time.deltaTime * 5;
        float A = SubstractAlpha(_textParameters.fadeSpeed);
        if (A == 0)
            GoToPool();


    }

    private void LateUpdate() => LookCamera(Camera.main.transform.position, transform.position);

    void LookCamera(Vector3 cam, Vector3 posToChange) => transform.LookAt(new Vector3(posToChange.x, cam.y, cam.z));

    float SubstractAlpha(float Decreasespeed)
    {
        _DmgText.alpha = Mathf.Clamp(_DmgText.alpha, 0f, 1f);
        _DmgText.alpha -= Decreasespeed * Time.deltaTime;
        _DmgText.alpha = Mathf.Clamp(_DmgText.alpha, 0f, 1f);
        float t = _DmgText.alpha;
        return t;

       
    }
    #endregion
   
    void GoToPool()
    {
        _TextForce = Vector3.zero;
        _initialPos = Vector3.zero;
        pool_ReturnMethod(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_initialPos, _initialPos+_TextForce);
    }
}
