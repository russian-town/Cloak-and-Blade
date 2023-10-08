using System.Collections.Generic;
using UnityEngine;

public class MenuModelChanger : MonoBehaviour
{
    [SerializeField] private ModelPlace _modelPlace;

    private List<MenuModel> _menuModels = new List<MenuModel>();
    private MenuModel _selectedMenuModel;
    private MenuModel _defaultModel;

    public void Create(Character character)
    {
        MenuModel menuModel = Instantiate(character.MenuModelTemplate, _modelPlace.transform);
        _menuModels.Add(menuModel);
        menuModel.Hide();
        menuModel.transform.rotation = _modelPlace.Rotation;
    }

    public void SetDefaultModel(int index)
    {
        _defaultModel = _menuModels[index];

        if (_selectedMenuModel == null)
        {
            _selectedMenuModel = _defaultModel;
            _selectedMenuModel.Show();
        }
    }

    public void TryChange(int index)
    {
        if (_selectedMenuModel == null)
            return;

        _selectedMenuModel.Hide();
        _selectedMenuModel = null;
        SetSelectedModel(index);
    }

    private void SetSelectedModel(int index)
    {
        _selectedMenuModel = _menuModels[index];
        _selectedMenuModel.Show();
    }
}
