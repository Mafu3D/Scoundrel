using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffCollectionView : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Awake()
    {

    }

    private void Update()
    {
        PlayerBuffManager buffManager = player.BuffManager;
        if (buffManager != null)
        {
            // Display buff icons
            List<PlayerBuff> visibleBuffs = buffManager.GetVisibleBuffs();
            int buffCount = visibleBuffs.Count;
            if (buffCount > 0 )
            {
                // this.gameObject.SetActive(true);
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    Transform child = this.transform.GetChild(i);
                    if (i < buffCount)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<PlayerBuffView>().RegisterPlayerBuff(visibleBuffs[i]);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                        child.GetComponent<PlayerBuffView>().DeregisterPlayerBuff();
                    }
                }
            }
            else
            {
                foreach (Transform child in this.transform)
                {
                    child.gameObject.SetActive(false);
                    child.GetComponent<PlayerBuffView>().DeregisterPlayerBuff();
                }
            }
        }
        else
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
                child.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }
}
