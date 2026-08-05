console.log("FlowAI Voice JS Loaded");


window.flowAIVoice = {


    // ==========================
    // STATE
    // ==========================

    initialized:false,

    recognition:null,

    synth:window.speechSynthesis,

    voices:[],

    dotNet:null,


    isRunning:false,

    isPaused:false,

    isSpeaking:false,


    restartTimer:null,



    // ==========================
    // WAKE WORD
    // ==========================

    backgroundMode:false,


    wakeWords:[
        "flow ai",
        "flowai",
        "hey flow ai",
        "hey flowai"
    ],



    // ==========================
    // SAFE DOTNET CALLBACK
    // ==========================

    callDotNet(method,...args)
    {

        if(!this.dotNet)
            return;


        try
        {

            this.dotNet.invokeMethodAsync(
                method,
                ...args
            );

        }
        catch(error)
        {

            console.log(
                "DotNet callback error",
                error
            );

        }

    },



    // ==========================
    // INITIALIZE
    // ==========================

    initialize(dotNetHelper)
    {


        console.log(
            "FlowAI Initialize"
        );



        if(this.initialized)
            return;



        this.dotNet =
            dotNetHelper;



        const SpeechRecognition =
            window.SpeechRecognition ||
            window.webkitSpeechRecognition;



        if(!SpeechRecognition)
        {

            console.error(
                "Speech Recognition not supported"
            );

            return;

        }



        this.recognition =
            new SpeechRecognition();



        this.recognition.continuous=true;


        this.recognition.interimResults=true;


        this.recognition.maxAlternatives=1;



        this.recognition.lang="en-US";



        this.loadVoices();



        if(this.synth)
        {

            this.synth.onvoiceschanged=()=>{

                this.loadVoices();

            };

        }




        // ==========================
        // START
        // ==========================

        this.recognition.onstart=()=>{


            console.log(
                "Recognition started"
            );


            this.isRunning=true;


            this.callDotNet(
                "VoiceStarted"
            );


        };




        // ==========================
        // END
        // ==========================

        this.recognition.onend=()=>{


            console.log(
                "Recognition ended"
            );



            if(
                this.isRunning &&
                !this.isPaused &&
                !this.isSpeaking
            )
            {

                clearTimeout(
                    this.restartTimer
                );


                this.restartTimer =
                setTimeout(()=>{


                    try
                    {

                        this.recognition.start();

                    }
                    catch
                    {

                    }


                },500);


            }


        };




        // ==========================
        // ERROR
        // ==========================

        this.recognition.onerror=(event)=>{


            console.log(
                "Voice Error:",
                event.error
            );



            this.callDotNet(
                "VoiceError",
                event.error
            );


        };





        // ==========================
        // RESULT
        // ==========================

        this.recognition.onresult=(event)=>{


            let text="";


            for(
                let i=event.resultIndex;
                i<event.results.length;
                i++
            )
            {

                text +=
                event.results[i][0]
                .transcript;

            }



            console.log(
                "Voice Result:",
                text
            );



            let language =
                this.detectLanguage(text);



            this.callDotNet(
                "SpeechStreaming",
                text,
                language
            );




            let last =
            event.results[
                event.results.length-1
            ];



            if(!last.isFinal)
                return;




            // ==========================
            // WAKE WORD MODE
            // ==========================

            if(this.backgroundMode)
            {


                let command =
                text
                .toLowerCase()
                .trim();



                let found =
                this.wakeWords.some(word=>
                    command.includes(word)
                );



                if(found)
                {


                    console.log(
                        "Wake word detected"
                    );


                    this.backgroundMode=false;



                    this.callDotNet(
                        "WakeWordDetected"
                    );



                    // Remove wake word

                    command =
                    command
                    .replace("hey flow ai","")
                    .replace("hey flowai","")
                    .replace("flow ai","")
                    .replace("flowai","")
                    .trim();



                    if(command.length>0)
                    {

                        this.callDotNet(
                            "SpeechCompleted",
                            command,
                            this.detectLanguage(command)
                        );

                    }


                }



                return;

            }





            // ==========================
            // NORMAL SPEECH
            // ==========================


            console.log(
                "Sending Question:",
                text
            );



            this.callDotNet(
                "SpeechCompleted",
                text,
                language
            );


        };




        this.initialized=true;



        console.log(
            "FlowAI Voice Ready"
        );


    },





    // ==========================
    // VOICES
    // ==========================

    loadVoices() {

        this.voices = this.synth.getVoices();

        console.log("Available Voices");

        this.voices.forEach(v => {

            console.log(v.name + " | " + v.lang);

        });

    },





    // ==========================
    // BACKGROUND LISTEN
    // ==========================

    startBackgroundListening()
    {


        console.log(
            "Background listening"
        );



        this.backgroundMode=true;



        this.start();


    },



    // ==========================
    // START
    // ==========================

    start()
    {


        if(!this.recognition)
            return;



        this.isRunning=true;


        this.isPaused=false;



        try
        {

            this.recognition.start();

        }
        catch(error)
        {

            console.log(
                "Recognition already active"
            );

        }


    },






    // ==========================
    // STOP
    // ==========================

    stop()
    {


        console.log(
            "Voice stopped"
        );


        this.isRunning=false;


        this.isPaused=false;


        this.backgroundMode=false;



        clearTimeout(
            this.restartTimer
        );



        if(this.synth)
        {

            this.synth.cancel();

        }



        try
        {

            this.recognition?.stop();

        }
        catch
        {

        }


    },






    // ==========================
    // PAUSE
    // ==========================

    pause()
    {


        this.isPaused=true;



        try
        {

            this.recognition?.stop();

        }
        catch
        {

        }


    },







    // ==========================
    // RESUME
    // ==========================

    resume()
    {


        if(this.isSpeaking)
            return;



        this.isPaused=false;


        this.isRunning=true;



        this.start();


    },







    // ==========================
    // SPEAK
    // ==========================

    speak(text, language) {

        if (!text)
            return;

        this.synth.cancel();

        const clean = this.cleanText(text);

        const utter = new SpeechSynthesisUtterance(clean);

        language = language || "en-US";

        utter.lang = language;

        // ------------------------
        // Voice Selection
        // ------------------------

        let voice = null;

        if (language.startsWith("km")) {

            voice =
                this.voices.find(v =>
                    v.lang &&
                    v.lang.toLowerCase().startsWith("km"));

            if (!voice) {

                console.warn("Khmer voice not installed.");

            }

        }
        else {

            voice =
                this.voices.find(v =>
                    v.lang &&
                    v.lang.toLowerCase().startsWith("en"));

        }

        if (voice)
            utter.voice = voice;

        utter.rate = 0.95;
        utter.pitch = 1;
        utter.volume = 1;

        utter.onstart = () => {

            this.isSpeaking = true;

            this.pause();

            this.callDotNet("AISpeakingStarted");

        };

        utter.onend = () => {

            this.isSpeaking = false;

            this.callDotNet("AISpeakingFinished");

            if (!this.backgroundMode) {

                setTimeout(() => {

                    this.resume();

                }, 300);

            }

        };

        utter.onerror = (e) => {

            console.log("Speech Error", e);

            this.isSpeaking = false;

            this.callDotNet("AISpeakingFinished");

        };

        this.synth.speak(utter);

    },





    // ==========================
    // CLEAN TEXT
    // ==========================

    cleanText(text)
    {

        return text

        .replace(
            /[\u{1F300}-\u{1FAFF}]/gu,
            ""
        )

        .replace(
            /[#*_`>-]/g,
            ""
        )

        .replace(
            /\s+/g,
            " "
        )

        .trim();

    },







    // ==========================
    // LANGUAGE
    // ==========================

    detectLanguage(text) {

        if (!text)
            return "en-US";

        const khmer =
            /[\u1780-\u17FF]/.test(text);

        return khmer
            ? "km-KH"
            : "en-US";
    }


};