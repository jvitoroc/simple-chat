﻿using NAudio.Wave;

namespace ClientForm
{
    public class AudioManager
    {
        private bool listening = false;
        private bool recording = false;

        private WaveIn recorder;

        private BufferedWaveProvider playerBufferedWaveProvider;
        private WaveOut player;

        public delegate void DataAvailableEventHandler(object sender, WaveInEventArgs waveInEventArgs);
        public event DataAvailableEventHandler DataAvailable;

        public void StartRecording()
        {
            if (!recording)
            {
                recorder = new WaveIn();
                //recorder.DeviceNumber = 0;
                recorder.BufferMilliseconds = 50;
                recorder.DataAvailable += OnDataAvailable;
                recorder.StartRecording();
                recording = true;
            }
        }

        public void StartListening()
        {
            if (!listening)
            {
                player = new WaveOut();
                player.Volume = 1;
                playerBufferedWaveProvider = new BufferedWaveProvider((new WaveInEvent()).WaveFormat);
                player.Init(playerBufferedWaveProvider);
                player.Play();
                listening = true;
            }
        }

        public void StopRecording()
        {
            if (recording)
            {
                recorder.Dispose();
                recording = false;
            }
        }

        public void StopListening()
        {
            if (listening)
            {
                player.Dispose();
                playerBufferedWaveProvider = null;
                listening = false;
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            DataAvailable?.Invoke(sender, waveInEventArgs);
        }

        public void Feed(byte[] audioBuffer, int audioLength)
        {
            if (listening)
                playerBufferedWaveProvider.AddSamples(audioBuffer, 0, audioLength);
        }
    }
}
