using UnityEngine;
using UnityEngine.SceneManagement;

public class BallPredictionScript : MonoBehaviour
{
    public GameObject ball;
    public int maxItarations = 50;

    private Scene mainScene, predictionScene;
    private PhysicsScene mainPhysicsScene, predictionPhysicsScene;

    private GameObject predictionBall;
    private LineRenderer liner;
    private Vector3 lastBallPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Physics.autoSimulation = false;

        mainScene = SceneManager.GetActiveScene();
        mainPhysicsScene = mainScene.GetPhysicsScene();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene();

        liner = ball.AddComponent<LineRenderer>();
        liner.startColor = Color.green;
        liner.endColor = Color.green;
        liner.startWidth = 0.2f;
        liner.endWidth = 0.2f;
    }

    void FixedUpdate()
    {
        if (mainPhysicsScene.IsValid())
        {
            mainPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    public void predictBallPath(float throwBallForce)
    {
        if (mainPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {

            if (predictionBall == null)
            {
                predictionBall = Instantiate(ball);
                predictionBall.GetComponent<Rigidbody>().isKinematic = false;
                predictionBall.GetComponent<Rigidbody>().useGravity = true;
                SceneManager.MoveGameObjectToScene(predictionBall, predictionScene);
            }

            if (ball.transform.position == lastBallPosition)
            {
                return;
            }
            predictionBall.transform.position = ball.transform.position;
            lastBallPosition = ball.transform.position;

            Vector3 forward = transform.forward;
            predictionBall.GetComponent<Rigidbody>().AddForce(forward * throwBallForce, ForceMode.VelocityChange);
            predictionBall.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 10f, 10f);

            liner.positionCount = 0;
            liner.positionCount = maxItarations;

            for (int i = 0; i < maxItarations; i++)
            {
                predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
                liner.SetPosition(i, predictionBall.transform.position);
            }

            Destroy(predictionBall);
        }
    }

    public void reset()
    {
        if (predictionBall != null)
        {
            Destroy(predictionBall);
        }
        liner.positionCount = 0;
    }
}
