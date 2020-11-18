using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using AdVd.GlyphRecognition;
using TMPro;

public class CCastDrawnGlyph : MonoBehaviour
{

    #region Private Variables
    private GlyphDrawInput m_Gdi;
    [SerializeField]
    private GlyphSet m_CustomDictionaryGlyphSet;
    [SerializeField]
    private Glyph m_EmptyGlyph;
    [SerializeField]
    private CapStretchStrokeGraphic m_MorphTargetGlyph;

    [Header("UI")]
    public float m_AppearDuration = 0.6f;

    #region Private Variables
    [SerializeField]
    public float m_MinimumAlpha = 0.0f;
    [SerializeField]
    public float m_MaximumAlpha = 1.0f;
    private float m_StartTime;
    #endregion

    [Header("Training Mode")]
    [SerializeField]
    Image m_GlyphInputBackground = null;
    

    [SerializeField]
    private GameObject m_SliderGlyphDisplay;
    [SerializeField]
    private TextMeshProUGUI m_SliderText;
    [SerializeField]
    private GameObject m_SliderObject;

    [Header("Clear drawing")]
    [SerializeField]
    private Button b_ClearDrawingsButton;

    #endregion



    // Use this for initialization
    void Start () {
        m_Gdi = this.GetComponent<GlyphDrawInput>();
	}

	
	public void OnGlyphCast(int index, GlyphMatch match)
    {
        StopAllCoroutines();
        if (match != null)
        {
            Glyph foundGlyph = GameLoopManager.gameManagerInstance.glyphDictionary[match.target.name].Item2;
            m_SliderGlyphDisplay.GetComponent<GlyphDisplay>().glyph = foundGlyph;
            m_SliderObject.GetComponent<Animator>().SetTrigger("ShowNotif");
            m_SliderText.text = "Added " + match.target.name;
            float alpha = 1f;
            Color newColor = m_MorphTargetGlyph.color;
            newColor.a = alpha;
            m_MorphTargetGlyph.color = newColor;
            GameLoopManager.gameManagerInstance.AddNewSymbolToLayout(foundGlyph);
            StartCoroutine("FadeOut");
            /*if (m_Training)
            {
                if(m_ThisNotificationBox) m_ThisNotificationBox.PopUpNotification("Detected letter: " + match.target.name);
            }*/
            /*b_ClearDrawingsButton.onClick.Invoke(); //Call the button that clears inputs and drawings
            m_GlyphDisplay.GetComponent<GlyphDisplay>().glyph = CDictionaryManager.Instance.GetCustomGlyph(m_alphabetDictionary[match.target.name]);
            m_GlyphDisplay.SetActive(true);*/

        }
        else
        {
            Debug.Log("No match");
            /*if (m_Training)
            {
                if (m_ThisNotificationBox) m_ThisNotificationBox.PopUpNotification("No match");
            }*/
        }
    }

    public void CastOrRecast()
    {
        float start = Time.realtimeSinceStartup;
        if (!m_Gdi.Cast()) if (m_Gdi.castedGlyph != null) m_Gdi.Cast(m_Gdi.castedGlyph);
        Debug.Log("Cast time: " + (Time.realtimeSinceStartup - start));
    }

    public Glyph GetCustomGlyph(int glyph_index)
    {
        return m_CustomDictionaryGlyphSet[glyph_index];
    }

    private IEnumerator FadeOut()
    {
        Color tempPanelColorStart = m_MorphTargetGlyph.color;

        tempPanelColorStart.a = m_MaximumAlpha;

        m_MorphTargetGlyph.color = tempPanelColorStart;

        float Velocity = 0.0f;
        //float FadeInPanelAlpha= 0.0f;
        float FadeInTextAlpha = 1.0f;

        //Appear
        /*while (Mathf.Abs(m_MaximumAlpha - FadeInTextAlpha) > 0.01f)
        {
            Color currentPanelColor = m_NotificationPanel.color;
            Color currentTextColor = m_NotificationText.color;

            //FadeInPanelAlpha = Mathf.SmoothDamp(currentPanelColor.a, m_MaximumAlpha, ref Velocity, m_AppearDuration);
            FadeInTextAlpha = Mathf.SmoothDamp(currentTextColor.a, m_MaximumAlpha, ref Velocity, m_AppearDuration);

            currentPanelColor.a = FadeInTextAlpha;
            currentTextColor.a = FadeInTextAlpha;

            m_NotificationPanel.color = currentPanelColor;
            m_NotificationText.color = currentTextColor;

            yield return null;
        }*/
        yield return new WaitForSeconds(1f);
        while (Mathf.Abs(FadeInTextAlpha) > 0.01f)
        {
            Color currentPanelColor = m_MorphTargetGlyph.color;

            //FadeInPanelAlpha = Mathf.SmoothDamp(currentPanelColor.a, m_MaximumAlpha, ref Velocity, m_AppearDuration);
            FadeInTextAlpha = Mathf.SmoothDamp(m_MorphTargetGlyph.color.a, m_MinimumAlpha, ref Velocity, m_AppearDuration);

            currentPanelColor.a = FadeInTextAlpha;

            m_MorphTargetGlyph.color = currentPanelColor;

            yield return null;
        }

        tempPanelColorStart.a = m_MinimumAlpha;

        m_MorphTargetGlyph.color = tempPanelColorStart;

        yield return null;
    }
}
