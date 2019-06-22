
let express = require('express');
let bodyParser = require('body-parser');

const routes = () => {
    let router = express.Router();
    router.use(bodyParser.json());

    // all controllers with the actual logic to handle the request 
    let authController = rootRequire('controllers/authController.js')();
    let saveFileController = rootRequire('controllers/saveFileController.js')();
    let assetsController = rootRequire('controllers/assetsController.js')();

    // define options for all endpoints 
    router.options('/verify', (req, res) => {
        res.header('Allow', 'GET,OPTIONS');
        res.send();
    });

    router.options('/saveData', (req, res) => {
        res.header('Allow', 'GET, POST, PUT ,OPTIONS');
        res.send();
    });

    router.options('/assets/:type', (req, res) => {
        res.header('Allow', 'GET, POST, PUT ,OPTIONS');
        res.send();
    });
    // put in a piece of middleware to add some kind of security and get userdata.
    router.use(authController.verify);
    
    // simple response that gives back the user 
    router.route('/verify')
    .get((req, res) => { 
        res.status(200).json({
            success: true,
            user: req.user
        });
    });
    // connect the method and url to a function.
    router.route('/saveData')
        .get(saveFileController.getOwnSaveFile)
        .put(saveFileController.setSaveFile);
    
    router.route('/saveData/:mode')
        .get(saveFileController.getOwnSaveFile)
        .put(saveFileController.setSaveFile);
    
    router.route('/assets/:type')
        .get(assetsController.getAssetListOfType);
        // .put(assetsController.addAsset);
    
    return router;
};

module.exports = routes;
