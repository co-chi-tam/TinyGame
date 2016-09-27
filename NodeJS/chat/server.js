const  cluster 	= require('cluster');
const  numCPUs 	= require('os').cpus().length;

var clients = [];

if (cluster.isMaster) {
	console.log("======CONNECT OK=====");
	// Fork workers.
	for (var i = 0; i < numCPUs; i++) {
		cluster.fork();
	}
	cluster.on('online', function (worker, code, signal){
		console.log("Worker Online");
	});
	cluster.on('exit', function (worker, code, signal){
		console.log("Worker ${worker.process.pid} died");
		cluster.fork();
	});
} else { 
	var ipaddress = process.env.OPENSHIFT_NODEJS_IP || "127.0.0.1";
	var port      = process.env.OPENSHIFT_NODEJS_PORT || 8080;

	var WebSocketServer = require('ws').Server;
	var http = require('http');
	var connectInfo = {
		server: process.env.OPENSHIFT_NODEJS_IP,
		port: process.env.OPENSHIFT_NODEJS_PORT
	};

	var server = http.createServer(function(request, response) {
		console.log((new Date()) + ' Received request for ' + request.url);
		response.writeHead(200, {'Content-Type': 'text/plain'});
		  response.write("Welcome to Node.js on OpenShift!\n\n");
		  response.end("Thanks for visiting us! \n");
	});

	server.listen( port, ipaddress, function() {
		console.log((new Date()) + ' Server is listening on port 8080');
	});

	wss = new WebSocketServer({
		server: server,
		autoAcceptConnections: false
	});
	wss.on('connection', function(ws) {
	  console.log("New connection");
	  clients.push(ws);
	  
	  ws.on('message', function(message) {
		wss.clients.forEach(function each(client) {
			client.send(message);
		  });
	  });
	  
	  connectInfo['status'] = 1;
	  connectInfo['connects'] = wss.clients.length;
	  ws.send(parseToMessage("message", connectInfo));
	});

	wss.on('disconnect', function(ws) {
	  console.log("Disconection");
	  for(var i = 0; i < clients.length; i++) {
        // # Remove from our connections list so we don't send
		if(clients[i] == ws) {
		  clients.splice(i);
		  break;
		}
	  }
	});

	console.log("Listening to " + ipaddress + ":" + port + "...");

	function message(client, data) {
		client.send(data);
	}
	
	function all_client(client, data) {
		client.send(data);
	}
	
	function broadcast(name, data) {
	  wss.clients.forEach(function each(client) {
		client.send(parseToMessage(name, data));
	  });
	};
	
	function parseToMessage(name, data) {
		return "42[\"" + name + "\"," + JSON.stringify(data)+"]";
	}
}