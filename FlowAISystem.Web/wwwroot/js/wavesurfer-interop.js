window.waveformInterop = {
    instance: null,
    regionsPlugin: null,

    init: function (elementId, waveColor, progressColor) {
        if (this.instance) {
            this.instance.destroy();
            this.instance = null;
            this.regionsPlugin = null;
        }

        // Ensure WaveSurfer and Regions plugin are available
        if (typeof WaveSurfer === 'undefined') {
            console.error("WaveSurfer is not loaded.");
            return;
        }

        // Initialize Regions Plugin (Adjust based on WaveSurfer v6 vs v7)
        this.regionsPlugin = window.WaveSurfer?.Regions ? WaveSurfer.Regions.create() : null;

        const plugins = this.regionsPlugin ? [this.regionsPlugin] : [];

        this.instance = WaveSurfer.create({
            container: '#' + elementId,
            waveColor: waveColor || '#4a90e2',
            progressColor: progressColor || '#50e3c2',
            height: 100,
            barWidth: 2,
            barGap: 3,
            cursorWidth: 2,
            plugins: plugins
        });
    },

    loadAudio: function (url) {
        if (this.instance) {
            this.instance.load(url);
        }
    },

    renderRegions: function (regions) {
        if (!this.regionsPlugin) return;
        this.regionsPlugin.clearRegions();

        if (!Array.isArray(regions)) return;

        regions.forEach(r => {
            const start = r.start !== undefined ? r.start : r.Start;
            const end = r.end !== undefined ? r.end : r.End;
            const color = r.color !== undefined ? r.color : (r.Color || 'rgba(239, 83, 80, 0.35)');

            this.regionsPlugin.addRegion({
                start: start,
                end: end,
                color: color,
                drag: false,
                resize: false
            });
        });
    },

    playPause: function () {
        if (this.instance) {
            this.instance.playPause();
        }
    }
};

window.downloadFileFromStream = async function (fileName, contentStreamReference) {
    try {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? 'file';
        document.body.appendChild(anchorElement);
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    } catch (err) {
        console.error("File download failed:", err);
    }
};

window.micRecorder = {
    mediaRecorder: null,
    audioChunks: [],
    mediaStream: null,

    startRecording: async function () {
        try {
            this.mediaStream = await navigator.mediaDevices.getUserMedia({ audio: true });
            this.audioChunks = [];
            
            this.mediaRecorder = new MediaRecorder(this.mediaStream);
            
            this.mediaRecorder.ondataavailable = (event) => {
                if (event.data && event.data.size > 0) {
                    this.audioChunks.push(event.data);
                }
            };

            this.mediaRecorder.start();
            return true;
        } catch (err) {
            console.error("Microphone access failed:", err);
            return false;
        }
    },

    stopRecording: async function () {
        return new Promise((resolve) => {
            if (!this.mediaRecorder) {
                resolve(null);
                return;
            }

            // Capture chunks when recorder stops
            let resolved = false;

            const processStop = async () => {
                if (resolved) return;
                resolved = true;

                try {
                    // Stop all tracks to release hardware
                    if (this.mediaStream) {
                        this.mediaStream.getTracks().forEach(track => track.stop());
                    }

                    if (!this.audioChunks || this.audioChunks.length === 0) {
                        resolve(null);
                        return;
                    }

                    const audioBlob = new Blob(this.audioChunks, { type: 'audio/webm' });
                    
                    if (audioBlob.size === 0) {
                        resolve(null);
                        return;
                    }

                    const reader = new FileReader();
                    reader.onloadend = () => {
                        const base64String = reader.result.split(',')[1];
                        resolve(base64String);
                    };
                    reader.onerror = () => resolve(null);
                    reader.readAsDataURL(audioBlob);
                } catch (e) {
                    resolve(null);
                }
            };

            this.mediaRecorder.onstop = processStop;

            try {
                if (this.mediaRecorder.state !== 'inactive') {
                    this.mediaRecorder.stop();
                    // Fallback timer in case onstop doesn't fire immediately
                    setTimeout(processStop, 300);
                } else {
                    resolve(null);
                }
            } catch (ex) {
                resolve(null);
            }
        });
    }
};