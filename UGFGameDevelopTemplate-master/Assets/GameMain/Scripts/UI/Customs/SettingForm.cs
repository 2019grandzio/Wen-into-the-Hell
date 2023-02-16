using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

namespace Hamood
{
    /// <summary>
    /// 设置界面脚本
    /// </summary>
    public class SettingForm : UGuiForm
    {
        public Slider m_Slider_Music;
        public Slider m_Slider_Sound;
        public void OnCloseButtonClick()
        {
            Close();
        }
 
        public void OnMusicSettingValueChange(float value)
        {
            GameEntry.Sound.SetVolume("Music", m_Slider_Music.value);
        }
 
        public void OnSoundSettingValueChange(float value)
        {
            GameEntry.Sound.SetVolume("Sound", m_Slider_Sound.value);
            GameEntry.Sound.SetVolume("UISound", m_Slider_Sound.value);
        }
    }
}