using UnityEngine;
using UnityEngine.VFX;
public class SoundWave : MonoBehaviour
{

    [SerializeField] VisualEffect visualEffect;

    public void TriggerWave(Vector3 pos)
    {

        visualEffect.SendEvent("OnTik");
        visualEffect.SetVector3("PlayerPosition", pos);
    }

}

