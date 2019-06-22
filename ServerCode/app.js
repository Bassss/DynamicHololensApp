// Global definitions.
const rootDir = __dirname;
global.rootRequire = name => { // Require from root instead of relative path (makes for easier require statements).
    return require(rootDir + '/' + name);
};
global.Promise = require("bluebird"); // Overriding default promise library. Tested this, _seems_ to work :).
global.APP_CONFIG = require('./config.json'); // Declaring and importing the APP_CONFIG object.

// Require statements.
let path = require('path');
let express = require('express')
let multer = require('multer');
let app = express();
let fs = require('fs');
let morgan = require('morgan');
// had some old certs 
let server = require('https').Server({
    key: fs.readFileSync("certs/prod/privkey.pem"),
    cert: fs.readFileSync("certs/prod/fullchain.pem")
}, app);

// log all HTTP requests for development purposes.
if (APP_CONFIG.DEV_LOGGING) {
    app.use(morgan('dev'));
}
// create a storage space with multer in the directory assets 
var storage = multer.diskStorage({
    destination: function (req, file, cb) {
        cb(null, 'assets')
    },
    filename: function (req, file, cb) {
        cb(null, file.fieldname)
    }
})
var upload = multer({ storage: storage })
// give a notice that the api is accessible using the right url 
app.get('/', (req, res) => {
    res.send(`Please use /api/${APP_CONFIG.API_VERSION} to access the API, use /upload to add assets`);
});
// instead of of going Through a router we need to check if the user is using /upload so we can send a html page back instead.
app.get('/upload', function (request, response) {
    // send back the html file
    response.sendfile('uploadPage.html');
});
// if the page is requested using the post method it means we need to handle a file upload
app.post('/upload', upload.single("myFiles"), (req, res, next) => {
    const files = req.file
    // check if a file is really uploaded
    if (!files) {
        const error = new Error('Please choose files')
        error.httpStatusCode = 400
        return next(error)
    }
    //to make sure the right name is applied to the file we rename it to be sure
    fs.rename(files.path, "assets/" + files.originalname, function (error) {
        if (error) {
            return res.status(402).json({
                success: false,
                message: "could not make file"
            });
        }
    });
    let assets = [];
    let url;
    let body = req.body;
    // we check for a directory with name of the user
    fs.mkdir('./assetHelpers/' + body.user + '/', function (e) {

        if (!e || (e && e.code === 'EEXIST')) {
            // check if file of type exists
            fs.readFile('./assetHelpers/' + body.user + '/' + body.type + '.json', 'utf-8', function (error, data) {
                // if it exists we get the existing data 
                if (error == null) {
                    if (data != undefined) {
                        assets = JSON.parse(data);
                    }
                }
                // we push our new asset data in the array with the old data
                url = "http://"+APP_CONFIG.IP+":"+(APP_CONFIG.PORT + 1)+"/assets/"+files.originalname;
                assets.push({
                    "name": body.name,
                    "url": url
                })
                // we write all the data to the file
                fs.writeFile('./assetHelpers/' + body.user + '/' + body.type + '.json', JSON.stringify(assets, true, true), function (error) {
                    if (error == null) {
                        // we return the data to the user as conformation.
                        return res.status(200).json({
                            success: true,
                            files
                        });
                    }
                });
            });
        } else {
            return res.status(500).json({
                success: false,
                message: 'I dont know what happend but something went wrong when looking for a directory!',
                error: e
            });
        }
    });
  

})
// we set the header for the responses of the whole server 
app.use((req, res, next) => {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PATCH, DELETE');
    next();
});
// we make sure that /assets is publicly accessible so Unity or other application can get the data.
app.use("/assets", express.static(path.join(__dirname, 'assets')));
// for all other urls that get trown at the server we user the apirouter to handle the requests.
app.use(`/api/${APP_CONFIG.API_VERSION}`, require('./routers/apiRouter')());
//create the server (https)
server.listen(APP_CONFIG.PORT, APP_CONFIG.IP, () => {
    console.log(`https listening on *: ${APP_CONFIG.PORT}`);
});
// create the server (http) in the event that some https is unavalible 
const httpServer = require('http').createServer(app);
httpServer.listen(APP_CONFIG.PORT + 1, APP_CONFIG.IP, () => {
    console.log(`http listening on *: ${APP_CONFIG.PORT + 1} `);
});

