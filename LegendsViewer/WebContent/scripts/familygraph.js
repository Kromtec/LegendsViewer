document.addEventListener('DOMContentLoaded', function () {
    var leader = 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAHsSURBVEhLYxgFgwIIQWlaAGEQ4QnEMSAGEcASiokB8UDsBWIYAPFXIDYEcfAAaXEhhufiwgxPgGxJiBBOYALE34FYB8QRkZdk+CfEx/AQyBYFCWABHOxsDMf3z2L4v3cGw39WZoajQDE2iBQGEBURYHgoJ8HwF8jmBgmwq8oxfN42ieE/ExPDXiCfBSSIBmZOLGH4//8sBPcVMfwHiUGkUAAr0IwDW4FmyYozvAXymcCiQNc/+HOa4X9rNlhjH1gQAdJivRn+/zuDsADEjvYEq02DKAEDRiCe2JbD8P/7MYb/3JwMNyDCQMDIyLD/0yGG/3+BlgQ5MfwDCsEi3dpIg+HXN6AGmOEwDBIz1mT4AVQDi/S4UBeGfyDLX+5m+A+0bQ9UHAzmX1wB0QiySFsJHEE+EsBIfbQV03AYvr+Z4b+oAMNToFovXRWG71+PQsTPLQX7bj7IYBho3tiP0HhrPcN/YIr5v28mQgwX3jMdaIkgRA9MDGQWyEyI0RCQPrUCVePrvah8fPjVHlQ+yCyQmRCjIcCrMhFVESUYZBbQTFAGhgO9GC/sisnBILOAZqpDjIYAMQdjhr/YFJODHU0YfgHN5IcYDQEsUqIMP9f3MvynBpYWY/gANJMZYjQCFANxA5VwARCPgpEBGBgAxBHRUN6IEB4AAAAASUVORK5CYII=)';
    var necromancer = 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAYuSURBVEhLnZV5VFR1FMeHOmG2/JGZ23FN3Eg7JwJST0c6isfcSg21TEotMFxyRXNCJ8AtQBSRlF3WGWSZUZCB2ZgZkFmYTQVkGHYVDBQVNbPT8dv9PcYt01Pdcz7nvfd7937v73d/73cf71mWFb+5f3Fq6Axp5p6DipyIyzppEoyyNJiUmagsToD2ZFyzIvdQ+KnkHz88cUTwmjPsX5mLJIH/lUYcW9FaZ/hDIdqPeqsSN6524EbXpV6uXsb1zou42tEEx1n1HVn2Xrk4kT+fxfZKPNtcTibxo3XFybfv3L4Bu1kBWdYeXGq0oa3eTFQ9hpkbv3mtA3d6rqHi1LHugvhts506/2guhenhQXaz/N5vPd246LBAeSICJlUWd99qNz5NnQEtF/S40laLnu4rVL6snoKEkHlMq1fykbmI4rattGhy73W1N6DNXgWzWsSJy4X7YNXkoMUp1lyr+xuVHGxV3b+2oFKa/LsoLngB0+yVJhMEfz5EmhF++SbVtlfkDM7rTpGoDnXmUsiy98CmzaNnPTfG3jfVVDxFW70Jt6934nR6aHlw8KrXOfHFPN6LkqSQ+Dqb9s9WhwkN5zWEFg3VjHLuXpUbBVXeAbpGwlCSimp9ISWWo95W1uvrxHFOg0tNNlQbS+4K47YtInkX3hHBmkFlBXH6unNnUGdVkBMLUhMaNNZoSTAZ5yvF3LPdpsAFUwklOIXaqmJKoODGe1FzsXbSuGBVozBz/+EDm/z68lKi1k/VK7OvG9V5qLPI0EKiHY16dLWacK3Ngq5mI9ob9GgiARZcb1Oh2elzlfkQXa1VaHfo0FStppXJYK44CWXB4Wb+xoWDecf2Bc3XKYUwqHJoVlIcjQnHuLFucHV1hWufPtx1svf7KJeJSFxO370SmcnRGDvmSR9vz/dQlJeIekspTFoJNCfj8K3/7LG8w2GBCzSnU6BXCSEVJ2P8hPEYNeptuLm5wW3MGIwe7Yahw4YjLSGSViBDw1k5/Jf5Ydjw4Q992HXEyFGY/IEXqg2FMKpzoco/BP+lvhMoQdACpeQodMpsiIVxmDjpXXh5ecHb25vD09MT03x8oCoW0uxK4LCWIPFYDDw8PLh3D/0oZtx4d5yrlEBPFVHkRj9KoBLH4UxpOjQl2Vi8ZCl8fX0xa9YszJw5E75ETEw0ak1yFGTGoDDnF7TS57hz507uHfNhvixmzdp19HlLUCnLgOJEVG+Cn0O+mUP1ultenELLK0KptAhbg4MREBAIPp+PMpUS7U0W7A/dgv79+2PUyBEozotHR8s5FBUVYsOGjQgIDOQSWqrY+SlAZWkaZKKI636fTB3HW7tinocsJ8qhlSbDohWhjQ5SD/WXW9QubnZ3oLW2AjUU9OXSBZjg/g7tkTtiI0NwQS/BpXoDem50cr6sCTpoj6waESpKUpATv1Pq4z1pKM996NB++Qk75EZVNqyUgEMjhLksg9pyPPTSBKgKkzB3zsfwob2YNm0aln+xGLbyHOjoHcOqzuJibM54PbX12LDA7QMHDnyVl3Foi9fp9J8626h5mcnRps2h4BMoyz9AvSgbFgrUSNOx3P9rREVFQCAQYPPmrdw489OIY6CRxHAJWSzTYOelMFVQK9iyfABvU8Cnk8QpITWsqcmFe6HOj+YoyQzjTi0TqtaLcY36f+fFGnRdtqOr3UFiQqq3GLriJM73QRzrW8T9rNhgyRwfr0GsHbGmNDHIf8763KM/mBMjNxQs+2z6d4kR3xeZlBlUqnRUKY+jSpECo5MqRSo3ZtVkQys5jNjw1RlB/rM3p8dsLcs9tsO0YvGMVaQ5hniFJWBttQ/xJjGSGEa8tWvjMv/SrN1UpnSncDKMcifsnsYMsiT6Ie2Gr4/HZIoZQIxwarxBvEQ88V9gDy84ry6xoasXlmaFw079iXVRtidmVRoMpYncxrJSnKdDxSax0m+6+2PxDzSeb4vmTRmfenBTbn48v1EuirylLjh0/2x5HvfTN8iO3z+dFnYzL55vPxi6OsrdfQRX6/9qLxPDPpo80Wf9yvlB/HVLBNuD/MIYO9b67VrjPzdgiue4qeTDxF1ZwP8xtty+BNufwcQQJ0y0H8Em8Zxy8Hh/AePT7xrkZYHBAAAAAElFTkSuQmCC)';
    var vampire = 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAQWSURBVEhLnVVbSFxXFL0zGRFCoBmF1o8GKzqjZZwa1H5USj5CAhZhKlJbFY0j1hgVn63ph7F1qthqmCYfEmnRWEURraC0ovGBoh92JJEqFN+jlakYH5D6qEa0ru597h0xSX1MFyzOPvt5zz773iudgHNElSyeCo2yuoQk4oeyeCKMxGJZdAGekvSIlgh5dyLefl2SphT5zDj/sVr9nNZweXsi9AmSBFr95O2r+ISYTfyGWEN8pFarx35QqaDVaid8fHxser3eZjAYbEajUTAwMNAWEBBg8/X1tXl5eY0+pAIatXqSY4k/Er8lck7OLf1dXFyMqakprKysYHd3F9Y7d/CrVovOzk6cBo5rvXAB1hs3ROzy8jImJiaQlZX1D+fmArPV1dWKu4yvoqLwOCIC7e3tiuZ4jI+Po+3KFXwZFqZoZJSXl3Pb7Fygu6ysTFEDe3t7uHXtGn6/dw+1tbU4ODgQZNjtdkGGU9/U1ISfb99GSWQkFhcXhY1RUFDABbq5wP3c3FxFDYyOjqKAAuxjY7BarcjPz9/nQpubmwgKChohPmG5rq4OOTk5e42Njehpa8PDBw/Q2tqqZAFSUlK4wH0uYI6OjlbUQFVVFerr67G6ugq6YHaK8fT0/D42NpblQGZcXBxYR/K7fn5+W8PDw+jt7UVJSYmSBQgPD2d/M1EyhIaGKmrAYrGgo6NDtIqmaZDsIUR+q78jvqaQZdYxumZmZkRMUVGRkgWgyXM+kKSmcXym6FFYWIiWlhYh+/v7V7DDEagVHkXe1tYW+vr6kJmZKeJ4mjQazTOyHfr+srS0JC4tOTkZXV1dwtFkMnWQzZ34DvE88X1iEPENIiNMp9NVsC/fXWJiomgty2Rrl11k5PX09GBhYQFmsxmDg4OiAF3in/QkX5A9mY5cSy3LJDnC29t7gFYP2g/FxMQI5+npaWRkZKC/vx81NTVc4DPiIYw8t/zkaWlpGBgYEAUaGhoQHBz8Gw3BYnNzM6Kioh7TvsVms4Hu7Y/4+Pi/SktLn7Pv5OQkTxwqKytFDsrJpz6EKjEhwVFBs5+dnX04btxbnoa1tTXs7++LoztfPn7rk5KSMD8/L/ZDQ0NIT08XQxISEuLgnHJqBQadrvLrmzcxOzsrEjnhcDgU6VUctfEbzcxKTcU5lapSSfsCrsddvaq4/398IM//dTnli9Bc9vB4aqE+8iXxpfNJuAXcjvX1dWxvb2NnZ0esGxsbQj83N4eRkRHROuvdu/C9dOkp55JTvoRkd3drt5sbP8GnxPeIJmI8MZXIn1+ejHzi58Qc4i0i2/nHlDdw8SLedHOzknws9GTlz+xZfpUvw2SRY/Xy9hjoJOknWqLlnUv4yCDHnoq3iJGy6BL41Bx7Jjg/ZK7gP2Ik6V89SVqJoK3vSAAAAABJRU5ErkJggg==)';
    var werebeast = 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAALhSURBVEhLrVbNSmpRFPZJus9gZmGS4L9NIhQKSQdOhDBBEpUyCpLURkWpNFDIUMOgTBv1QD1Bw6J197c4+7D3Oda9cO+Cz3PO2mt962f/6fhJXC7XgkB5cXHxVTzfxPMDMN6hK8PGMP97cTqdvwTBUOBzbW2NMpkMHR4eUr1eZ+AdOozBBrbwMdx/FmG8I7J6j0Qi1Gg0aDqd0svLCz9rtRr1ej3+ljrYwBY+8DVo5otR8tfe3h49Pz/TcDhk0oeHByYSY+R2u+ns7MwMAsAWPvAFh0Gni5H519HRkemYTCaZdGtri6rVKr8Dy8vLdHt7qwUB4AsOWyXonxh4RxaqQzweZ8KlpSU6ODgwAwCoSLWVMCp51+YEk4Q+olQYNZtNarfb1Ol0yOPxcMatVosDgTyVSpm2VkAPLnAyuXBYEB+fMqPJZMJE6PXl5SWNRiO6ubmh2WzG391u10ZqBbjACW6eWCw3uVqAUCjEmW5ubrJ+f3+fvF4v6xAc+p8CwQec4EZ7XrGm5SAm7+TkhLa3t+n8/JxyuRwTWxGLxTRSK8AJblTwho0DJSKvrKxwz6+vrzmY7LsVgUDARqoCnMLuDRV8YHfKAWQOgkQiQaVSSSNVcXx8rBFaAU5w2wI8Pj5SoVCgcrlM6XTaRozqKpWKRjYPZgDhZLbICmwwlbhYLNLd3R2PjcdjbsPp6Sk9PT3ZfNUWaZOsQq0ArZN6udblWDQapcFgoPmqk2xbphJolSQJBoOmDTKWy1YCq036actU/GgbTcXV1ZVGsru7yxsOY/1+XwuSz+dNP22jQcSHdlRIgAzlSxJgfX2dW+fz+TT9xcUF+9iOCsh3hx2AKnBsqGRWbGxsmJXNPewgIqLtuJbA4be6umojBvx+v7myvj2upYhB7cJRg+DQQ5/D4TDvbkxiNpul+/t7tjUy//7CkWJUYrsy5wFjsDGW7J+vTCnoHyZJ4P9f+qqIrP7xb4vD8RsUqEDd6L8qtAAAAABJRU5ErkJggg==)';
    var ghost = 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuOWwzfk4AAAQtSURBVEhLnVZJbFtFGDa0tGUTEpCc4MoBiVy4RFwicai4NT3kgnqoglUoKgdEGgQCGagavL7YbhtVCOgiurhBsRtFarO0JS2tiVclQWlcN4uTponXeHfsPHuYb96MHbeAC7/06b03nv/7/u3Ns6qRaTSa7aZe/YejYyPjd5y3UxTZ8Vs3PGar1E1/e45v+39mNBpfdVwecCWTyUqpVCIbGxsUBVIsFkk+nyNXh6/co3ve4Nv/m3V0dGy7POS4tbm5SUqlIkWJX4tUQBHC/bXrYwGdTvcid3tyM5kNH6TTKRY5RGRZZhCCQgRZnTrzYw93e3K7efOGV5BRIUpcIuVymQnQkpFCIc/IC4UCcU7cTqFX3LWxabXal3x+Nw2+SOz2AdLc3EwMBgMTcDjspKmpieh0Wi6QJw8frshak7aVuze2o4ajby6GFkoow+HDXaSl5S3Svredlaj63L6HlQgC2WyGmK2mfdy9sWmNR94Jh9cqIHA675BDn3zMIkc/nE4ne0ZmIMc0oUxU4BB3b2ySpN+TSqHOykjKMnpRmyL0AdmBPJfLslIZJX03d29sdHMXCMWUQKQGZXpE9BDA2vE+q5a7NzbHoP00pgaOIBWRC5GtAqg/rsMjV0a4+78bJsjjdSUVUuUdgJgokcggn8+z6DOZNBtjl3si19PT8wqn+Wez9V/oByHqjqnBaOJevBNiNEGezaYJepVMJpjQuQtnh+j78DSnetzMx6SPYrFoWRBXKpWqADJA9GIsQZ5OK+SJRIysr8fJ6upK2XrC/PfNpqV5ze/3phEpCMvl2tGwtbGIfG1ttUoOYggAePb6PEW9Wd/CaWt29pfTJ0SUW5uqRKs0UkQeDN6rRv4oMpkUuWg7N8RpFdNc0uzAWY8Ia8ixqMPhVVZfpZlJWvN1MjnpJ7FYpI4Y+8S9z+8tGY1HXuf09OSk58jSckgW0QIgzOVyZG4uWG0kyoGrz+chDx4s1QlEo+HqWiQSJpLFeIDTo7mm92lzGREiBDB6iUSczMz8WVdnwONxUeH7dWtAIHCXxONRlqnO8P03nF6lOvlDnwTySGStSoYslpdDZHp6qo4kHo8Rt9tFZmdn6tYBCCALBEjH/SojN1lMbbOBu5tQhbMQQFPhMDU1WUeCTF2uCZbZ1nVgfv4+WViYYxyhpUXZYpHUKmOvrt0xOBCUzHqL2drbT6MuYwMygIDL/YcsosJ6NBohdJzpJAXI5JRfnpsPVkR/0HiLVRrTfPu11nbp/O8mk+4rlgW1pyi2t7a2vtz/q22afs2K9Hsc+vyLzwy739vd2ane/93xvmO/jY4NFygqF23nF7u6P/2p7d02tfpA55enzvw8fu36aGJwyL5y8KD6bcq1i2Ib533MsIjXHRvwGdxBAQf8RXme4gV+fZZiJ8UzFNiH/Y8cEyrVX3mEHgOFKQYMAAAAAElFTkSuQmCC)';

    var cy = cytoscape({
        container: document.getElementById('familygraph'),

        boxSelectionEnabled: false,
        autounselectify: true,
        userZoomingEnabled: true,
        wheelSensitivity: 0.1,
        zoom: 1,

        style: cytoscape.stylesheet()
          .selector('node')
            .css({
                'shape': 'roundrectangle',
                'width': 120,
// ReSharper disable InvalidValue
                'height': 'label',
                'content': 'data(name)',
                'background-color': 'data(faveColor)',
                'text-wrap': 'wrap',
// ReSharper restore InvalidValue
                'font-family': 'Helvetica',
                'font-size': 12,
                'text-valign': 'center',
                'text-max-width': 100,
                'padding-top': 5,
                'padding-bottom': 5,
                'color': '#ffffff',
                'border-width': 1,
                'border-style': 'solid',
                'border-color': '#888888',
                'shadow-blur': 5,
                'shadow-color': '#888888',
                'shadow-offset-x': 3,
                'shadow-offset-y': 3,
                'shadow-opacity': 0.8
            })
          .selector('node.vampire')
            .css({
                'shape': 'hexagon',
                'background-blacken': 0.2,
                'background-image': vampire,
                // ReSharper disable InvalidValue
                'background-clip': 'none',
                // ReSharper restore InvalidValue
                'background-width': '24px',
                'background-height': '24px',
                'background-position-x': '-15px',
                'padding-left': 30,
                'padding-right': 30,
                'border-style': 'dotted',
                'text-weight': 'bold'
            })
          .selector('node.werebeast')
            .css({
                'shape': 'hexagon',
                'background-blacken': 0.1,
                'background-image': werebeast,
                // ReSharper disable InvalidValue
                'background-clip': 'none',
                // ReSharper restore InvalidValue
                'background-width': '24px',
                'background-height': '24px',
                'background-position-x': '-15px',
                'padding-left': 30,
                'padding-right': 30,
                'border-style': 'dotted',
                'text-weight': 'bold'
            })
          .selector('node.necromancer')
            .css({
                'shape': 'hexagon',
                'background-blacken': 0.1,
                'background-image': necromancer,
                // ReSharper disable InvalidValue
                'background-clip': 'none',
                // ReSharper restore InvalidValue
                'background-width': '24px',
                'background-height': '24px',
                'background-position-x': '-15px',
                'padding-left': 30,
                'padding-right': 30,
                'border-style': 'dotted',
                'text-weight': 'bold'
            })
          .selector('node.ghost')
            .css({
                'shape': 'hexagon',
                'background-blacken': -0.1,
                'background-image': ghost,
                // ReSharper disable InvalidValue
                'background-clip': 'none',
                // ReSharper restore InvalidValue
                'background-width': '24px',
                'background-height': '24px',
                'background-position-x': '-15px',
                'padding-left': 30,
                'padding-right': 30,
                'border-style': 'dotted',
                'text-weight': 'bold'
            })
          .selector('node.leader')
            .css({
                'shape': 'octagon',
                'background-image': leader,
                // ReSharper disable InvalidValue
                'background-clip': 'none',
                // ReSharper restore InvalidValue
                'background-width': '24px',
                'background-height': '24px',
                'background-position-x': '-20px',
                'padding-left': 25,
                'padding-right': 25,
                'padding-top': 15,
                'padding-bottom': 15,
                'border-width': 2,
                'border-style': 'dotted',
                'text-weight': 'bold'
            })
          .selector('node.current')
            .css({
                'border-style': 'dashed',
                'color': '#222299'
            })
          .selector('node.dead')
            .css({
                'background-blacken': -0.4,
                'background-opacity': 0.75,
                'shadow-opacity': 0
            })
          .selector('edge')
            .css({
                'width': 1,
                'target-arrow-shape': 'triangle',
                'curve-style': 'bezier'
            })
          .selector(':selected')
            .css({
                'background-color': 'black',
                'line-color': 'black',
                'target-arrow-color': 'black',
                'source-arrow-color': 'black'
            }),

        elements: {
            nodes: window.familygraph_nodes,
            edges: window.familygraph_edges
        },

        layout: {
            name: 'dagre',
            fit: false,
            padding: 30
        }
    });

    cy.center();

    cy.on('tap', 'node', function () {
        window.location.href = this.data('href');
    });
});
