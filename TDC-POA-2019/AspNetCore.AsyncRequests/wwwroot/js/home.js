// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var vueApp = new Vue({
    el: "#app",
    data: {
        status: {
            availableAsyncIOThreads: 0,
            availableWorkerThreads: 0
        },
        threadInfo: {

        },
        lastTime: moment(),
        polling: null,
        statusText: "",
        requests: [],
        asyncRequests: [],
        asyncCalls: 0
    },
    methods: {
        moment: function () {
            return moment();
        },
        checkStatus: function () {
            this.polling = setInterval(() => {

                axios
                    .get("/status")
                    .then(response => (this.status = response))
                    .then(() => this.statusText = moment.duration(moment().diff(this.lastTime)).asMilliseconds() + "ms ago")
                    .then(() => this.lastTime = moment())
                    .catch(error => console.table(error));

            }, 1000);
        },
        getThreadInfo: function () {
            axios
                .get("status/threads")
                .then(response => this.threadInfo = response)
                .then(r => this.asyncCalls = this.threadInfo.maxAsyncIOThreads + 32)
                .catch(error => console.table(error));
        },
        callSync: function (plusThreads) {
            this.requests = [];
            for (var i = 0; i < plusThreads; i++) {
                var id = this.requests.length;
                this.requests.push({
                    id: id,
                    status: "AWAITING",
                    start: moment(),
                    finished: false
                });

                axios
                    .get(`sync/5/${id}`)
                    .then(response => {
                        var request = this.requests.find(f => f.id === parseInt(response.id));
                        request.end = moment();
                        request.status = `DONE in ${moment.duration(request.end.diff(request.start)).asMilliseconds()} ms`;
                        request.finished = true;
                    })
                    .catch(error => console.table(error));
            }
        },
        callAsync: function (plusThreads) {
            this.asyncRequests = [];
            for (var i = 0; i < plusThreads; i++) {
                var id = this.asyncRequests.length;
                this.asyncRequests.push({
                    id: id,
                    status: "AWAITING",
                    start: moment(),
                    finished: false
                });

                jQuery.get(`async/5/${id}`)
                    .then(response => {
                        var request = this.asyncRequests.find(f => f.id === parseInt(response.id));
                        request.end = moment();
                        request.status = `DONE in ${moment.duration(request.end.diff(request.start)).asMilliseconds()} ms`;
                        request.finished = true;
                    })
                    .catch(error => console.table(error));
            }
        },

        getClass: function (request) {
            return request.finished ? "badge-success" : moment.duration(moment().diff(request.start)).asSeconds() > 1 ? "badge-primary" : "badge-warning";
        }
    },
    beforeDestroy() {
        clearInterval(this.polling);
    },
    created() {
        this.checkStatus();
        this.getThreadInfo();
    }
});