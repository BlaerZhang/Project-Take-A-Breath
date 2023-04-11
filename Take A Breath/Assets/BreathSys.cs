using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        SettingDefault();
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
        Debug.Log("value   " + changeValue);
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
}
