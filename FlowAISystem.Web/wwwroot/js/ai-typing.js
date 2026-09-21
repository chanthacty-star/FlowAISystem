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


// // ==============================
// /**
//  * AI Chat Typing Utility for Blazor Integration
//  */
// window.aiTyping = {
//   // Flag container for active instances
//   activeTasks: new Map(),

//   typeHTML: async function (element, html, speed = 15) {
//     if (!element) return;

//     // Create a unique token for this typing execution
//     const taskId = Symbol("typingTask");
//     this.activeTasks.set(element, taskId);

//     const temp = document.createElement("div");
//     temp.innerHTML = html;

//     if (window.addCodeCopyButtons) {
//       window.addCodeCopyButtons(temp);
//     }

//     element.innerHTML = temp.innerHTML;

//     const walker = document.createTreeWalker(element, NodeFilter.SHOW_TEXT);
//     const textNodes = [];
//     let currentNode;

//     while ((currentNode = walker.nextNode())) {
//       textNodes.push({
//         node: currentNode,
//         fullText: currentNode.textContent
//       });
//       currentNode.textContent = "";
//     }

//     const container = element.closest(".ai-messages") || element.parentElement;

//     if (window.Prism) {
//       element.querySelectorAll("pre code").forEach((block) => {
//         Prism.highlightElement(block);
//       });
//     }

//     for (const item of textNodes) {
//       for (let j = 0; j < item.fullText.length; j++) {
//         // Check if task was aborted mid-typing
//         if (this.activeTasks.get(element) !== taskId) {
//           // Instantly reveal the remaining text upon cancellation
//           this.revealRemainingText(textNodes);
//           return;
//         }

//         item.node.textContent += item.fullText[j];

//         if (container) {
//           container.scrollTop = container.scrollHeight;
//         }

//         await new Promise((resolve) => setTimeout(resolve, speed));
//       }
//     }

//     // Clean up task token when typing completes naturally
//     if (this.activeTasks.get(element) === taskId) {
//       this.activeTasks.delete(element);
//     }
//   },

//   // Immediately stop animation for a specific element
//   cancelTyping: function (element) {
//     if (!element) return;
//     this.activeTasks.delete(element);
//   },

//   // Helper to instantly reveal full text when user clicks Stop
//   revealRemainingText: function (textNodes) {
//     textNodes.forEach((item) => {
//       item.node.textContent = item.fullText;
//     });
//   }
// };