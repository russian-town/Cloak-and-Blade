using System.Collections.Generic;
using UnityEngine;

public class MenuModelChanger : MonoBehaviour
{
    [SerializeField] private ModelPlace _modelPlace;

    private List<MenuModel> _menuModels = new List<MenuModel>();
    private MenuModel _selectedMenuModel;

    public void Create(Character character)
    {
        MenuModel menuModel = Instantiate(character.MenuModelTemplate, _modelPlace.transform);
        _menuModels.Add(menuModel);
        menuModel.Hide();
        menuModel.transform.rotation = _modelPlace.Rotation;
    }

    public bool TryChangeModel(int index)
    {
        if (_selectedMenuModel == null)
            return false;

        if (_selectedMenuModel == _menuModels[index])
            return false;

        _selectedMenuModel.Hide();
        _selectedMenuModel = null;
        SetSelectedModel(index);
        return true;
    }

    public void SetSelectedModel(int index)
    {
        _selectedMenuModel = _menuModels[index];
        _selectedMenuModel.Show();
    }
}
