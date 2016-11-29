var http = require('http');
var serviceFabric = require('azure-servicefabric');

var port = process.env.port || 1337;
http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<html><body><h1>Hello World\n');
}).listen(port);

console.log("Server listening on port "+port);