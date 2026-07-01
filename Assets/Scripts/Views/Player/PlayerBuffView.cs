using System.Collections.Generic;
using Project.UI.Tooltips;
using UnityEngine;

public class PlayerBuffView : MonoBehaviour, ITooltipGettable
{
    SpriteRenderer spriteRenderer;
    PlayerBuff registeredBuff;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RegisterPlayerBuff(PlayerBuff buff)
    {
        if (registeredBuff != null)
        {
            DeregisterPlayerBuff();
        }

        if (buff != null)
        {
            registeredBuff = buff;
            spriteRenderer.sprite = buff.Sprite;
        }
    }

    public void DeregisterPlayerBuff()
    {
        spriteRenderer.sprite = null;
        registeredBuff = null;
    }

    public bool TryGetTooltipInformation(out TooltipCollection tooltipCollection)
    {
        if (registeredBuff == null)
        {
            tooltipCollection = new TooltipCollection(new List<TooltipData>());
            return false;
        }

        TooltipData tooltipData = new TooltipData(
            header: registeredBuff.Name,
            subtitle: "",
            content: registeredBuff.Description,
            image: registeredBuff.Sprite
        );

        tooltipCollection = new TooltipCollection(new List<TooltipData> { tooltipData });
        return true;
    }
}
