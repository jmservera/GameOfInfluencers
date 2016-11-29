var port = process.env.port || 1337;

var express = require('express')
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var path = require('path');

var timer=null;

app.get('/', function (req, res) {
    res.sendFile(__dirname + '/views/index.html');
});

app.use(express.static(path.join(__dirname, 'public')));


io.on('connection', function (socket) {
    console.log('a user connected: ' + socket.id);
    if (timer === null) {
        console.log("timer started");
        var count = 0;
        timer = setInterval(function () {
            console.log("emit " + count);
            io.emit("Message", "msg "+count++);
        },1000);
    }
    socket.on('disconnect', function () {
        console.log('user disconnected: ' + socket.id);
        console.log('sockets: ' + io.sockets.connected);
        //TODO: clear interval on listener
    });
});

http.listen(port, function () {
    console.log('listening on *:' + port);
});