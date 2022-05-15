using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentUI : MonoBehaviour
{
    public PComponent _myComponent;
    public PComponent _parentComponent;
    public Direction _parentInstallDir;
    public bool _isOnScene;
    public PailouComponent _pailouComponent;

    public Text _dirText;

    public List<PailouComponent> topCandidateComps = new List<PailouComponent>();
    public List<PailouComponent> sideCandidateComps = new List<PailouComponent>();
    public List<PailouComponent> bottomCandidateComps = new List<PailouComponent>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMyChoiceView()
    {
        
    }

    public void IconPress()
    {
        if (_isOnScene)
        {
            PailouUtils._nowFocusComp = _myComponent;
            PailouUtils._gameManager.UpdateChoiceView(this);
            
        }
        else
        {
            // TODO : generate object
            GameObject compObPrefab = PailouUtils._gameManager.GetPrefabObFromPailouComp(_pailouComponent);
            _myComponent = Instantiate(compObPrefab).GetComponent<PComponent>();
            _myComponent._myButton = this.gameObject;
            // Update tree structure
            GameObject parentBut = _parentComponent._myButton;
            ComponentUI parentCompUI = parentBut.GetComponent<ComponentUI>();
            // Attach to compoent
            _parentComponent.AttachWith(_myComponent, _parentInstallDir, Margin.CENTER, 0);
            switch (_parentInstallDir)
            {
                case Direction.UP:
                    // my parent = ui_parent->parrent
                    this.gameObject.transform.SetParent(parentCompUI._parentComponent._myButton.transform);
                    _parentComponent = parentCompUI._parentComponent;
                    // ui_parrent->parrent = me
                    parentBut.transform.SetParent(this.gameObject.transform);
                    parentCompUI._parentComponent = _myComponent;
                    break;
                case Direction.DOWN:
                    this.gameObject.transform.SetParent(parentCompUI.transform);
                    _parentComponent = parentCompUI._myComponent;
                    break;
                case Direction.LEFT:
                case Direction.RIGHT:
                    this.gameObject.transform.SetParent(parentCompUI._parentComponent._myButton.transform);
                    _parentComponent = parentCompUI._parentComponent;
                    break;
            }
            _isOnScene = true;
            // TODO : call GameManager update tree.
            PailouUtils._gameManager.BuildUITree();
            PailouUtils._gameManager.ClearChoices();
            // Attach object
            
        }
    }

    public void SetInstallDir(Direction dir)
    {
        _parentInstallDir = dir;
        switch (dir)
        {
            case Direction.UP:
                _dirText.text = "U";
                break;
            case Direction.LEFT:
                _dirText.text = "L";
                break;
            case Direction.RIGHT:
                _dirText.text = "R";
                break;
            case Direction.DOWN:
                _dirText.text = "D";
                break;
        }
    }
}
