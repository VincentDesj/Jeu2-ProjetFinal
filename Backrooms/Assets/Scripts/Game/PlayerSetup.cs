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
        /*
        foreach(Transform child in transform)
        {
            if(child.CompareTag("CharacterModel"))
            {
                // componentsToDisableSelf[componentsToDisableSelf.Length] = child.gameObject;
            }
        }
        */

        if(!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisableOthers.Length; i++)
            {
                componentsToDisableOthers[i].enabled = false;
            }
        } else
        {
            for (int i = 0; i < componentsToDisableSelf.Length; i++)
            {
                componentsToDisableSelf[i].SetActive(false);
            }
        }
    }
}
