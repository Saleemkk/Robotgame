using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrostweepGames.Plugins.Core;


    public class TutorialExample : MonoBehaviour
    {
        private FrostweepGames.Plugins.GoogleCloud.TextToSpeech.GCTextToSpeech _gcTextToSpeech;
        private FrostweepGames.Plugins.GoogleCloud.TextToSpeech.Voice[] _voices;

        // Start is called before the first frame update
        void Start()
        {
            _gcTextToSpeech = FrostweepGames.Plugins.GoogleCloud.TextToSpeech.GCTextToSpeech.Instance;

            _gcTextToSpeech.GetVoicesSuccessEvent += _gcTextToSpeech_GetVoicesSuccessEvent;
            _gcTextToSpeech.SynthesizeSuccessEvent += _gcTextToSpeech_SynthesizeSuccessEvent;

            _gcTextToSpeech.GetVoicesFailedEvent += _gcTextToSpeech_GetVoicesFailedEvent;
            _gcTextToSpeech.SynthesizeFailedEvent += _gcTextToSpeech_SynthesizeFailedEvent;

            _gcTextToSpeech.GetVoices(new FrostweepGames.Plugins.GoogleCloud.TextToSpeech.GetVoicesRequest()
            {
                languageCode = _gcTextToSpeech.PrepareLanguage(FrostweepGames.Plugins.GoogleCloud.TextToSpeech.Enumerators.LanguageCode.en_US),
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TTS_saveFile(string content)
        {
            _gcTextToSpeech.Synthesize(content, new FrostweepGames.Plugins.GoogleCloud.TextToSpeech.VoiceConfig()
            {
                gender = 0,
                languageCode = "en_US",
                name = "en-US-Wavenet-G"
            });
        }

        private void _gcTextToSpeech_SynthesizeSuccessEvent(FrostweepGames.Plugins.GoogleCloud.TextToSpeech.PostSynthesizeResponse response, long requestId)
        {
            ServiceLocator.Get<FrostweepGames.Plugins.GoogleCloud.TextToSpeech.IMediaManager>().
               SaveAudioFileAsFile(response.audioContent,
                                   Application.persistentDataPath,
                                   "1",
                                   FrostweepGames.Plugins.GoogleCloud.TextToSpeech.Enumerators.AudioEncoding.MP3);

        }

        private void _gcTextToSpeech_SynthesizeFailedEvent(string error, long requestId)
        {
            Debug.Log(error);
        }

        private void _gcTextToSpeech_GetVoicesFailedEvent(string error, long requestId)
        {
            Debug.Log(error);
        }

        private void _gcTextToSpeech_GetVoicesSuccessEvent(FrostweepGames.Plugins.GoogleCloud.TextToSpeech.GetVoicesResponse response, long requestId)
        {
            _voices = response.voices;
            Debug.Log(_voices[0].name);
        }

    }
