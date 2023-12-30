using VComm.Core.Objects;

namespace VComm.Core.Functions
{
    internal class VoiceEngine
    {
        SpeechRecognitionEngine? engine;
        private IKeyboardMouseEvents? inputHook;
        public bool listening = false;

        /// <summary>
        /// Starts the VoiceEngine.
        /// </summary>
        public async Task StartEngine()
        {
            await this.Log("Starting VoiceEngine...");
            engine = new SpeechRecognitionEngine();
            GrammarBuilder builder = new GrammarBuilder();

            Choices vRequests = new Choices();

            foreach (VRequest vRequest in Variables.ActiveVPack.vRequests)
            {
                foreach (string phrase in vRequest.phrases)
                {
                    await this.Log($"Adding Engine listener for phrase: {phrase}");
                    vRequests.Add(phrase);
                }
            }

            builder.Append(vRequests);
            Grammar grammarWithDictation = new Grammar(builder);
            grammarWithDictation.Name = "VComm";
            grammarWithDictation.Enabled = true;

            engine.LoadGrammar(grammarWithDictation);

            engine.SetInputToDefaultAudioDevice();

            engine.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(Engine_SpeechHypothesized);
            engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Engine_SpeechRecognized);
            engine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(Engine_AudioLevelChange);
            engine.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>(Engine_AudioStateChanged);

            if (!Data.isPTTActive())
            {
                if (inputHook != null) await RemovePTTHooks();
                await StartListening();
            }
            else
            {
                await SubscribeToPTTHooks();
                await this.Log("PTT Active.");
            }
            
            await this.Log("VoiceEngine ready!");
        }

        /// <summary>
        /// Subscribes to the Push-To-talk Hook.
        /// </summary>
        public async Task SubscribeToPTTHooks()
        {
            inputHook = Hook.GlobalEvents();
            inputHook.KeyDown += OnKeyDown;
            inputHook.KeyUp += OnKeyUp;
        }

        /// <summary>
        /// Removed the Push-To-Talk Events and disposes of the Hook.
        /// </summary>
        public async Task RemovePTTHooks()
        {
            inputHook.KeyDown -= OnKeyDown;
            inputHook.KeyUp -= OnKeyUp;
            inputHook.Dispose();
        }

        /// <summary>
        /// Event handler for OnKeyDown. (used for Push-To-Talk)
        /// </summary>
        private async void OnKeyDown(object? sender, System.Windows.Forms.KeyEventArgs e)
        {
            
            if (Data.isPTTActive())
            {
                if (e.KeyValue == Data.getPTTKey() && !listening)
                {
                    await StartListening();
                }
            }
        }
        /// <summary>
        /// Event handler for OnKeyUp. (used for Push-To-Talk)
        /// </summary>
        private async void OnKeyUp(object? sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Data.isPTTActive())
            {
                if (e.KeyValue == Data.getPTTKey() && listening)
                {
                    await StopListening();
                }
            }
        }

        /// <summary>
        /// Allows the VoiceEngine to start listening.
        /// </summary>
        public async Task StartListening()
        {
            if (!listening)
            {
                listening = true;
                await this.Log("Started listening...");
                engine.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        /// <summary>
        /// Stops the VoiceEngine from listening.
        /// </summary>
        public async Task StopListening()
        {
            if (listening) 
            {
                listening = false;
                await this.Log("Stopped listening...");
                engine.RecognizeAsyncStop();
            }
        }

        /// <summary>
        /// Event handler for when the VoiceEnginer audio input stream changes.
        /// </summary>
        private async void Engine_AudioStateChanged(object? sender, AudioStateChangedEventArgs e)
        {
            await this.Log($"Audio State Changed: {e.AudioState}");

            if (e.AudioState == AudioState.Silence || e.AudioState == AudioState.Stopped) Variables.Overlay.Volume = 0;
        }

        /// <summary>
        /// Reloads the VoiceEngine. (have you tried turning it off and on again?)
        /// </summary>
        public async Task ReloadEngine()
        {
            await this.Log("Reloading the VoiceEngine...");
            await StopEngine();
            await StartEngine();
        }

        /// <summary>
        /// Shuts down the VoiceEngine and disposes of it cleanly.
        /// </summary>
        public async Task StopEngine()
        {
            await this.Log("Stopping VoiceEngine...");
            engine.UnloadAllGrammars();
            engine.Dispose();
            engine = null;
        }

        /// <summary>
        /// Event handler for when the audio input level/volume changes.
        /// </summary>
        public async void Engine_AudioLevelChange(object? sender, AudioLevelUpdatedEventArgs e)
        {
            Variables.Overlay.Volume = e.AudioLevel;
        }

        /// <summary>
        /// Event handler for when the VoiceEngine begins to recognize a Phrase. (partial match)
        /// </summary>
        public async void Engine_SpeechHypothesized(object? sender, SpeechHypothesizedEventArgs e)
        {
            string text = e.Result.Text + "?";
            await this.Log($"Detected text: {text}");

            await Variables.Overlay.SetSpeechText(e.Result.Text.ToUpper(), true);
        }

        /// <summary>
        /// Event handler for when the VoiceEnginer recognizes a matching phrase. (full match)
        /// </summary>
        public async void Engine_SpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            string request = e.Result.Text;
            string text = request + ".";
            await this.Log($"Detected text: {text}");
            
            Variables.Overlay.SetSpeechText(e.Result.Text.ToUpper());

            VRequest vRequest = null;

            vRequest = await Task.Run(async () => { return await FindVRequest(request); });

            Task.Run(async () => { await Chime(); });
            await this.Log($"Sending \"{vRequest.macro}\" from: {Variables.ActiveVPack}");
            Task.Run(async () => { await Simulate.Press(vRequest.macro); }); 
        }

        /// <summary>
        /// Finds a VRequest by providing a recognized Phrase.
        /// </summary>
        /// <param name="requestRecognized">The Phrase recognized</param>
        /// <returns></returns>
        public async Task<VRequest> FindVRequest(string requestRecognized)
        {
            VPack vPack = Variables.ActiveVPack;
            return vPack.vRequests.FirstOrDefault(r => r.phrases.FirstOrDefault(p => p == requestRecognized) == requestRecognized);
        }

        /// <summary>
        /// Play a chime. (*radio-check, over?*)
        /// </summary>
        public async Task Chime()
        {
            string chimePath = Variables.ChimePath;

            Assembly a = Assembly.GetExecutingAssembly();
            Stream s = a.GetManifestResourceStream(chimePath);

            SoundPlayer player = new SoundPlayer(s);
            player.PlaySync();
        }
    }

    
}
