(function ($) {
    $.extend({
        bb_SignaturePad: function (el, obj,  alertText, backgroundColor) {
            //AlexChow: Code modify from https://github.com/szimek/signature_pad
            var wrapper = el;
            var clearButton = wrapper.querySelector("[data-action=clear]");
            var changeColorButton = wrapper.querySelector("[data-action=change-color]");
            var undoButton = wrapper.querySelector("[data-action=undo]");
            var saveBase64Button = wrapper.querySelector("[data-action=save-base64]");
            var savePNGButton = wrapper.querySelector("[data-action=save-png]");
            var saveJPGButton = wrapper.querySelector("[data-action=save-jpg]");
            var saveSVGButton = wrapper.querySelector("[data-action=save-svg]");
            var canvas = wrapper.querySelector("canvas");
            var signaturePad = new SignaturePad(canvas, {
                backgroundColor: backgroundColor || 'rgb(255, 255, 255)'
            });

            function resizeCanvas() {
                var ratio = Math.max(window.devicePixelRatio || 1, 1);

                canvas.width = canvas.offsetWidth * ratio;
                canvas.height = canvas.offsetHeight * ratio;
                canvas.getContext("2d").scale(ratio, ratio);
                signaturePad.clear();
            }

            window.onresize = resizeCanvas;
            resizeCanvas();

            function download(dataURL, filename) {
                if (navigator.userAgent.indexOf("Safari") > -1 && navigator.userAgent.indexOf("Chrome") === -1) {
                    window.open(dataURL);
                } else {
                    var blob = dataURLToBlob(dataURL);
                    var url = window.URL.createObjectURL(blob);

                    var a = document.createElement("a");
                    a.style = "display: none";
                    a.href = url;
                    a.download = filename;

                    document.body.appendChild(a);
                    a.click();

                    window.URL.revokeObjectURL(url);
                }
            }

            function dataURLToBlob(dataURL) {
                // Code taken from https://github.com/ebidel/filer.js
                var parts = dataURL.split(';base64,');
                var contentType = parts[0].split(":")[1];
                var raw = window.atob(parts[1]);
                var rawLength = raw.length;
                var uInt8Array = new Uint8Array(rawLength);

                for (var i = 0; i < rawLength; ++i) {
                    uInt8Array[i] = raw.charCodeAt(i);
                }

                return new Blob([uInt8Array], { type: contentType });
            }

            if (clearButton) clearButton.addEventListener("click", function (event) {
                signaturePad.clear();
                return obj.invokeMethodAsync("signatureResult", null);
            });

            if (undoButton) undoButton.addEventListener("click", function (event) {
                var data = signaturePad.toData();

                if (data) {
                    data.pop(); 
                    signaturePad.fromData(data);
                }
            });

            if (changeColorButton) changeColorButton.addEventListener("click", function (event) {
                var r = Math.round(Math.random() * 255);
                var g = Math.round(Math.random() * 255);
                var b = Math.round(Math.random() * 255);
                var color = "rgb(" + r + "," + g + "," + b + ")";

                signaturePad.penColor = color;
            });

            if (saveBase64Button) saveBase64Button.addEventListener("click", function (event) {
                if (signaturePad.isEmpty()) {
                    alertMessage();
                } else {
                    var imgBase64 = signaturePad.toDataURL();
                    return obj.invokeMethodAsync("signatureResult", imgBase64);
                }
            });

            if (savePNGButton) savePNGButton.addEventListener("click", function (event) {
                if (signaturePad.isEmpty()) {
                    alertMessage();
                } else {
                    var dataURL = signaturePad.toDataURL();
                    download(dataURL, "signature.png");
                }
            });

            if (saveJPGButton) saveJPGButton.addEventListener("click", function (event) {
                if (signaturePad.isEmpty()) {
                    alertMessage();
                } else {
                    var dataURL = signaturePad.toDataURL("image/jpeg");
                    download(dataURL, "signature.jpg");
                }
            });

            if (saveSVGButton) saveSVGButton.addEventListener("click", function (event) {
                if (signaturePad.isEmpty()) {
                    alertMessage();
                } else {
                    var dataURL = signaturePad.toDataURL('image/svg+xml');
                    download(dataURL, "signature.svg");
                }
            });

            function alertMessage() {
                if (alertText && alertText.length>0) alert(alertText);
                obj.invokeMethodAsync("signatureAlert");
            }
        }      
    });
})(jQuery); 
