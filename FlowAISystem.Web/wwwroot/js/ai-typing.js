window.aiTyping = {

    typeText: async function (element, text, speed = 20, dotnetRef) {

        element.innerHTML = "";

        for (let i = 0; i < text.length; i++) {

            element.innerHTML += text[i];

            if(dotnetRef)
            {
                await dotnetRef.invokeMethodAsync("ScrollToBottom");
            }

            await new Promise(resolve =>
                setTimeout(resolve, speed)
            );
        }

    }

};