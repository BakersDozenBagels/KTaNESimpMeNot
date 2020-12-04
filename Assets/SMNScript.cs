using System.Collections;
using System.Linq;
using UnityEngine;

public class SMNScript : MonoBehaviour {
    public KMSelectable button;
    public Transform buttonTransform;
    public KMBossModule boss;
    public KMBombModule Module;
    public KMBombInfo Info;
    public KMAudio Audio;

    public AudioClip SFX;

    public TextMesh textMesh;

    private int correctInputs = 0;
    private int currentStage = 0;
    private int totalStages;
    private string[] ignoredModules;

    private bool canReceiveInput = false;

    private static int modID = 1;
    private int curModID;

    private void Awake()
    {
        curModID = modID++;
    }

    // Use this for initialization
    void Start () {
        if (ignoredModules == null)
            ignoredModules = boss.GetIgnoredModules("Simp Me Not", new string[]{
                "14",
                "Cruel Purgatory",
                "Forget Enigma",
                "Forget Everything",
                "Forget It Not",
                "Forget Me Later",
                "Forget Me Not",
                "Forget Perspective",
                "Forget Them All",
                "Forget This",
                "Forget Us Not",
                "Organization",
                "Purgatory",
                "Simon's Stages",
                "Souvenir",
                "Tallordered Keys",
                "The Time Keeper",
                "Timing is Everything",
                "The Troll",
                "Turn The Key",
                "Übermodule",
                "Ültimate Custom Night",
                "The Very Annoying Button",
                "Simp Me Not"
            });
        Module.OnActivate += delegate ()
        {
            totalStages = Info.GetSolvableModuleNames().Where(a => !ignoredModules.Contains(a)).ToList().Count;
            if (totalStages <= 0) { Debug.LogFormat("[Simp Me Not #{0}] Cannot generate stages, autosolving...", curModID); Module.HandlePass(); }
        };
        button.OnInteract += delegate ()
        {
            HandleButton();
            return false;
        };
    }

    private void HandleButton()
    {
        button.AddInteractionPunch();
        if (!canReceiveInput)
        {
            Debug.LogFormat("[Forget Simp Not #{0}] Pressed too early, striking.", curModID);
            Module.HandleStrike();
        }
        else
        {
            StopCoroutine("Wait");
            correctInputs++;
            StartCoroutine("Wait");
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (correctInputs == totalStages)
        {
            Debug.LogFormat("[Forget Simp Not #{0}] Solved!", curModID);
            Audio.PlaySoundAtTransform(SFX.name, transform);
            textMesh.text = "VICTORY!";
            canReceiveInput = false;
            Module.HandlePass();
        }
        else
        {
            Debug.LogFormat("[Forget Simp Not #{0}] Pressed wrong number of times, striking.", curModID);
            Module.HandleStrike();
            correctInputs = 0;
        }
    }

    private int cooldown = 0;
    // Update is called once per frame
    void Update () {

        if (cooldown > 0)
            cooldown--;
        else
        {
            int solvecount = Info.GetSolvedModuleNames().Where(a => !ignoredModules.Contains(a)).ToList().Count;
            if (solvecount > currentStage)
            {
                currentStage++;
                if (currentStage >= totalStages)
                {
                    
                    canReceiveInput = true;
                }
                cooldown = 80;
            }
        }
    }
}
