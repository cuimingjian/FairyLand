using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAudio : MonoBehaviour {

    private AudioSource m_AudioSource;
    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        m_AudioSource = GetComponent<AudioSource>();
        EventCenter.AddListener(EventDefine.ButtonClickAudio, PlayAudio);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ButtonClickAudio, PlayAudio);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);

    }
    private void PlayAudio()
    {
        m_AudioSource.PlayOneShot(vars.buttonClip);
    }
    /// <summary>
    /// Ises the music on.
    /// </summary>
    /// <param name="value">If set to <c>true</c> value.</param>
    private void IsMusicOn(bool value)
    {
        m_AudioSource.mute = !value;
    }
}
