using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundWave : MonoBehaviour
{
    public enum Mode { Auto, Manual }

    [Header("Core Settings")]
    [Tooltip("Het Visual Effect component (VFX Graph) dat de geluidsgolf afspeelt.")]
    [SerializeField] private VisualEffect visualEffect;

    [Tooltip("Kies 'Auto' voor automatische herhaling of 'Manual' om de golf handmatig te activeren. LET OP: In handmatige modus moet TriggerWave(Vector3 pos) vanuit een ander script worden aangeroepen.")]
    [SerializeField] private Mode mode = Mode.Auto;

    [Header("Auto Mode Settings")]
    [Tooltip("De vertraging in seconden voordat de eerste golf wordt afgevuurd.")]
    [SerializeField] private float delay = 0f;

    [Tooltip("De tijd in seconden tussen opeenvolgende golven.")]
    [SerializeField] private float repeatDelay = .1f;

    private Coroutine _autoEmitCoroutine;
    private Mode _lastMode;

    private void Start()
    {
        if (!ValidateReferences()) return;
        
        _lastMode = mode;
        RefreshState();
    }

    private void Update()
    {
        // Detecteer runtime wijzigingen in de inspector voor makkelijker testen
        if (mode != _lastMode)
        {
            _lastMode = mode;
            RefreshState();
        }
    }

    /// <summary>
    /// Synchroniseert de interne staat met de huidige modus. 
    /// Stopt actieve loops en start de logica opnieuw op basis van de selectie.
    /// </summary>
    private void RefreshState()
    {
        if (!Application.isPlaying) return;

        StopAutoLoop();

        if (mode == Mode.Auto)
        {
            _autoEmitCoroutine = StartCoroutine(EmitRoutine());
        }
    }

    private void StopAutoLoop()
    {
        if (_autoEmitCoroutine != null)
        {
            StopCoroutine(_autoEmitCoroutine);
            _autoEmitCoroutine = null;
        }
    }

    private bool ValidateReferences()
    {
        if (visualEffect == null)
        {
            Debug.LogError($"[SoundWave] FOUT: De 'Visual Effect' referentie op '{gameObject.name}' is leeg! Oplossing: Sleep een VFX Graph in het 'Visual Effect' veld in de Inspector.", this);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Activeert handmatig een golf op de opgegeven positie.
    /// </summary>
    public void TriggerWave(Vector3 pos)
    {
        if (ValidateReferences())
            ExecuteEmit(pos);
    }

    private void ExecuteEmit(Vector3 pos)
    {
        visualEffect.SetVector3("SystemPosition", pos);
        visualEffect.SendEvent("OnTik");
    }

    private IEnumerator EmitRoutine()
    {
        yield return new WaitForSeconds(delay);

        while (mode == Mode.Auto)
        {
            ExecuteEmit(transform.position);
            yield return new WaitForSeconds(repeatDelay);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundWave))]
public class SoundWaveEditor : Editor
{
    private SerializedProperty _visualEffect;
    private SerializedProperty _mode;
    private SerializedProperty _delay;
    private SerializedProperty _repeatDelay;

    private void OnEnable()
    {
        _visualEffect = serializedObject.FindProperty("visualEffect");
        _mode = serializedObject.FindProperty("mode");
        _delay = serializedObject.FindProperty("delay");
        _repeatDelay = serializedObject.FindProperty("repeatDelay");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Standard script reference field
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_visualEffect);
        EditorGUILayout.PropertyField(_mode);

        // Conditional display based on enum value
        if (_mode.enumValueIndex == (int)SoundWave.Mode.Auto)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Auto Emission Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_delay);
            EditorGUILayout.PropertyField(_repeatDelay);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
