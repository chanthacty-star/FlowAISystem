export class AudioWordSyncPlayer {
    constructor(dotNetHelper) {
        this.dotNetHelper = dotNetHelper;
        this.audioElement = new Audio();
        this.animationFrameId = null;
        this.lastReportedTimeMs = 0;
        this.currentBlobUrl = null;

        this.audioElement.onplay = () => this.startSyncLoop();
        this.audioElement.onpause = () => this.stopSyncLoop();
        this.audioElement.onended = () => {
            this.stopSyncLoop();
            this.dotNetHelper.invokeMethodAsync('OnPlaybackEnded');
        };
    }

    loadAudio(audioBase64, contentType) {
        // Clean up previous blob URL to prevent memory leaks
        if (this.currentBlobUrl) {
            URL.revokeObjectURL(this.currentBlobUrl);
        }

        // Convert base64 to Blob URL for fast binary loading
        const byteCharacters = atob(audioBase64);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: contentType });

        this.currentBlobUrl = URL.createObjectURL(blob);
        this.audioElement.src = this.currentBlobUrl;
        this.audioElement.load();
    }

    play() {
        this.audioElement.play();
    }

    pause() {
        this.audioElement.pause();
    }

    startSyncLoop() {
        const update = () => {
            if (!this.audioElement.paused) {
                const currentTimeMs = this.audioElement.currentTime * 1000;

                // Throttle updates: send to Blazor only if 40ms (~25 FPS) has passed
                if (Math.abs(currentTimeMs - this.lastReportedTimeMs) >= 40) {
                    this.lastReportedTimeMs = currentTimeMs;
                    this.dotNetHelper.invokeMethodAsync('UpdatePlaybackTime', currentTimeMs);
                }

                this.animationFrameId = requestAnimationFrame(update);
            }
        };
        this.animationFrameId = requestAnimationFrame(update);
    }

    stopSyncLoop() {
        if (this.animationFrameId) {
            cancelAnimationFrame(this.animationFrameId);
            this.animationFrameId = null;
        }
    }

    dispose() {
        this.stopSyncLoop();
        this.audioElement.pause();
        if (this.currentBlobUrl) {
            URL.revokeObjectURL(this.currentBlobUrl);
        }
    }
}

export function initAudioPlayer(dotNetHelper) {
    return new AudioWordSyncPlayer(dotNetHelper);
}