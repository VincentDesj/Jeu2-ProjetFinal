using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisableOthers;

    [SerializeField]
    private GameObject[] componentsToDisableSelf;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.CompareTag("CharacterModel"))
            {
                // componentsToDisableSelf[componentsToDisableSelf.Length] = child.gameObject;
            }
        }

        int nbComponents;
        if(!isLocalPlayer)
        {
            nbComponents = componentsToDisableOthers.Length;
            for (int i = 0; i < nbComponents; i++)
            {
                componentsToDisableOthers[i].enabled = false;
            }
        } else
        {
            nbComponents = componentsToDisableSelf.Length;
            for (int i = 0; i < nbComponents; i++)
            {
                //componentsToDisableSelf[i].SetActive(false);
            }
        }
    }
}
