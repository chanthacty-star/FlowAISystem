window.codeExample = {

    highlight: function () {

        if (typeof Prism === "undefined") {
            return;
        }

        Prism.highlightAll();
    },

    highlightElement: function (element) {

        if (typeof Prism === "undefined") {
            return;
        }

        if (!element) {
            return;
        }

        Prism.highlightElement(element);
    }
};