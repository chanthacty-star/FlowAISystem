window.typeHTML = async function (
    element,
    html,
    speed = 15
) {

    const temp =
        document.createElement("div");


    temp.innerHTML = html;


    element.innerHTML =
        temp.innerHTML;



    const walker =
        document.createTreeWalker(
            element,
            NodeFilter.SHOW_TEXT
        );



    const textNodes = [];


    while(walker.nextNode())
    {
        textNodes.push(
            walker.currentNode
        );
    }



    const texts =
        textNodes.map(
            node => node.textContent
        );



    textNodes.forEach(node =>
    {
        node.textContent = "";
    });





    for(let i = 0; i < textNodes.length; i++)
    {

        const node =
            textNodes[i];


        const text =
            texts[i];



        for(let j = 0; j < text.length; j++)
        {

            node.textContent += text[j];


            await new Promise(
                resolve =>
                    setTimeout(resolve, speed)
            );



            // Auto scroll during typing

            const container =
                element.closest(".ai-messages");


            if(container)
            {
                container.scrollTop =
                    container.scrollHeight;
            }

        }

    }


    // Re-highlight code blocks

    if(window.Prism)
    {
        Prism.highlightAll();
    }

    window.copyCode = async function(button)
    {
        const codeBlock =
            button.parentElement
                .querySelector("code");


        if(!codeBlock)
            return;


        const text =
            codeBlock.innerText;


        await navigator.clipboard.writeText(text);



        const oldText =
            button.innerHTML;


        button.innerHTML = "✅ Copied";


        setTimeout(() =>
        {
            button.innerHTML = oldText;

        },1500);

    };

};





// Scroll after loading old conversation

window.scrollChatBottom = function(element)
{

    if(element)
    {
        element.scrollTop =
            element.scrollHeight;
    }

};
// Add copy buttons to code blocks
window.addCodeCopyButtons = function(container)
{
    if(!container)
        return;


    const blocks =
        container.querySelectorAll("pre");


    blocks.forEach(block =>
    {

        if(block.querySelector(".copy-code-btn"))
            return;



        const button =
            document.createElement("button");


        button.className =
            "copy-code-btn";


        button.innerHTML =
            "📋 Copy";



        button.onclick = function()
        {
            window.copyCode(button);
        };



        block.style.position =
            "relative";


        block.appendChild(button);

    });

};