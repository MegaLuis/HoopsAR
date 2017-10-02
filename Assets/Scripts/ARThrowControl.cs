using UnityEngine;
using UnityEngine.UI;
using ProgressBar;

public class ARThrowControl : MonoBehaviour 
{
    //Progress bar script
    public ProgressRadialBehaviour radialBar;

    //public Text text;
    public GameObject ball;
	public Vector2 sensivity = new Vector2(8f, 100f);

	public float speed = 5f;
	public float resetBallAfterSeconds = 0.5f;
	public float lerpTimeFactorOnTouch = 7f;
	public float cameraNearClipPlaneFactor = 7.5f;

	public bool isThrowBackAvailable = false;

	// if (isFullPathThrow == false)
	// sensivity = new Vector2(100f, 100f);
	// speed = 45f;
	public bool isFullPathThrow = true;

    //Value that will apply force to ball
    public float throwPower;
    //Tells the throwPower meter to go up or down depending on true/false
    //Up true, down false
    public bool meterDirection;

	private Vector3 direction;

	private Vector3 inputPositionCurrent;


	private Vector3 newBallPosition;
	private BallControl ballControl;
	private Rigidbody _rigidbody;
	private RaycastHit raycastHit;

	private bool isThrown; 

	private bool isInputBegan = false;
	private bool isInputEnded = false;
	private bool isInputLast = false;

    bool isTouching = false;

	void Start() 
	{
        radialBar = GameObject.Find("RadialBar").GetComponent<ProgressRadialBehaviour>();
        ball = this.gameObject;
		_rigidbody = GetComponent<Rigidbody> ();
		ballControl = GetComponent<BallControl>();

		Reset ();
	}

	void Update() 
	{
		#if UNITY_EDITOR

            isInputBegan = Input.GetMouseButton(0);
			isInputEnded = Input.GetMouseButtonUp(0);
			isInputLast = Input.GetMouseButton(0);

			inputPositionCurrent = Input.mousePosition;

		#else

			isInputBegan = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
			isInputEnded = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
			isInputLast = Input.touchCount == 1;

			inputPositionCurrent = Input.GetTouch (0).position;

		#endif

		//if (isHolding)
		//	OnTouch ();

		
			
		if (isInputBegan)
		{
            if (Physics.Raycast(Camera.main.ScreenPointToRay(inputPositionCurrent), out raycastHit, 100f))
            {
                if (raycastHit.transform == transform)
                {
                    isTouching = true;

                    Debug.Log("Throw Power: " + throwPower);
                    radialBar.SetFillerSizeAsPercentage(throwPower * 100f);
                    //This is where you begin charging ball.
                    if (throwPower >= 1)
                    {
                        throwPower = 1;
                        meterDirection = false;
                    }
                    else if (throwPower <= 0)
                    {
                        throwPower = 0;
                        meterDirection = true;
                    }

                    if (meterDirection)
                    {
                        throwPower += Time.deltaTime;
                    }
                    else
                    {
                        throwPower -= Time.deltaTime;
                    }

                }
            }
		}

		//This is where you apply the tossPower to the ball, call event.
		if (isInputEnded)
		{
            if(isTouching)
            {
                Throw(throwPower);
                isTouching = false;
                radialBar.Value = 0;
                throwPower = 0;
            }
		}
	}

	void Reset()
	{
		CancelInvoke ();

		transform.position = Camera.main.ViewportToWorldPoint (
			new Vector3 (0.5f, 0.1f, Camera.main.nearClipPlane * cameraNearClipPlaneFactor)
		);
		
		newBallPosition = transform.position;

		

		_rigidbody.useGravity = false;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;

		transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		//transform.SetParent (Camera.main.transform);
	}


	void Throw(float ballPower) 
	{
		ballControl.SetThrown();

		_rigidbody.useGravity = true;

        direction = new Vector3 (0f, 0f, ballPower);
		direction = Camera.main.transform.TransformDirection (direction);

        _rigidbody.AddForce((direction + Vector3.up) * 100);

		

		Invoke ("CreateNewBall", resetBallAfterSeconds);
	}

    void CreateNewBall(){
        Instantiate(ball);
        Destroy(gameObject, 3f);
    }
}