using System.Collections.Generic;
using UnityEngine;

public class ModelsScroll : MonoBehaviour
{
    private List<MenuPlayerModel> _menuPlayerModels = new List<MenuPlayerModel>();

    public void Initialize(MenuPlayerModel model)
    {
        _menuPlayerModels.Add(model);
    }

    public void SwitchCurrentModel(MenuPlayerModel menuPlayerModel)
    {

    }
}
