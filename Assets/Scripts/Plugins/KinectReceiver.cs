using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class KinectReceiver : MonoBehaviour
{
    [Tooltip("UPD")]
    private UdpClient udpClient;
    private Thread receiveThread;
    private string lastPacket = "";
    [Header("Bones")]
    public Transform head;
    public Transform neck;
    public Transform spineMid;
    public Transform spineBase;
    public Transform hip;
    
    public Transform shoulderL;
    public Transform upperArmL;
    public Transform lowerArmL;
    public Transform handL;

    public Transform shoulderR;
    public Transform upperArmR;
    public Transform lowerArmR;
    public Transform handR;

    public Transform upperLegL;
    public Transform lowerLegL;
    public Transform footL;

    public Transform upperLegR;
    public Transform lowerLegR;
    public Transform footR;
    [Space]
    [Header("Offsets")]
    public Vector3 headVector;
    public Vector3 upperLegLVector;
    public Vector3 lowerLegLVector;
    public Vector3 footLVector;

    #region Unity Methods
    void Start()
    {
        udpClient = new UdpClient(5055);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    private void Update()
    {
        if (!string.IsNullOrEmpty(lastPacket))
        {
            string[] parts = lastPacket.Split(',');

            if (parts.Length == 99)
            {
                //Debug.Log("Pacote recebido com 99 valores");
                Vector3[] joints = new Vector3[33];
                for (int i = 0; i < 33; i++) 
                {
                    float x = float.Parse(parts[i * 3]);
                    float y = float.Parse(parts[i * 3 + 1]);
                    float z = float.Parse(parts[i *3 + 2]);
                    joints[i] = new Vector3(
                        (x - 0.5f) * 2.0f, -(y - 0.5f) * 2.0f, -z * 2.0f);
                }

                // Cabeça e pescoço
                if (head != null)
                {
                    Vector3 dir = (joints[0] - joints[1]).normalized;
                    Quaternion rot = Quaternion.LookRotation(dir);

                    Quaternion offset = Quaternion.Euler(headVector);

                    head.rotation = Quaternion.Slerp(head.rotation, rot * offset, Time.deltaTime * 10f);
                }

                /*
                 * if (neck != null) 
                {
                    neck.rotation = Quaternion.LookRotation(((joints[11] + joints[12]) / 2f) - joints[0]);
                }**/

                // Braço esquerdo
                /*if (upperArmL != null) upperArmL.rotation = Quaternion.LookRotation(joints[13] - joints[11]);
                if (lowerArmL != null) lowerArmL.rotation = Quaternion.LookRotation(joints[15] - joints[13]);
                if (handL != null) handL.rotation = Quaternion.LookRotation(joints[19] - joints[15]);

                // Braço direito
                if (upperArmR != null) upperArmR.rotation = Quaternion.LookRotation(joints[14] - joints[12]);
                if (lowerArmR != null) lowerArmR.rotation = Quaternion.LookRotation(joints[16] - joints[14]);
                if (handR != null) handR.rotation = Quaternion.LookRotation(joints[20] - joints[16]);
                **/

                // Pernas
                if (upperLegL != null) 
                {
                    Vector3 dir = (joints[25] - joints[23]).normalized;
                    Quaternion rot = Quaternion.LookRotation(dir);

                    Quaternion offset = Quaternion.Euler(upperLegLVector);

                    upperLegL.rotation = Quaternion.Slerp(upperLegL.rotation, rot * offset, Time.deltaTime * 5f);
                }

                if (lowerLegL != null) 
                {
                    Vector3 dir = (joints[27] - joints[25]).normalized;
                    Quaternion rot = Quaternion.LookRotation(dir);

                    Quaternion offset = Quaternion.Euler(lowerLegLVector);

                    lowerLegL.rotation = Quaternion.Slerp(lowerLegL.rotation, rot * offset, Time.deltaTime * 5f);
                }
                if (footL != null) 
                {
                    Vector3 dir = (joints[31] - joints[29]).normalized;
                    Quaternion rot = Quaternion.LookRotation(dir);

                    Quaternion offset = Quaternion.Euler(footLVector);

                    footL.rotation = Quaternion.Slerp(footL.rotation, rot * offset, Time.deltaTime * 5f);
                }

                /*if (upperLegR != null) upperLegR.rotation = Quaternion.LookRotation(joints[26] - joints[24]);
                if (lowerLegR != null) lowerLegR.rotation = Quaternion.LookRotation(joints[28] - joints[26]);
                if (footR != null) footR.rotation = Quaternion.LookRotation(joints[30] - joints[28]);

                // Coluna
                if (spineMid != null) spineMid.rotation = Quaternion.LookRotation(((joints[23] + joints[24]) / 2f) - joints[11]);
                if (spineBase != null) spineBase.rotation = Quaternion.LookRotation(joints[24] - joints[23]);
                if (hip != null) hip.rotation = Quaternion.LookRotation(joints[24] - joints[23]);
                **/
            }
        }
    }
    #endregion

    #region Receive
    void ReceiveData()
    {
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
        
        while (true) 
        {
            byte[] data = udpClient.Receive(ref ip);
            lastPacket = Encoding.UTF8.GetString(data);
        }
    }
    #endregion

    #region Quit Application
    private void OnApplicationQuit()
    {
        receiveThread.Abort();
        udpClient.Close();
    }
    #endregion
}
