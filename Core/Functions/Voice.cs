using VComm.Core.Objects;

namespace VComm.Core.Functions
{
    internal class VoiceEngine
    {
        SpeechRecognitionEngine? engine;
        private IKeyboardMouseEvents? inputHook;
        public bool listening = false;

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

        public async Task SubscribeToPTTHooks()
        {
            inputHook = Hook.GlobalEvents();
            inputHook.KeyDown += OnKeyDown;
            inputHook.KeyUp += OnKeyUp;
        }

        public async Task RemovePTTHooks()
        {
            inputHook.KeyDown -= OnKeyDown;
            inputHook.KeyUp -= OnKeyUp;
            inputHook.Dispose();
        }

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

        public async Task StartListening()
        {
            if (!listening)
            {
                listening = true;
                await this.Log("Started listening...");
                engine.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        public async Task StopListening()
        {
            if (listening) 
            {
                listening = false;
                await this.Log("Stopped listening...");
                engine.RecognizeAsyncStop();
            }
        }

        private async void Engine_AudioStateChanged(object? sender, AudioStateChangedEventArgs e)
        {
            await this.Log($"Audio State Changed: {e.AudioState}");

            if (e.AudioState == AudioState.Silence || e.AudioState == AudioState.Stopped) Variables.Overlay.Volume = 0;
        }

        public async Task ReloadEngine()
        {
            await this.Log("Reloading the VoiceEngine...");
            await StopEngine();
            await StartEngine();
        }

        public async Task StopEngine()
        {
            await this.Log("Stopping VoiceEngine...");
            engine.UnloadAllGrammars();
            engine.Dispose();
            engine = null;
        }

        public async void Engine_AudioLevelChange(object? sender, AudioLevelUpdatedEventArgs e)
        {
            Variables.Overlay.Volume = e.AudioLevel;
        }

        public async void Engine_SpeechHypothesized(object? sender, SpeechHypothesizedEventArgs e)
        {
            string text = e.Result.Text + "?";
            await this.Log($"Detected text: {text}");

            await Variables.Overlay.SetSpeechText(e.Result.Text.ToUpper(), true);
        }

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

        public async Task<VRequest> FindVRequest(string requestRecognized)
        {
            VPack vPack = Variables.ActiveVPack;
            return vPack.vRequests.FirstOrDefault(r => r.phrases.FirstOrDefault(p => p == requestRecognized) == requestRecognized);
        }

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
