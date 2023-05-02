using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class BreathSys : MonoBehaviour
{
    public List<BreathUI> breathUIs;
    public int activeBreathIndex = 0;
    public Image playerBreathImg;

    public float breathTime = 0;

    public BreathDataContainer breathDataSCO;

    public bool underAttack = false;
    
    private MMF_Player fireSwitchFeedback;
    private MMF_Player iceSwitchFeedback;
    private MMF_Player windSwitchFeedback;

    private void Awake()
    {
        SettingDefault();
        
        fireSwitchFeedback = GameObject.Find("FireSwitchFeedback").GetComponent<MMF_Player>();
        iceSwitchFeedback = GameObject.Find("IceSwitchFeedback").GetComponent<MMF_Player>();
        windSwitchFeedback = GameObject.Find("WindSwitchFeedback").GetComponent<MMF_Player>();
    }

    private void Update()
    {
        playerBreathImg.fillAmount = breathTime;
    }

    public void SettingDefault()
    {
        activeBreathIndex = 0;
        ActiveBreathByIndex(0);

    }

    public void InitializeBreathPt()
    {
        breathDataSCO.firePt = 3;
        breathDataSCO.waterPt = 3;
        breathDataSCO.bugPt = 3;
        UpdateBreathPtUI();
    }

    public void ChangeBreath()
    {
        activeBreathIndex += 1;
        if(activeBreathIndex > breathUIs.Count - 1)
        {
            activeBreathIndex = 0;
        }

        ActiveBreathByIndex(activeBreathIndex);
        PlaySwitchBreathFeedbacks(activeBreathIndex);
    }


    public void ActiveBreathByIndex(int index)
    {
        foreach(BreathUI ui in breathUIs)
        {
            if(ui != breathUIs[index])
            {
                ui.breathHu.gameObject.SetActive(false);
            }
        }

        breathUIs[index].breathHu.gameObject.SetActive(true);
    }

    public void ChangeBreathPt(int changeValue)
    {
        DetectOrChangeCurrentBreathPt(changeValue);
        UpdateBreathPtUI();
    }

    public void UpdateBreathPtUI()
    {
        breathUIs[0].breathIntUI.text = breathDataSCO.firePt.ToString();
        breathUIs[1].breathIntUI.text = breathDataSCO.waterPt.ToString();
        breathUIs[2].breathIntUI.text = breathDataSCO.bugPt.ToString();
    }

    public int DetectOrChangeCurrentBreathPt(int changeValue = 0)
    {
        if (activeBreathIndex == 0)
        {
            breathDataSCO.firePt += changeValue;
            return breathDataSCO.firePt;
        }
        else if (activeBreathIndex == 1)
        {
            breathDataSCO.waterPt += changeValue;
            return breathDataSCO.waterPt;
        }
        else if (activeBreathIndex == 2)
        {
            breathDataSCO.bugPt += changeValue;
            return breathDataSCO.bugPt;
        }
        else
        {
            return 0;
        }
    }
    public void DetectOutOfBreath()
    {
        if(DetectOrChangeCurrentBreathPt() == 0)
        {
            GameMaster.Instance().ifLose = true;
        }
    }

    public void PlaySwitchBreathFeedbacks(int index)
    {
        switch (index)
        {
            case 0:
                fireSwitchFeedback.PlayFeedbacks();
                break;
            case 1:
                iceSwitchFeedback.PlayFeedbacks();
                break;
            case 2:
                windSwitchFeedback.PlayFeedbacks();
                break;
        }
    }
}
