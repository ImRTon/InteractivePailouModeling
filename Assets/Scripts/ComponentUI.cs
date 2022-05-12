using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentUI : MonoBehaviour
{
    public Component _myComponent;
    public Component _parentComponent;
    public Direction _parentInstallDir;
    public bool _isOnScene;
    public PailouComponent _pailouComponent;

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
            GameObject parentBut = _parentComponent.gameObject;
            this.gameObject.transform.SetParent(parentBut.transform);
            // TODO : call GameManager update tree.
            PailouUtils._gameManager.BuildUITree();
            // TODO : generate object
        }
    }

    //public 
}
