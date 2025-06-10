using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CanonShooter
{
    public class CameraControl : MonoBehaviour
    {


        private float m_ShakeTimer;
        private float m_ShakeArc;
        private float m_ShakeRadius = 1;

        public float m_MinZ = 0;


        [HideInInspector]
        public Transform m_Target;
        [HideInInspector]
        public Vector3 m_TargetPoint;
        public float m_TargetDistance = 10;
        public Vector3 m_TargetDirection;
        [SerializeField]
        private Transform m_TargetPointTransform;

        public Vector3 m_InitPosition;

        public static CameraControl m_Main;

        public Camera m_MyCamera;

        public Transform m_BossTarget;

        public Transform m_BackBlock;

        public bool m_IsAiming = false;

        public AnimationCurve m_BlendCurve1;

        [HideInInspector]
        public Vector3 m_CameraBottomPosition;
        [HideInInspector]
        public Vector3 m_CameraTopPosition;

        Vector3 Direction;
        // Start is called before the first frame update
        void Awake()
        {
            m_Main = this;
            m_MyCamera = GetComponent<Camera>();
        }

        void Start()
        {
            Direction = transform.forward;

            //Player p = m_Target.gameObject.GetComponent<Player>();
            //    p.m_MyCamera = this;
            //m_MinZ = PlayerChar.m_Current.transform.position.z + 10;
            m_CameraBottomPosition = new Vector3(0, 0, -100);
            m_CameraTopPosition = new Vector3(0, 0, -100);
            m_InitPosition = transform.position;
            m_IsAiming = false;

            m_TargetPoint = Vector3.zero;
            m_TargetDirection = Quaternion.Euler(70, 0, 0) * Vector3.forward;
            m_TargetDistance = 30;
            //float distance = 80;
            //Direction = Quaternion.Euler(40, 0, 0) * Vector3.forward;
            //Vector3 targetPosition = PlayerChar.m_Current.transform.position;
            //targetPosition.z = m_MinZ;
            //targetPosition.x = 0.4f * targetPosition.x;
            //transform.position =  targetPosition + -distance * Direction;// - distance// * m_FaceVector;
            //transform.forward =  Direction;
        }

        void Update()
        {
            m_ShakeTimer -= Time.deltaTime;
            //ShakeArc += 100 * Time.deltaTime;

            if (m_ShakeTimer <= 0)
                m_ShakeTimer = 0;

            Vector3 ShakeOffset = Vector3.zero;
            float shakeSin = Mathf.Cos(30 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            float shakeCos = Mathf.Sin(50 * Time.time) * Mathf.Clamp(m_ShakeTimer, 0, 0.5f);
            ShakeOffset = new Vector3(m_ShakeRadius * shakeCos, m_ShakeRadius * shakeSin, 0);


            //if (PlayerControl.MainPlayerController.MyPlayerChar.transform.position.z+8>m_MinZ)
            //{
            //    m_MinZ = PlayerControl.MainPlayerController.MyPlayerChar.transform.position.z+8;
            //}

            //float distance = 80;
            //Direction = Quaternion.Euler(40, 0, 0) * Vector3.forward;
            //Vector3 targetPosition = PlayerChar.m_Current.transform.position;
            //targetPosition.z = m_MinZ;
            //targetPosition.x = 0.4f * targetPosition.x;

            //if (m_BossTarget!=null)
            //{
            //    targetPosition = PlayerChar.m_Current.transform.position+m_BossTarget.position;
            //    targetPosition = 0.5f * targetPosition;
            //    //targetPosition.z = m_MinZ;
            //    targetPosition.x = 0.6f * targetPosition.x;
            //}

            //if (PlayerChar.m_Current.m_LockedTarget==null)
            //{
            //    Direction = Quaternion.Euler(40, 0, 0) * Vector3.forward;
            //    distance = 80;
            //}
            //else
            //{
            //    targetPosition = PlayerChar.m_Current.transform.position + PlayerChar.m_Current.m_LockedTarget.transform.position;
            //    targetPosition = 0.5f * targetPosition;
            //    Direction = Quaternion.Euler(60, 0, 0) * Vector3.forward;
            //    distance = 80;
            //}


            //transform.position =Vector3.Lerp(transform.position,  targetPosition + -distance*Direction,5*Time.deltaTime);// - distance// * m_FaceVector;
            //transform.position += ShakeOffset;
            //transform.forward = Vector3.Lerp(transform.forward, Direction, 5 * Time.deltaTime);


            Vector3 offset = Input.mousePosition;
            offset.x /= Screen.width;
            offset.y /= Screen.height;
            offset.x = offset.x - 0.5f;
            offset.z = offset.y - 0.5f;
            offset.y = 0;

            m_TargetPoint = Vector3.zero;
            if (m_IsAiming)
            {
                m_TargetPoint = 12 * offset;
                Vector3 newPos = m_TargetPoint + (-m_TargetDistance * m_TargetDirection);
                transform.position = Vector3.Lerp(transform.position, newPos, 10 * Time.deltaTime);
                transform.position += ShakeOffset;
            }
            //bottom position and block
            float range = 200;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(0.5f * Screen.width, 0, 0));
            float dis = 0;
            new Plane(Vector3.up, Vector3.zero).Raycast(ray, out dis);
            m_CameraBottomPosition = ray.origin + dis * ray.direction;
            //m_BackBlock.position = m_CameraBottomPosition;

            ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(0.5f * Screen.width, Screen.height, 0));
            dis = 0;
            new Plane(Vector3.up, Vector3.zero).Raycast(ray, out dis);
            m_CameraTopPosition = ray.origin + dis * ray.direction;

            //m_TargetPointTransform.position = m_CameraTopPosition;
        }

        public void StartShake(float t, float r)
        {
            if (m_ShakeTimer == 0 || m_ShakeRadius < r)
                m_ShakeRadius = r;

            m_ShakeTimer = t;
        }

        public void BlendMove(Vector3 endPos, Quaternion endRot, float time)
        {
            StartCoroutine(Co_BlendMove(endPos, endRot, time));
        }
        IEnumerator Co_BlendMove(Vector3 endPos, Quaternion endRot, float time)
        {
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            float lerp = 0;
            float speed = 1f / time;
            //AnimationCurve curve = m_Contents.m_CamLerp;
            while (true)
            {
                transform.position = Vector3.Lerp(startPos, endPos, m_BlendCurve1.Evaluate(lerp));
                transform.rotation = Quaternion.Lerp(startRot, endRot, m_BlendCurve1.Evaluate(lerp));
                //m_MyCamera.orthographicSize = Mathf.Lerp(start, end, m_BlendCurve1.Evaluate(lerp));
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }

            transform.position = endPos;
            transform.rotation = endRot;
            //transform.localPosition = end;
            //m_MyCamera.orthographicSize = end;
        }

        public void BlendSize(float start, float end, float time)
        {
            StartCoroutine(Co_BlendSize(start, end, time));
        }
        IEnumerator Co_BlendSize(float start, float end, float time)
        {
            float lerp = 0;
            float speed = 1f / time;
            //AnimationCurve curve = m_Contents.m_CamLerp;
            while (true)
            {
                //transform.localPosition = Vector3.Lerp(start, end, curve.Evaluate(lerp));
                m_MyCamera.orthographicSize = Mathf.Lerp(start, end, m_BlendCurve1.Evaluate(lerp));
                lerp += speed * Time.deltaTime;
                if (lerp >= 1)
                    break;
                yield return null;
            }
            //transform.localPosition = end;
            m_MyCamera.orthographicSize = end;
        }
    }
}
