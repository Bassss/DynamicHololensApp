let express = require('express');
let bodyParser = require('body-parser');
let authController = rootRequire('controllers/authController.js')();
let routes = () =>
{
    let router = express.Router();
    router.use(bodyParser.json());
    // if the request url contains / (all of them) pass it through the verify function.
    router.options('/', (req, res) => {
        res.header('Allow', 'POST,OPTIONS');
        res.send();
    });
    
    router.route('/')
        .post((req, res, next) =>  authController.verify);
    

    return router;
};

module.exports = routes;