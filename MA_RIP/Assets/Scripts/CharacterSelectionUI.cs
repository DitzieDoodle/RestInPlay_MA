using UnityEngine;

public class CharacterSelectionUI : MonoBehaviour
{
    public void SelectEyes(int index)
    {
        CharacterSelectionData.Instance.selectedEyesIndex = index;
        CharacterSelectionData.Instance.SaveSelection();
        // ApplyEyesSkin(index);
    }

    public void SelectMouth(int index)
    {
        CharacterSelectionData.Instance.selectedMouthIndex = index;
        CharacterSelectionData.Instance.SaveSelection();
        // ApplyMouthSkin(index);
    }

    public void SelectBody(int index)
    {
        CharacterSelectionData.Instance.selectedBodyIndex = index;
        CharacterSelectionData.Instance.SaveSelection();
        // ApplyBodySkin(index);
    }

    public void SelectColor(int index)
    {
        CharacterSelectionData.Instance.selectedColorIndex = index;
        CharacterSelectionData.Instance.SaveSelection();
        // ApplyColor(index);
    }
}
