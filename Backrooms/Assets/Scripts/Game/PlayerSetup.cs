using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    public GameObject[] gameObjectsToDisableOthers;

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
            foreach (GameObject gameObject in gameObjectsToDisableOthers)
            {
                gameObject.SetActive(false);
            }

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
