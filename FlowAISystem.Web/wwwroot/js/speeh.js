window.localTts = {
    // Fetches the array of available voices, handling async loading
    getVoices: function () {
        return new Promise((resolve) => {
            let voices = window.speechSynthesis.getVoices();
            if (voices.length !== 0) {
                resolve(voices.map(v => ({ name: v.name, lang: v.lang, voiceURI: v.voiceURI })));
                return;
            }

            // If voices aren't ready, listen for the event
            window.speechSynthesis.onvoiceschanged = () => {
                voices = window.speechSynthesis.getVoices();
                resolve(voices.map(v => ({ name: v.name, lang: v.lang, voiceURI: v.voiceURI })));
            };
        });
    },

    // Speaks text using a specific voice selected by voiceURI
    speakText: function (text, voiceUri) {
        return new Promise((resolve) => {
            if (!('speechSynthesis' in window)) {
                resolve({ success: false, error: 'Web Speech API not supported' });
                return;
            }

            const cleanText = text.replace(/<[^>]*>?/gm, '');
            const utterance = new SpeechSynthesisUtterance(cleanText);

            if (voiceUri) {
                const voices = window.speechSynthesis.getVoices();
                const selectedVoice = voices.find(v => v.voiceURI === voiceUri);
                if (selectedVoice) {
                    utterance.voice = selectedVoice;
                }
            }

            utterance.onend = () => resolve({ success: true });
            utterance.onerror = (e) => resolve({ success: false, error: e.error });

            window.speechSynthesis.cancel();
            window.speechSynthesis.speak(utterance);
        });
    }
};