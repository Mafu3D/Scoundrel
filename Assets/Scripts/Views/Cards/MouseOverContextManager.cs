using System.Collections.Generic;
using UnityEngine;

public class MouseOverContextManager : MonoBehaviour
{
    [Header("Hover Context Icons")]
    [SerializeField] MouseOverContextView topMouseOverContext;
    [SerializeField] MouseOverContextView botMouseOverContext;
    [SerializeField] MouseOverContextView fullMouseOverContext;

    private List<MouseOverContextView> allMouseOverContexts;

    public void HideAllMouseOverContextViews() => allMouseOverContexts.ForEach(view => view.Hide());

    public void ShowTop(string text, Color color) => ToggleMouseOverContextView(topMouseOverContext, text, color);

    public void ShowBot(string text, Color color) => ToggleMouseOverContextView(botMouseOverContext, text, color);

    public void ShowFull(string text, Color color) => ToggleMouseOverContextView(fullMouseOverContext, text, color);

    private void Awake()
    {
        allMouseOverContexts = new() {topMouseOverContext, botMouseOverContext, fullMouseOverContext};
    }

    private void OnEnable()
    {
        HideAllMouseOverContextViews();
    }

    private void ToggleMouseOverContextView(MouseOverContextView viewToShow, string text, Color color)
    {
        if (viewToShow == null)
        {
            HideAllMouseOverContextViews();
            return;
        }

        foreach (MouseOverContextView view in allMouseOverContexts)
        {
            if (view == viewToShow)
            {
                view.Show(text, color);
                continue;
            }
            view.Hide();
        }
    }
}