using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using TMPro;
using LazyFollow = UnityEngine.XR.Interaction.Toolkit.UI.LazyFollow;

public struct Goal
{
    public GoalManager.OnboardingGoals CurrentGoal;
    public bool Completed;

    public Goal(GoalManager.OnboardingGoals goal)
    {
        CurrentGoal = goal;
        Completed = false;
    }
}

public class GoalManager : MonoBehaviour
{
    public enum OnboardingGoals
    {
        Empty,
        FindSurfaces,
        TapSurface,
    }

    // vondoste - replace this Queue with a list
    //Queue<Goal> m_OnboardingGoals;
    List<Goal> m_OnboardingGoals;
    Goal m_CurrentGoal;
    bool m_AllGoalsFinished;
    int m_SurfacesTapped;
    int m_CurrentGoalIndex = 0;
    int m_CurrentTestIndex = 0; // vondoste - initialize the current test number to 0
    int m_LastGoalIndex; // vondoste - adding variable to track goal just finished
    int m_FirstTestStep; // vondoste - tracking the beginning of the current test
    int m_LastTestStep;  // vondoste - tracking the end of the current test


    [Serializable]
    class Step
    {
        [SerializeField]
        public GameObject stepObject;

        [SerializeField]
        public string buttonText;

        public bool includeSkipButton;
    }

    [SerializeField]
    List<Step> m_StepList = new List<Step>();

    // vondoste - These defines the element in the "TestList" array below.
    [Serializable]
    class Test 
    {
        [SerializeField]
        public int testStart;
        public int testEnd;
    }
    
    // vondoste - This creates a new field in the Goal Manager Inspector window
    [SerializeField]
    List<Test> m_TestList = new List<Test>();

    [SerializeField]
    public TextMeshProUGUI m_StepButtonTextField;

    [SerializeField]
    public GameObject m_SkipButton;

    [SerializeField]
    GameObject m_LearnButton;

    [SerializeField]
    GameObject m_LearnModal;

    [SerializeField]
    Button m_LearnModalButton;

    [SerializeField]
    GameObject m_CoachingUIParent;

    [SerializeField]
    FadeMaterial m_FadeMaterial;

    [SerializeField]
    Toggle m_PassthroughToggle;

    [SerializeField]
    LazyFollow m_GoalPanelLazyFollow;

    [SerializeField]
    GameObject m_TapTooltip;

    [SerializeField]
    GameObject m_VideoPlayer;

    [SerializeField]
    Toggle m_VideoPlayerToggle;

    [SerializeField]
    ARPlaneManager m_ARPlaneManager;

    [SerializeField]
    ObjectSpawner m_ObjectSpawner;

    [SerializeField]
    public int m_CardWidth;

    [SerializeField]
    public int m_CardHeight;

    const int k_NumberOfSurfacesTappedToCompleteGoal = 1;
    Vector3 m_TargetOffset = new Vector3(-.5f, -.25f, 1.5f);

    void Start()
        // vondoste - this is where you add a variable for each coaching card and enqueue it
    {
        // vondoste - Change this queue to a list, and find all the dequeues to convert to a
        // persistent looping structure instead of a step through and die structure.
        m_OnboardingGoals = new List<Goal>();  //vondoste - replace queue with list (line ~34)
        BuildGoalList();

        m_CurrentGoal = m_OnboardingGoals[m_CurrentGoalIndex];  // vondoste - change to list, index by m_CurrentGoalIndex
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight);
        m_StepList[m_CurrentGoalIndex].stepObject.GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth,m_CardHeight + 150);
        m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2Int(150, -m_CardHeight - 20);
        string holding = m_CurrentGoalIndex.ToString();
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Card Number: " + holding;
        
        if (m_TapTooltip != null)
            m_TapTooltip.SetActive(false);

        if (m_VideoPlayer != null)
        {
            m_VideoPlayer.SetActive(false);

            if (m_VideoPlayerToggle != null)
                m_VideoPlayerToggle.isOn = false;
        }

        if (m_FadeMaterial != null)
        {
            m_FadeMaterial.FadeSkybox(false);

            if (m_PassthroughToggle != null)
                m_PassthroughToggle.isOn = false;
        }

        if (m_LearnButton != null)
        {
            m_LearnButton.GetComponent<Button>().onClick.AddListener(OpenModal); ;
            m_LearnButton.SetActive(false);
        }

        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }

        if (m_LearnModalButton != null)
        {
            m_LearnModalButton.onClick.AddListener(CloseModal);
        }

        if (m_ObjectSpawner == null)
        {
#if UNITY_2023_1_OR_NEWER
            m_ObjectSpawner = FindAnyObjectByType<ObjectSpawner>();
#else
            m_ObjectSpawner = FindObjectOfType<ObjectSpawner>();
#endif
        }
    }

    void OpenModal()
    {
        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.one;
        }
    }

    void CloseModal()
    {
        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }
    }


    /// <summary>
    /// Method <c>Update</c> is executed during every frame
    /// Author: vondoste
    /// </summary>
    void Update()
    {
        if (!m_AllGoalsFinished)
        {
            ProcessGoals();
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            PreviousTest();
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            NextTest();
        }
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            PreviousGoal();
        }
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            NextGoal();
        }

        // Debug Input
#if UNITY_EDITOR
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CompleteGoal();
        }


#endif
    }

    void ProcessGoals()
    {
        if (!m_CurrentGoal.Completed)
        {
            switch (m_CurrentGoal.CurrentGoal)
            {
                case OnboardingGoals.Empty:
                    m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
                    break;
                case OnboardingGoals.FindSurfaces:
                    m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
                    break;
                case OnboardingGoals.TapSurface:
                    if (m_TapTooltip != null)
                    {
                        m_TapTooltip.SetActive(true);
                    }
                    m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.None;
                    break;
            }
        }
    }

    void CompleteGoal()
    {
        if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface)
            m_ObjectSpawner.objectSpawned -= OnObjectSpawned;

        // disable tooltips before setting next goal
        DisableTooltips();

        m_CurrentGoal.Completed = true;
        m_CurrentGoalIndex++;
        if (m_OnboardingGoals.Count > 0)
        {
            m_CurrentGoal = m_OnboardingGoals[m_CurrentGoalIndex]; // vondoste - change to list, index by m_CurrentGoalIndex
            m_StepList[m_CurrentGoalIndex - 1].stepObject.SetActive(false);
            m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);
            m_StepButtonTextField.text = m_StepList[m_CurrentGoalIndex].buttonText;
            m_SkipButton.SetActive(m_StepList[m_CurrentGoalIndex].includeSkipButton);
            
            // vondoste - This accesses the indexed child object of the step object, then 
            // retrieves the component by <component type> and then sets its attribute.
            string holding = m_CurrentGoalIndex.ToString();
            //foreach (GoalManager.Step st in m_StepList) { }
            m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =  "Card Number: " + holding;

            
        }
        else
        {
            m_AllGoalsFinished = true;
            ForceEndAllGoals();
        }

        if (m_CurrentGoal.CurrentGoal == OnboardingGoals.FindSurfaces)
        {
            if (m_FadeMaterial != null)
                m_FadeMaterial.FadeSkybox(true);

            if (m_PassthroughToggle != null)
                m_PassthroughToggle.isOn = true;

            if (m_LearnButton != null)
            {
                m_LearnButton.SetActive(true);
            }

            // vondoste - commented out
            //StartCoroutine(TurnOnPlanes());
        }
        else if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface)
        {
            if (m_LearnButton != null)
            {
                m_LearnButton.SetActive(false);
            }
            m_SurfacesTapped = 0;
            m_ObjectSpawner.objectSpawned += OnObjectSpawned;
        }
    }

    public IEnumerator TurnOnPlanes()
    {
        yield return new WaitForSeconds(1f);
        // vondoste - changed to false
        m_ARPlaneManager.enabled = false;
    }

    void DisableTooltips()
    {
        if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface)
        {
            if (m_TapTooltip != null)
            {
                m_TapTooltip.SetActive(false);
            }
        }
    }

    public void ForceCompleteGoal()
    {
        CompleteGoal();
    }

    public void ForceEndAllGoals()
    {
        m_CoachingUIParent.transform.localScale = Vector3.zero;

        //TurnOnVideoPlayer();

        if (m_VideoPlayerToggle != null)
            m_VideoPlayerToggle.isOn = true;


        if (m_FadeMaterial != null)
        {
            m_FadeMaterial.FadeSkybox(true);

            if (m_PassthroughToggle != null)
                m_PassthroughToggle.isOn = true;
        }

        if (m_LearnButton != null)
        {
            m_LearnButton.SetActive(false);
        }

        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }

        // vondoste - commented out
        //StartCoroutine(TurnOnPlanes());
    }

    public void ResetCoaching()
        // vondoste - this duplicates the action at beginning of Start() around line 100
    {
        m_CoachingUIParent.transform.localScale = Vector3.one;
         // vondoste - maybe break this block out into a function to read m_StepList and autopopulate
        m_OnboardingGoals.Clear(); // vondoste - change to list, index by m_CurrentGoalIndex
        m_OnboardingGoals = new List<Goal>(); // vondoste - change to list, index by m_CurrentGoalIndex
        //var welcomeGoal = new Goal(OnboardingGoals.Empty);
        //var findSurfaceGoal = new Goal(OnboardingGoals.FindSurfaces);
        //var tapSurfaceGoal = new Goal(OnboardingGoals.TapSurface);
        //var endGoal = new Goal(OnboardingGoals.Empty);
        //var baseGoal = new Goal(OnboardingGoals.Empty);

        //m_OnboardingGoals.Enqueue(welcomeGoal);
        //m_OnboardingGoals.Enqueue(findSurfaceGoal);
        //m_OnboardingGoals.Enqueue(tapSurfaceGoal);
        //m_OnboardingGoals.Enqueue(endGoal);
        //m_OnboardingGoals.Enqueue(baseGoal);
        BuildGoalList();

        for (int i = 0; i < m_StepList.Count; i++)
        {
            if (i == 0)
            {
                m_StepList[i].stepObject.SetActive(true);
                m_SkipButton.SetActive(m_StepList[i].includeSkipButton);
                m_StepButtonTextField.text = m_StepList[i].buttonText;
            }
            else
            {
                m_StepList[i].stepObject.SetActive(false);
            }
        }

        m_CurrentGoal = m_OnboardingGoals[m_CurrentGoalIndex];
        m_AllGoalsFinished = false;

        if (m_TapTooltip != null)
            m_TapTooltip.SetActive(false);

        if (m_LearnButton != null)
        {
            m_LearnButton.SetActive(false);
        }

        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }

        m_CurrentGoalIndex = 0;
    }

    void OnObjectSpawned(GameObject spawnedObject)
    {
        m_SurfacesTapped++;
        if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface && m_SurfacesTapped >= k_NumberOfSurfacesTappedToCompleteGoal)
        {
            CompleteGoal();
            m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
        }
    }

    public void TooglePlayer(bool visibility)
    {
        if (visibility)
        {
            // vondoste - commented out.
            //TurnOnVideoPlayer();
        }
        else
        {
            m_VideoPlayer.SetActive(false);
        }
    }

    void TurnOnVideoPlayer()
    {
        if (m_VideoPlayer.activeSelf)
            return;

        var follow = m_VideoPlayer.GetComponent<LazyFollow>();
        if (follow != null)
            follow.rotationFollowMode = LazyFollow.RotationFollowMode.None;

        m_VideoPlayer.SetActive(false);
        var target = Camera.main.transform;
        var targetRotation = target.rotation;
        var newTransform = target;
        var targetEuler = targetRotation.eulerAngles;
        targetRotation = Quaternion.Euler
        (
            0f,
            targetEuler.y,
            targetEuler.z
        );

        newTransform.rotation = targetRotation;
        var targetPosition = target.position + newTransform.TransformVector(m_TargetOffset);
        m_VideoPlayer.transform.position = targetPosition;


        var forward = target.position - m_VideoPlayer.transform.position;
        var targetPlayerRotation = forward.sqrMagnitude > float.Epsilon ? Quaternion.LookRotation(forward, Vector3.up) : Quaternion.identity;
        targetPlayerRotation *= Quaternion.Euler(new Vector3(0f, 180f, 0f));
        var targetPlayerEuler = targetPlayerRotation.eulerAngles;
        var currentEuler = m_VideoPlayer.transform.rotation.eulerAngles;
        targetPlayerRotation = Quaternion.Euler
        (
            currentEuler.x,
            targetPlayerEuler.y,
            currentEuler.z
        );

        m_VideoPlayer.transform.rotation = targetPlayerRotation;
        m_VideoPlayer.SetActive(true);
        if (follow != null)
            follow.rotationFollowMode = LazyFollow.RotationFollowMode.LookAtWithWorldUp;
    }
    /// <summary>
    /// Method <c>BuildGoalList</c> automatically fills the m_OnboardingGoals queue
    /// with the cards specified in the GoalManager StepList. <br/>
    /// Author: vondoste
    /// </summary>
    public void BuildGoalList()
    {
        //var welcomeGoal = new Goal(OnboardingGoals.Empty);
        //var findSurfaceGoal = new Goal(OnboardingGoals.Empty);
        //var tapSurfaceGoal = new Goal(OnboardingGoals.Empty);
        //var endGoal = new Goal(OnboardingGoals.Empty);
        //var baseGoal = new Goal(OnboardingGoals.Empty);
        //m_OnboardingGoals.Enqueue(welcomeGoal);
        //m_OnboardingGoals.Enqueue(findSurfaceGoal);
        //m_OnboardingGoals.Enqueue(tapSurfaceGoal);
        //m_OnboardingGoals.Enqueue(endGoal);
        //m_OnboardingGoals.Enqueue(baseGoal);
        //
        // vondoste - retrieve the length of m_StepList, and add that many steps to the queue
        var cards = new List<Goal>();
        for (int i = 0; i < m_StepList.Count; i++)
        {
            m_OnboardingGoals.Add(new Goal(OnboardingGoals.Empty));
        }
        // vondoste - initialize the first and last test step to the current test number
        m_FirstTestStep = m_TestList[m_CurrentTestIndex].testStart;
        m_LastTestStep = m_TestList[m_CurrentTestIndex].testEnd;
    }

    /// <summary>
    /// Method <c>NextGoal()</c> Steps forward to the next card to display. <br/>
    /// Author: vondoste
    /// </summary>
    public void NextGoal()
    {
        // disable tooltips before setting next goal
        DisableTooltips();

        m_CurrentGoal.Completed = true;
        m_LastGoalIndex = m_CurrentGoalIndex;
        m_CurrentGoalIndex++;
        if (m_CurrentGoalIndex > m_LastTestStep)
        {
            m_CurrentGoalIndex = m_FirstTestStep;
        }

        m_CurrentGoal = m_OnboardingGoals[m_CurrentGoalIndex]; // vondoste - change to list, index by m_CurrentGoalIndex
        m_StepList[m_LastGoalIndex].stepObject.SetActive(false);
        // vondoste - attempt to resize the card
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight);
        m_StepList[m_CurrentGoalIndex].stepObject.GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight + 150);
        m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);
        m_StepButtonTextField.text = m_StepList[m_CurrentGoalIndex].buttonText;
        m_SkipButton.SetActive(m_StepList[m_CurrentGoalIndex].includeSkipButton);

        // vondoste - This accesses the indexed child object of the step object, then 
        // retrieves the component by <component type> and then sets its attribute.
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2Int(150, -m_CardHeight - 20);
        string holding = m_CurrentGoalIndex.ToString();
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Card Number: " + holding;
               
    }

    /// <summary>
    /// Method <c>PreviousGoal()</c> Steps backward to the previous card to display. <br/>
    /// Author: vondoste
    /// </summary>
    public void PreviousGoal()
    {
        // disable tooltips before setting next goal
        DisableTooltips();

        m_CurrentGoal.Completed = true;
        m_LastGoalIndex = m_CurrentGoalIndex;
        m_CurrentGoalIndex--;
        if (m_CurrentGoalIndex < m_FirstTestStep)
        {
            m_CurrentGoalIndex = m_LastTestStep;
        } 



        m_CurrentGoal = m_OnboardingGoals[m_CurrentGoalIndex]; // vondoste - change to list, index by m_CurrentGoalIndex
        m_StepList[m_LastGoalIndex].stepObject.SetActive(false);
        // vondoste - attempt to resize the card
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight);
        m_StepList[m_CurrentGoalIndex].stepObject.GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight + 150);
        m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);
        m_StepButtonTextField.text = m_StepList[m_CurrentGoalIndex].buttonText;
        m_SkipButton.SetActive(m_StepList[m_CurrentGoalIndex].includeSkipButton);

        // vondoste - This accesses the indexed child object of the step object, then 
        // retrieves the component by <component type> and then sets its attribute.
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2Int(150, -m_CardHeight - 20);
        string holding = m_CurrentGoalIndex.ToString();
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Card Number: " + holding;

    }

    /// <summary>
    /// Method <c>NextTest()</c> increments the current test counter, and sets the 
    /// Author: vondoste
    /// </summary>
    public void  NextTest()
    {
        DisableTooltips();
        m_CurrentGoal.Completed = true;
        m_StepList[m_CurrentGoalIndex].stepObject.SetActive(false);
        m_CurrentTestIndex++;
        if (m_CurrentTestIndex >= m_TestList.Count)
        {
            m_CurrentTestIndex = 0;
        } 
        m_FirstTestStep = m_TestList[m_CurrentTestIndex].testStart;
        m_LastTestStep = m_TestList[m_CurrentTestIndex].testEnd;
        m_CurrentGoalIndex = m_FirstTestStep;
        // vondoste - attempt to resize the card
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight);
        m_StepList[m_CurrentGoalIndex].stepObject.GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight + 150);
        m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2Int(150, -m_CardHeight - 20);
        string holding = m_CurrentGoalIndex.ToString();
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Card Number: " + holding;
    }

    /// <summary>
    /// Method <c>PreviousTest()</c> decrements the current test counter, and sets the 
    /// Author: vondoste
    /// </summary>
    public void PreviousTest()
    {
        DisableTooltips();
        m_CurrentGoal.Completed = true;
        m_StepList[m_CurrentGoalIndex].stepObject.SetActive(false);
        m_CurrentTestIndex--;
        if (m_CurrentTestIndex < 0)
        {
            m_CurrentTestIndex = m_TestList.Count - 1;
        }
                m_FirstTestStep = m_TestList[m_CurrentTestIndex].testStart;
        m_LastTestStep = m_TestList[m_CurrentTestIndex].testEnd;
        m_CurrentGoalIndex = m_FirstTestStep;
        // vondoste - attempt to resize the card
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight);
        m_StepList[m_CurrentGoalIndex].stepObject.GetComponent<RectTransform>().sizeDelta = new Vector2Int(m_CardWidth, m_CardHeight + 150);
        m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2Int(150, -m_CardHeight - 20);
        string holding = m_CurrentGoalIndex.ToString();
        m_StepList[m_CurrentGoalIndex].stepObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Card Number: " + holding;
    }

    

}
