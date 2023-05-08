using TMPro;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class FloatingText : MonoBehaviour
{
    [System.Serializable]
    public struct FloatingTextParam
    {
        [Range(0f, 1f)]
        public float x_Spread;
        [Range(0f, 1f)]
        public float y_Spread;

        [Range(0.1f, 1), Tooltip("la velocidad con la que se mueve el texto")]
        public float speed;

        [Range(0.1f, 1f), Tooltip("la velocidad con la que se desvanece el texto")]
        public float fadeSpeed;

        public int sortingOrderRead => _sortingOrder;
        int _sortingOrder;

        public void IncreaseSortingOrder() => _sortingOrder++;
       

    }
    FloatingTextParam _textParameters;

    public TextMeshPro _popUpText;

    Vector3 _textForce, _initialPos;   
   
    Action<FloatingText> pool_ReturnMethod;




    #region Initialize
    //Al crearse                                                     // MI metodo Return es igual al metodo Return que me pasaron por parametro
    public void Configure(Action<FloatingText> pool_ReturnMethod) => this.pool_ReturnMethod = pool_ReturnMethod;
  
 

    /// <summary>
    /// este metodo es el encargado de preparar a el texto para que haga todos 
    /// los comportamientos necesarios para su correcta funcion
    /// </summary>
    public void InitializeText(string TextValueDamage, Vector3 pos, FloatingTextParam parametersPass)
    {
        //inital pos lo uso para fijarme(en el gizmos) en que direccion va el texto 
        //transform.position = pos;
       
       _initialPos = transform.position =  pos;
       _textParameters = parametersPass;
       SetText(TextValueDamage,parametersPass.sortingOrderRead);   
       SetRandomForce(parametersPass.x_Spread,parametersPass.y_Spread);

    }
 
    void SetText(string value,int sortOrder)
    {
        // esto es para que el texto actual tenga prioridad de renderizado sobre los anteriores
        _popUpText.sortingOrder = sortOrder;

        //aplico el int pasado al texto
        _popUpText.text = value.ToString();
        // se pone el alpha al maximo por las dudas de que no lo estuviera
        _popUpText.alpha = 255f;
        _popUpText.gameObject.name = ("Text Damage  " + value );
    }

    void SetRandomForce(float RandomX,float RandomZ)
    {
        _textParameters.x_Spread = SetRandomValue(RandomX);
        _textParameters.y_Spread = SetRandomValue(RandomZ);
        _textForce = new Vector3(_textParameters.x_Spread, _textParameters.y_Spread);
    }

    float SetRandomValue(float Randomness) => Random.Range(-Mathf.Abs(Randomness), Mathf.Abs(Randomness));
    #endregion

    #region Update 
    void Update()
    {
        //se le suma al transform una fuerza para que se mueva a lo largo del tiempo hasta que
        //el "Alpha" del texto llegue a 0 (va de 0 a 1, no de 0 a 255)

        this.transform.position += _textForce.normalized * Time.deltaTime * 5;
        float A = SubstractAlpha(_textParameters.fadeSpeed);
        if (A == 0) GoToPool();



    }

    private void LateUpdate() => LookCamera(Camera.main.transform.position, transform.position);

    void LookCamera(Vector3 cam, Vector3 posToChange) => transform.LookAt(new Vector3(posToChange.x, cam.y, cam.z));

    float SubstractAlpha(float Decreasespeed)
    {
        _popUpText.alpha = Mathf.Clamp(_popUpText.alpha, 0f, 1f);
        _popUpText.alpha -= Decreasespeed * Time.deltaTime;
        _popUpText.alpha = Mathf.Clamp(_popUpText.alpha, 0f, 1f);
        float t = _popUpText.alpha;
        return t;

       
    }
    #endregion
   
    void GoToPool()
    {     
        _initialPos = _textForce = Vector3.zero;
        pool_ReturnMethod(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_initialPos, _initialPos+_textForce);
    }
}
