using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Handler), typeof(Rigidbody),typeof(PausableObject))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform cam;
    Rigidbody rb;
    PausableObject pauseOBJ;
    #region look
    [SerializeField, Range(0f, 10f)]
    float lookVerticalSensitivity, lookHorizontalSensitivity = 1;
    float lookVertical, lookHorizontal;

    [SerializeField, Range(0, 90)] float lookUpAngle = 90;
    [SerializeField, Range(-90, 0)] float lookDownAngle = -90;
    #endregion

    [SerializeField, Range(0, 20f)]
    float walkSpeed = 8;

    [SerializeField, Range(0, 20f)]
    float runSpeed = 14f;

    [SerializeField, Range(0, 20f)]
    float stealthSpeed = 4;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 37.5f, maxAirAcceleration = 20f;
    [SerializeField, Range(0f, 4f), Tooltip("The jump height when player is under a gravity of 9.81 N")]
    float jumpHeight = 1.5f;
    float jumpForce;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 45f, maxStairsAngle = 50f;

    float minGroundDotProduct, minStairsDotProduct;

    Vector3 inputMoveDirection;
    Vector3 velocity;
    public Vector3 Velocity => velocity;

    Vector3 desiredVelocity;
    bool desiredJump;

    bool desiredRun;
    bool desiredStealth;

    [SerializeField, Min(0f)]
    float probeDistance = 0.5f;
    [SerializeField]
    LayerMask probeMask = -1, stairsMask = -1;

    // Si bien no es esencial, contamos cuántos puntos de contacto con el suelo tenemos en lugar de solo rastrear si hay al menos uno.
    // Además de la optimización en UpdateState, el recuento de contactos con el suelo también podría ser útil para la depuración.
    int groundContactCount, steepContactCount;
    public bool OnGround => groundContactCount > 0;
    public bool OnSteep => steepContactCount > 0;

    int stepsSinceLastGrounded, stepsSinceLastJump;

    Vector3 groundContactNormal, steepNormal;

    Player_Handler player;

    [SerializeField] bool holdShiftToRun = true;

    private void Awake()
    {
        pauseOBJ = GetComponent<PausableObject>();

        pauseOBJ.onPause += () => StartCoroutine(StopMoving());
        player = GetComponent<Player_Handler>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // !!! Esta logica deberia ir en otro lado.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        OnValidate();
    }

    IEnumerator StopMoving()
    {
        enabled = false;
        Vector3 actualVelocity = rb.velocity;
        rb.velocity = Vector3.zero;

        bool gravity = rb.useGravity;
        rb.useGravity = false;
        Debug.LogWarning("Pause");
        yield return new WaitWhile(ScreenManager.IsPaused);

        enabled = true;

        rb.velocity = actualVelocity;
        rb.useGravity = gravity;
    }

    // Se llama a esta función cuando se carga el script o cuando se modifica un valor en el inspector (solo se llama en el editor)
    public void OnValidate()
    {
        // Estas variables son una optimizacion para chequear si el jugador puede caminar por la superficie en la que se encuentra.
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);

        // Es mas intuitivo setear un altura de salto ("jumpHeight") que una fuerza de salto.
        // Por eso, la siguiente ecuacion calcula la velocity necesaría para llegar a la altura de salto deseada.
        jumpForce = Mathf.Sqrt(2f * Physics.gravity.magnitude * jumpHeight);
    }

    public void Update()
    {
        GetInputs();

        float speed = desiredStealth ? stealthSpeed : desiredRun ? runSpeed : walkSpeed;
        desiredVelocity = inputMoveDirection * speed;

        // Rotacion horizontal. Gira solamente la camara. (La cabeza del personaje)
        transform.rotation = Quaternion.Euler(0, lookHorizontal, 0);
    }

    void LateUpdate()
    {
        // Rotacion vertical. Gira solamente la camara. (La cabeza del personaje)
        cam.localRotation = Quaternion.Euler(lookVertical, 0, 0);
    }

    void FixedUpdate()
    {
        UpdateState();


        if (OnGround || inputMoveDirection != Vector3.zero)
        {
            AdjustVelocity();
        }

        // Salto
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        // Aplicar gravedad
        velocity += Physics.gravity * Time.deltaTime;

        rb.velocity = velocity;

        ClearState();
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void Jump()
    {
        if (OnGround)
        {
            stepsSinceLastJump = 0;

            // La altura del salto es una indicación de qué tan alto saltamos cuando estamos en terreno plano o solo en el aire.
            // Si estamos en una pendiente, el salto debería ser en la direccion a la que apunta la normal de esta.
            // Estos saltos no alcanzarán tanta altura, pero afectarán la velocidad horizontal.
            velocity += jumpForce * groundContactNormal;
        }
    }

    // Consigue los inputs y los guarda en una variable. Se debe llamar en el update.
    void GetInputs()
    {
        // Conseguir input de direccion de movimiento;

        inputMoveDirection = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward;

        inputMoveDirection.Normalize();

        bool desiredForward = Vector3.Dot(inputMoveDirection, transform.forward) > 0;

        desiredStealth = Input.GetKey(KeyCode.LeftControl);


        // ¿Por qué '|=' en vez de '='?
        // El input de salto se toma en el Update, pero el salto se ejecuta dentor del FixedUpdate.
        // Existe la posibilidad de que no se invoke el FixedUpdate en un frame determinado. En ese caso,
        // si usaramos '=', desiredJump se setearía a falso en el siguiente Update, y el jugador nunca saltaría.
        // Esto se puede prevenir combinando el chequeo con su valor previo via la asignación booleana OR (|=)
        // De esta manera, desiredJump permanece verdadero hasta que explicitamente lo seteamos a falso. 
        desiredJump |= Input.GetButtonDown("Jump");

        // Si la tecla esta apretada, el jugador quiere correr.
        if (holdShiftToRun)
        {
            desiredRun = Input.GetKey(KeyCode.LeftShift) && desiredRun;
        }
        else
        {
            if (!desiredForward)
            {
                desiredRun = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                desiredRun = !desiredRun;
            }
        }


        lookHorizontal += Input.GetAxisRaw("Mouse X") * lookHorizontalSensitivity;

        lookVertical -= Input.GetAxisRaw("Mouse Y") * lookVerticalSensitivity;
        lookVertical = Mathf.Clamp(lookVertical, lookDownAngle, lookUpAngle);
    }

    void UpdateState()
    {
        stepsSinceLastGrounded++;
        stepsSinceLastJump++;
        velocity = rb.velocity;

        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            // Normalizar la acumulacion de normales de contacto para convertirla en un vector normal adecuado.
            if (groundContactCount > 1)
            {
                groundContactNormal.Normalize();
            }
        }
        else
        {
            groundContactNormal = Vector3.up;
        }
    }

    public void EvaluateCollision(Collision collision)
    {
        float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            // minGroundDotProduct es un calculo que proviene de maxGroundAngle.
            // Aca se esta comparando si el ángulo entre el jugador y la superficie es mayor o menor a maxGroundAngle.
            Vector3 normal = collision.GetContact(i).normal;

            if (normal.y >= minDot)
            {
                groundContactCount++;

                // En el caso que de haya multiples contactos con el piso. ¿Qué dirección es la mejor? No hay una.
                // Tiene más sentido combinarlas a todas en una sola normal que represente un plano de tierra promedio.
                // Para hacer eso tenemos que acumular los vectores normales.
                groundContactNormal += normal;
            }
            // Si no tenemos un contacto con el suelo, cericar si es un contacto empinado.
            // El producto escalar de una pared perfectamente vertical debería ser cero,
            // pero seamos un poco indulgentes y aceptemos todo lo que esté por encima de -0,01
            else if (normal.y > -0.01f)
            {
                steepContactCount++;
                steepNormal += normal;
            }
        }
    }

    // Ajusta la velocidad para que apunte en direccion de la pendiente que se tiene enfrente
    void AdjustVelocity()
    {
        // Determinar los ejes X y Z proyectados proyectando los ejes derecho y delantero en el plano de contacto.
        Vector3 xAxis = transform.right.ProjectDirectionOnPlane(groundContactNormal);
        Vector3 zAxis = transform.forward.ProjectDirectionOnPlane(groundContactNormal);

        // Ahora podemos proyectar la velocidad actual en ambos vectores para obtener las velocidades relativas X y Z.
        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        // Aceleración
        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;

        // ¿Por qué no Time.fixedDeltaTime?
        // Cuando se invoca FixedUpdate, Time.deltaTime equivale a Time.fixedDeltaTime.
        // Es decir, es lo mismo.
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, Vector3.Dot(desiredVelocity, transform.right), maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, Vector3.Dot(desiredVelocity, transform.forward), maxSpeedChange);


        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

        // Prevenir deslizamiento hacia abajo en pendientes.
        if (OnGround && velocity.sqrMagnitude < 0.01f)
        {
            velocity -= Physics.gravity * Time.deltaTime;
        }

    }

    void ClearState()
    {
        // Cada paso de la física comienza con la invocación de todos los métodos FixedUpdate,
        // después de lo cual PhysX hace lo suyo y, al final, se invocan los métodos de colisión.
        // Entonces, cuando se invoque FixedUpdate, OnGround se habrá establecido en verdadero durante
        // el último paso si hubo colisiones activas. Todo lo que tenemos que hacer para mantener OnGround
        // válido es volver a configurarlo como falso al final de FixedUpdate.
        // Lo hacemos indirectamente, setando la cantidad de contactos con el piso a 0.
        groundContactCount = steepContactCount = 0;
        groundContactNormal = steepNormal = Vector3.zero;
    }

    bool SnapToGround()
    {
        // SnapToGround solo se invoca cuando no estamos grounded, por lo que la cantidad de pasos desde la última conexión
        // al piso es mayor a cero. Pero solo debemos intentar snappear una vez directamente después de perder el contacto.
        // Por lo tanto, cuando la cantidad de pasos es mayor que uno, debemos abortar.

        // Además, podemos abortar SnapToGround cuando es demasiado pronto después de un salto.
        // Debido a la demora de los datos de colisión, todavía se nos considera grounded el paso después de que se inició el salto.
        // Así que debemos abortar si estamos dos o menos pasos después de un salto.
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
        {
            return false;
        }


        // Solo queremos snappear al piso cuando hay suelo debajo al que adherirse.
        if (!Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit, probeDistance, probeMask))
        {
            return false;
        }

        // Si el Raycast golpeó algo, entonces debemos verificar si cuenta como suelo.
        if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        // Si no hemos abortado en este punto, simplemente perdimos el contacto con el suelo,
        // pero todavía estamos sobre el suelo, por lo que nos snappeamos a él.
        groundContactCount = 1;
        groundContactNormal = hit.normal;

        // Ahora nos consideramos grounded, aunque todavía estamos en el aire.
        // El siguiente paso es ajustar nuestra velocidad para alinearnos con el suelo.
        float speed = velocity.magnitude;
        float dot = Vector3.Dot(velocity, hit.normal);

        // En esta instancia todavía estamos flotando sobre el suelo, pero la gravedad se encargará de bajarnos hacia la superficie.
        // De hecho, la velocidad ya podría apuntar un poco hacia abajo, en cuyo caso, realinearla retrasaría la convergencia hacia el suelo.
        // Por lo tanto, solo debemos ajustar la velocidad cuando el producto escalar de esta y la superficie normal sea positivo.
        if (dot > 0f)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }

        return true;
    }

    // Devuelve el mínimo apropiado para una capa dada
    float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ?
            minGroundDotProduct : minStairsDotProduct;
    }

    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();

            if (steepNormal.y >= minGroundDotProduct)
            {
                groundContactCount = 1;
                groundContactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }


}

public static class HelperMethods
{
    // ¿Por qué no usar Vector3.ProjectOnPlane?
    // Ese método hace lo mismo pero no asume que el vector normal proporcionado es de longitud unitaria;
    // Divide el resultado por la longitud al cuadrado de la normal, que siempre es 1, por lo que no es necesario.
    public static Vector3 ProjectDirectionOnPlane(this Vector3 direction, Vector3 planeNormal)
    {
        return (direction - planeNormal * Vector3.Dot(direction, planeNormal)).normalized;
    }
}
