#!/bin/env node
//  OpenShift sample Node application
const  express 	= require('express');
const  fs      	= require('fs');
const  cluster 	= require('cluster');
const  numCPUs 	= require('os').cpus().length;
const  bodyParser = require('body-parser');
const  http = require('http');

// URL helper TinyGame
require("./models/database.js")();
require('./models/webSocketClient')();
const  urlHelper = require("./routes.js");
var connectInfo = {
	server: process.env.OPENSHIFT_NODEJS_IP,
	port: process.env.OPENSHIFT_NODEJS_PORT || 8080
};
/**
 *  Define the sample application.
 */
var SampleApp = function(database) {

    //  Scope.
    var self = this;


    /*  ================================================================  */
    /*  Helper functions.                                                 */
    /*  ================================================================  */

    /**
     *  Set up server IP address and port # using env variables/defaults.
     */
    self.setupVariables = function() {
        //  Set the environment variables we need.
		self.ipaddress = process.env.OPENSHIFT_NODEJS_IP;
        self.port      = process.env.OPENSHIFT_NODEJS_PORT || 8080;
        if (typeof self.ipaddress === "undefined") {
            //  Log errors on OpenShift but continue w/ 127.0.0.1 - this
            //  allows us to run/test the app locally.
            console.warn('No OPENSHIFT_NODEJS_IP var, using 127.0.0.1');
            self.ipaddress = "127.0.0.1";
        };
    };


    /**
     *  Populate the cache.
     */
    self.populateCache = function() {
        if (typeof self.zcache === "undefined") {
            self.zcache = { 'index.html': '' };
        }

        //  Local cache for static content.
        self.zcache['index.html'] = fs.readFileSync('./index.html');
    };


    /**
     *  Retrieve entry (content) from cache.
     *  @param {string} key  Key identifying content to retrieve from cache.
     */
    self.cache_get = function(key) { return self.zcache[key]; };


    /**
     *  terminator === the termination handler
     *  Terminate server on receipt of the specified signal.
     *  @param {string} sig  Signal to terminate on.
     */
    self.terminator = function(sig){
        if (typeof sig === "string") {
           console.log('%s: Received %s - terminating sample app ...',
                       Date(Date.now()), sig);
           process.exit(1);
        }
        console.log('%s: Node server stopped.', Date(Date.now()) );
    };


    /**
     *  Setup termination handlers (for exit and a list of signals).
     */
    self.setupTerminationHandlers = function(){
        //  Process on exit and signals.
        process.on('exit', function() { self.terminator(); });

        // Removed 'SIGPIPE' from the list - bugz 852598.
        ['SIGHUP', 'SIGINT', 'SIGQUIT', 'SIGILL', 'SIGTRAP', 'SIGABRT',
         'SIGBUS', 'SIGFPE', 'SIGUSR1', 'SIGSEGV', 'SIGUSR2', 'SIGTERM'
        ].forEach(function(element, index, array) {
            process.on(element, function() { self.terminator(element); });
        });
    };


    /*  ================================================================  */
    /*  App server functions (main app logic here).                       */
    /*  ================================================================  */

    /**
     *  Create the routing table entries + handlers for the application.
     */
    self.createRoutes = function() {
        self.routes = { };

        self.routes['/asciimo'] = function(req, res) {
            var link = "http://i.imgur.com/kmbjB.png";
            res.send("<html><body><img src='" + link + "'></body></html>");
        };

        self.routes['/'] = function(req, res) {
            res.setHeader('Content-Type', 'text/html');
            res.send(self.cache_get('index.html') );
        };
    };

    /**
     *  Initialize the server (express) and create the routes and register
     *  the handlers.
     */
    self.initializeServer = function() {
        self.createRoutes();
		
		self.app 	= express();
		self.app.use(bodyParser.urlencoded({ extended: false }));
		self.app.use(bodyParser.json());
		self.app.use(express.static(__dirname + "/public"));

        //  Add handlers for the app (from the routes).
        for (var r in self.routes) {
            self.app.get(r, self.routes[r]);
        }
    };

    /**
     *  Initializes the sample application.
     */
    self.initialize = function() {
        self.setupVariables();
        self.populateCache();
        self.setupTerminationHandlers();
		
        // Create the express server and routes.
        self.initializeServer();
    };

    /**
     *  Start the server (starts up the sample application).
     */
    self.start = function() {
		print("========== Start server OK ===========");
		
		wss.initializeWebSocketServer(function() {
			print ("WEBSOCKET OPEN");
		}, function(mess) {
			print ("WEBSOCKET MESSAGE " + mess);
		}, function() {
			print ("WEBSOCKET CLOSE");
		}, function(err){
			print ("WEBSOCKET ERROR " + err);
		});
		
		// Start the app on the specific interface (and port).
		self.app.listen(self.port, self.ipaddress, function() {
			console.log('%s: Node server started on %s:%d ...', Date(Date.now()), self.ipaddress, self.port);	
		});
		// URL helper
		urlHelper.setRequestUrl (self.app, database, wss);
    }
	
};   /*  Sample Application.  */


/**
 *  main():  Main code.
 */
if (cluster.isMaster) {
	print("======CONNECT OK=====");
	// Fork workers.
	for (var i = 0; i < numCPUs; i++) {
		cluster.fork();
	}
	cluster.on('online', function (worker, code, signal){
		print("Worker Online");
	});
	cluster.on('exit', function (worker, code, signal){
		print("Worker ${worker.process.pid} died");
		cluster.fork();
	});
} else { 
	if (db.getDatabase()) {
		print("======WORKER OK=====");
		var zapp = new SampleApp(db);
		zapp.initialize();
		zapp.start();
	} else {
		print("======WORKER OK=====");
		db.connectDB(function() {
			print("===================== CONNECT OK =====================");
			var zapp = new SampleApp(db);
			zapp.initialize();
			zapp.start();
		}, function(errorConnect) {
			print("Connect error " + errorConnect);
		});
	}
}	

function parseToMessage(data) {
	return "42[\"message\"," + JSON.stringify(data)+"]";
}

