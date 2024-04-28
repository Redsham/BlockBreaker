using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class LoadingScreenManager : MonoBehaviour
    {
        private static LoadingScreenManager m_Active;
        
        [SerializeField] private GameObject m_Root;
        
        [Header("Components")]
        [SerializeField] private RectTransform m_Indicator;
        [SerializeField] private Image m_Background;
        [SerializeField] private RectTransform m_PanelTransform;
        [SerializeField] private CanvasGroup m_PanelGroup;
        
        [Header("Settings")]
        [SerializeField] private float m_RotationSpeed = 1.0f;

        private void Awake()
        {
            m_Active = this;
            Hide(true);
        }
        
        private void ShowAnimated()
        {
            m_Background.color = Color.clear;
            LeanTween.alpha(m_Background.rectTransform, 0.9f, 0.1f);
            
            m_PanelGroup.alpha = 0.0f;
            LeanTween.alphaCanvas(m_PanelGroup, 1.0f, 0.1f)
                .setEaseInQuad()
                .setDelay(0.05f);
            
            m_PanelTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
            LeanTween.moveY(m_PanelTransform, m_PanelTransform.rect.height, 0.1f)
                .setEaseInQuad()
                .setDelay(0.05f);

            LeanTween.rotateAround(m_Indicator.gameObject, Vector3.forward, 360.0f, 1.0f / m_RotationSpeed).setLoopClamp();
        }
        private void HideAnimated()
        {
            LeanTween.alpha(m_Background.rectTransform, 0.0f, 0.1f);
            LeanTween.alphaCanvas(m_PanelGroup, 0.0f, 0.1f).setEaseInQuad();
            LeanTween.moveY(m_PanelTransform, m_PanelTransform.rect.height, 0.1f)
                .setEaseInQuad()
                .setOnComplete(() =>
            {
                LeanTween.cancel(m_Indicator.gameObject);
                m_Active.m_Root.SetActive(false);
            });
        }

        public static void Show(bool forced = false)
        {
            m_Active.m_Root.SetActive(true);
            
            if(!forced)
                m_Active.ShowAnimated();
            else
            {
                m_Active.m_Background.color = new Color(0.0f, 0.0f, 0.0f, 0.9f);
                m_Active.m_PanelGroup.alpha = 1.0f;
                m_Active.m_PanelTransform.anchoredPosition = new Vector2(0.0f, m_Active.m_PanelTransform.rect.height);
            }
        }
        public static void Hide(bool forced = false)
        {
            if(forced)
                m_Active.m_Root.SetActive(false);
            
            m_Active.HideAnimated();
        }
    }
}