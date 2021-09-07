(function ($) {
    $.extend({
        bb_screencapture: function (el, method,media) {
            const video = document.getElementById("video");
            const logElem = document.getElementById("log");
            //const startElem = document.getElementById("start");
            //const stopElem = document.getElementById("stop");

            const canvas = document.getElementById('canvas');
            const photo = document.getElementById('photo');
            const startbutton = document.getElementById('startbutton');
            const startstream = document.getElementById('startstream');

            var width = 320;    // We will scale the photo width to this
            var height = 0;     // This will be computed based on the input stream

            var streaming = false;

            // Options for getDisplayMedia()

            var displayMediaOptions = {
                video: {
                    cursor: "always"
                },
                audio: false
            };

            if (method === 'start') {
                startCapture();
            }
            else if (method === 'stop') {
                stopCapture();
            } else {
            //    // Set event listeners for the start and stop buttons
            //    startElem.addEventListener("click", function (evt) {
            //        startCapture();
            //    }, false);

            //    stopElem.addEventListener("click", function (evt) {
            //        stopCapture();
            //    }, false);
            }

            console.log = msg => logElem.innerHTML += `${msg}<br>`;
            console.error = msg => logElem.innerHTML += `<span class="error">${msg}</span><br>`;
            console.warn = msg => logElem.innerHTML += `<span class="warn">${msg}<span><br>`;
            console.info = msg => logElem.innerHTML += `<span class="info">${msg}</span><br>`;

            async function startCapture() {
                logElem.innerHTML = "";

                try {
                    if (media === 'camera')
                        video.srcObject = await navigator.mediaDevices.getUserMedia(displayMediaOptions);
                    else
                        video.srcObject = await navigator.mediaDevices.getDisplayMedia(displayMediaOptions);

                    video.play();
                    dumpOptionsInfo();
                } catch (err) {
                    console.error("Error: " + err);
                }
            }
            function stopCapture(evt) {
                let tracks = video.srcObject.getTracks();

                tracks.forEach(track => track.stop());
                video.srcObject = null;
            }
            function dumpOptionsInfo() {
                const videoTrack = video.srcObject.getVideoTracks()[0];

                console.info("Track settings:");
                console.info(JSON.stringify(videoTrack.getSettings(), null, 2));
                console.info("Track constraints:");
                console.info(JSON.stringify(videoTrack.getConstraints(), null, 2));
            }

            video.addEventListener('canplay', function (ev) {
                if (!streaming) {
                    height = video.videoHeight / (video.videoWidth / width);

                    video.setAttribute('width', width);
                    video.setAttribute('height', height);
                    canvas.setAttribute('width', width);
                    canvas.setAttribute('height', height);
                    streaming = true;
                }
            }, false);

            startbutton.addEventListener('click', function (ev) {
                takepicture();
                ev.preventDefault();
            }, false);

            startstream.addEventListener('click', function (ev) {
                takestream();
                ev.preventDefault();
            }, false);

            function clearphoto() {
                var context = canvas.getContext('2d');
                context.fillStyle = "#AAA";
                context.fillRect(0, 0, canvas.width, canvas.height);

                var data = canvas.toDataURL('image/png');
                photo.setAttribute('src', data);
            }

            function takepicture() {
                var context = canvas.getContext('2d');
                if (width && height) {
                    canvas.width = width;
                    canvas.height = height;
                    context.drawImage(video, 0, 0, width, height);

                    var data = canvas.toDataURL('image/png');
                    photo.setAttribute('src', data);
                } else {
                    clearphoto();
                }
            }

            function takestream() {
                var stream = canvas.captureStream(25); // 25 FPS
                console.log(stream);
                //photo.setAttribute('src', stream);
            }
        }
    });
})(jQuery);
